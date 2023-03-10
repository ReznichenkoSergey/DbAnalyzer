using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces;
using DbAnalyzer.Core.Models.Parsers;
using DbAnalyzer.Core.Models.ReportModels;
using DbAnalyzer.Core.Models.ReportModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace DbAnalyzer.Core.Infrastructure.Reports.DbProcedures
{
    public class ProceduresUsageReport : IProceduresUsageReport
    {
        private readonly IProcedureExplorer _procedureExplorer;
        private readonly IExecPlanExplorer _execPlanExplorer;
        private readonly ILogger<ProceduresUsageReport> _logger;
        public List<ReportItem> ReportItems { get; private set; }

        public ProceduresUsageReport(IProcedureExplorer procedureExplorer,
            IExecPlanExplorer execPlanExplorer,
            ILogger<ProceduresUsageReport> logger)
        {
            _procedureExplorer = procedureExplorer;
            _execPlanExplorer = execPlanExplorer;
            _logger = logger;
        }

        public async Task<Report?> GetReportAsync()
        {
            var report = new Report();
            try
            {
                var reportItems = new List<IReportItem>();
                var parser = new ExecPlanParser();
                var procNames = await _procedureExplorer.GetProcNamesFromDbAsync();
                var procExecStatistics = await _procedureExplorer.GetFullExecutionStatisticsAsync();

                await Parallel.ForEachAsync(procNames, async (procName, token) =>
                {
                    var planResult = await _execPlanExplorer.GetExecPlanAsync(procName);

                    if (!string.IsNullOrEmpty(planResult?.Content) && planResult.Content.Contains("IndexKind=\"NonClustered\"", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var list = parser.GetUsedNonclusterIndexes(planResult.Content);

                    }

                    if (planResult.ExecPlanResultType == ExecPlanResultType.Success && !string.IsNullOrEmpty(planResult.Content))
                    {
                        var missingIndexes = parser.GetMissingIndexes(planResult.Content);
                        var procStat = procExecStatistics.FirstOrDefault(x => x.ProcedureName.Equals(procName));
                        var stat = "not found";
                        if (procStat != null)
                        {
                            stat = $"ExecCount= {procStat.ExecCount}, LastExecTime= {procStat.LastExecTime}, CreateDate= {procStat.CreateDate}";
                        }

                        if (missingIndexes.Any())
                        {
                            foreach (var index in missingIndexes)
                            {
                                reportItems.Add(new ReportItem()
                                {
                                    ReportItemStatus = ReportItemStatus.Success,
                                    Annotation = $"Procedure {procName}, impact: {index.Impact}%, statistics: {stat}",
                                    Query = index.Query
                                });
                            }
                        }
                        else
                        {
                            reportItems.Add(new ReportItem()
                            {
                                ReportItemStatus = ReportItemStatus.None,
                                Annotation = $"Procedure {procName}, no indexes need, statistics: {stat}"
                            });
                        }
                        //
                        var missingStatistics = parser.GetMissingStatistics(planResult.Content);
                        if (missingStatistics.Any())
                        {
                            foreach (var index in missingStatistics)
                            {
                                reportItems.Add(new ReportItem()
                                {
                                    ReportItemStatus = ReportItemStatus.Warning,
                                    Annotation = "Missing statistics",
                                    Query = index
                                });
                            }
                        }
                    }
                    else
                    {
                        reportItems.Add(new ReportItem()
                        {
                            ReportItemStatus = ReportItemStatus.Error,
                            Annotation = $"Procedure {procName}, execution plan generation error: {planResult.Content}"
                        });
                    }
                });
                report.ReportItems = reportItems;
                report.Result = new List<string>()
                {
                    $"Errors: {reportItems.Where(x=>x.ReportItemStatus == ReportItemStatus.Error).Count()}",
                    $"Warnings: {reportItems.Where(x=>x.ReportItemStatus == ReportItemStatus.Warning).Count()}",
                    $"Optimizations: {reportItems.Where(x=>x.ReportItemStatus == ReportItemStatus.Success).Count()}"
                };
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"NewIndexReport; Generate, Error= {ex.Message}");
                return report;
            }
        }
    }
}
