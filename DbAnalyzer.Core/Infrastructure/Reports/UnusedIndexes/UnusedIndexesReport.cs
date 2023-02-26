using DbAnalyzer.Core.Models.ReportModels.Interfaces;
using DbAnalyzer.Core.Models.ReportModels;
using Microsoft.Extensions.Logging;
using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.Models.Helpers;

namespace DbAnalyzer.Core.Infrastructure.Reports.UnusedIndexes
{
    public class UnusedIndexesReport : IReportGenerator<Report, UnusedIndexesQueryDto>
    {
        private readonly IIndexExplorer _indexExplorer;
        private readonly ILogger<UnusedIndexesReport> _logger;
        public List<ReportItem> ReportItems { get; private set; }

        public UnusedIndexesReport(IIndexExplorer indexExplorer,
            ILogger<UnusedIndexesReport> logger)
        {
            _indexExplorer = indexExplorer;
            _logger = logger;
        }

        public async Task<Report> GetReportAsync(UnusedIndexesQueryDto dto)
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
                                Annotation = dto.ShowAnnotation ? x.GetAnnotation() : string.Empty,
                                Query = dbScriptGenerator.GetDropIndexScript(x.IndexName, x.TableName)
                            });
                        });
                }
                report.ReportItems = reportItems;
                report.Result = new List<string>()
                {
                    $"Warnings: {reportItems.Where(x=>x.ReportItemStatus == ReportItemStatus.Warning).Count()}",
                    $"Released space: {spaceAmount} Kb"
                };
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetReportAsync (UnusedIndexesReport), Error= {ex.Message}");
                return report;
            }
        }
    }
}
