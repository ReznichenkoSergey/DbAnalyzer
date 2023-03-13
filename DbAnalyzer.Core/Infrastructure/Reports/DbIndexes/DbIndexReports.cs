using DbAnalyzer.Core.Models.ReportModels.Interfaces;
using DbAnalyzer.Core.Models.ReportModels;
using Microsoft.Extensions.Logging;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.Models.Helpers;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Models;
using DbAnalyzer.Core.Models.Parsers;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces;

namespace DbAnalyzer.Core.Infrastructure.Reports.DbIndexes
{
    public class DbIndexReports : IDbIndexReports
    {
        private readonly IIndexExplorer _indexExplorer;
        private readonly IProcedureExplorer _procedureExplorer;
        private readonly IExecPlanExplorer _execPlanExplorer;
        private readonly ILogger<DbIndexReports> _logger;
        public List<ReportItem> ReportItems { get; private set; }

        public DbIndexReports(IIndexExplorer indexExplorer,
            IProcedureExplorer procedureExplorer,
            IExecPlanExplorer execPlanExplorer,
            ILogger<DbIndexReports> logger)
        {
            _indexExplorer = indexExplorer;
            _procedureExplorer = procedureExplorer;
            _execPlanExplorer = execPlanExplorer;
            _logger = logger;
        }

        public async Task<Report> GetUnusedIndexesReportAsync()
        {
            var report = new Report();
            try
            {
                var spaceAmount = 0;
                var reportItems = new List<IReportItem>();
                var items = await _indexExplorer.GetUnusedIndexesAsync();
                if (items != null && items.Any())
                {
                    var dbScriptGenerator = new DbScriptGenerator();
                    items
                        .ToList()
                        .ForEach(x =>
                        {
                            spaceAmount += x.IndexSize;
                            reportItems.Add(new ReportItem()
                            {
                                ReportItemStatus = ReportItemStatus.Warning,
                                Annotation = x.GetAnnotation(),
                                Query = dbScriptGenerator.GetDropIndexScript(x.IndexName, x.TableName)
                            });
                        });
                }
                report.ReportItems = reportItems;
                report.Result = new List<string>()
                {
                    $"Warnings: {reportItems.Where(x=>x.ReportItemStatus == ReportItemStatus.Warning).Count()}",
                    $"Released space: {Math.Round((decimal)spaceAmount/1024)} Kb"
                };
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetReportAsync (UnusedIndexesReport), Error= {ex.Message}");
                return report;
            }
        }

        public async Task<IList<NonclusterIndexUsageStatistic>> GetNonClusterIndexesReportAsync()
        {
            var list = new List<NonclusterIndexUsageStatistic>();
            try
            {
                var parser = new ExecPlanParser();
                var procNames = await _procedureExplorer.GetProcNamesFromDbAsync();

                await Parallel.ForEachAsync(procNames, async (procName, token) =>
                {
                    var planResult = await _execPlanExplorer.GetExecPlanAsync(procName);

                    if (!string.IsNullOrEmpty(planResult?.Content) && planResult.Content.Contains("IndexKind=\"NonClustered\"", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var indexes = parser.GetUsedNonclusterIndexes(planResult.Content);
                        if (indexes != null && indexes.Any())
                        {
                            list.Add(new NonclusterIndexUsageStatistic()
                            {
                                ProcedureName = procName,
                                Indexes = indexes
                            });
                        }
                    }
                });
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetReportAsync; Generate, Error= {ex.Message}");
                return null;
            }
        }

        public async Task<Report> GetDublicateIndexesReportAsync()
        {
            try
            {
                var report = new Report()
                {
                    ReportItems = new List<IReportItem>()
                };

                var items = await _indexExplorer.GetDublicateIndexesAsync();
                if (items != null && items.Any())
                {
                    DbIndex? temp = null;
                    var dbScriptGenerator = new DbScriptGenerator();
                    items
                        .OrderBy(x => x.TableName)
                        .ThenBy(x => x.KeyColumnList)
                        .ThenBy(x => x.IncludeColumnList)
                        .ToList()
                        .ForEach(x =>
                        {
                            if (temp != null && temp.Equals(x))
                            {
                                report.ReportItems.Add(new ReportItem()
                                {
                                    ReportItemStatus = ReportItemStatus.Warning,
                                    Query = dbScriptGenerator.GetDropIndexScript(x),
                                    Annotation = $"A copy of {temp.IndexName} index"
                                });
                            }
                            temp = x;
                        });
                }
                report.Result = new List<string>()
                {
                    $"Warnings: {report.ReportItems.Where(x=>x.ReportItemStatus == ReportItemStatus.Warning).Count()}"
                };
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetReportAsync (DublicateIndexes), Error= {ex.Message}");
                return null;
            }
        }
    }
}
