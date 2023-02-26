﻿using DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries.Interfaces;
using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;
using DbAnalyzer.Core.Models.Parsers;
using DbAnalyzer.Core.Models.ReportModels;
using DbAnalyzer.Core.Models.ReportModels.Interfaces;
using Microsoft.Extensions.Logging;

namespace DbAnalyzer.Core.Infrastructure.Reports.Procedures
{
    public class ExpensiveQueriesReport : IReportGenerator<Report, ExpensiveQueriesReportDto>
    {
        private readonly IDbQueryExplorer _dbQueryExplorer;
        private readonly ILogger<ProceduresReport> _logger;
        public List<ReportItem> ReportItems { get; private set; }

        public ExpensiveQueriesReport(IDbQueryExplorer dbQueryExplorer,
            ILogger<ProceduresReport> logger)
        {
            _dbQueryExplorer = dbQueryExplorer;
            _logger = logger;
        }

        public async Task<Report?> GetReportAsync(ExpensiveQueriesReportDto queryParams)
        {
            var report = new Report();
            try
            {
                var reportItems = new List<IReportItem>();
                var parser = new ExecPlanParser();
                var queries = await _dbQueryExplorer.GetExpensiveQueryListAsync(queryParams.Ordering, queryParams.TakeTopAmount, true);
                
                Parallel.ForEach(queries, (query, token) =>
                {
                    var missingIndexes = parser.GetMissingIndexes(query.QueryPlan);
                    if (missingIndexes.Any())
                    {
                        foreach (var index in missingIndexes)
                        {
                            reportItems.Add(new ReportItem()
                            {
                                ReportItemStatus = ReportItemStatus.Success,
                                Annotation = $"Query: {query.CompleteQueryText}, impact: {index.Impact}%",
                                Query = index.Query
                            });
                        }
                    }
                    var missingStatistics = parser.GetMissingStatistics(query.QueryPlan);
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
