using DbAnalyzer.Core.Models.ReportModels.Interfaces;
using DbAnalyzer.Core.Models.ReportModels;
using Microsoft.Extensions.Logging;
using DbAnalyzer.Core.Models.ExecPlanModels;
using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.Models.Helpers;

namespace DbAnalyzer.Core.Infrastructure.Reports.DublicateIndexes
{
    public class DublicateIndexesReport : IReportGenerator<Report, DublicateIndexesQueryDto>
    {
        private readonly IIndexExplorer _indexExplorer;
        private readonly ILogger<DublicateIndexesReport> _logger;
        public List<ReportItem> ReportItems { get; private set; }

        public DublicateIndexesReport(IIndexExplorer indexExplorer,
            ILogger<DublicateIndexesReport> logger)
        {
            _indexExplorer = indexExplorer;
            _logger = logger;
        }

        public async Task<Report> GetReportAsync(DublicateIndexesQueryDto dto)
        {
            var report = new Report()
            {
                Title = "Formation of a list of duplicate database indexes",
                Description = "Contains a list of recommendations for removing duplicate indexes"
            };
            try
            {
                var reportItems = new List<IReportItem>();
                var items = await _indexExplorer.GetDublicateIndexesAsync();
                if (items != null && items.Any())
                {
                    DbIndex temp = null;
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
                                reportItems.Add(new ReportItem()
                                {
                                    ReportItemStatus = ReportItemStatus.Warning,
                                    Query = dbScriptGenerator.GetDropIndexScript(x),
                                    Annotation = dto.ShowAnnotation ? $"A copy of {temp.IndexName} index" : string.Empty
                                });
                            }
                            temp = x;
                        });
                }
                report.ReportItems = reportItems;
                report.Result = new List<string>()
                {
                    $"Warnings: {reportItems.Where(x=>x.ReportItemStatus == ReportItemStatus.Warning).Count()}"
                };
                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetReportAsync (DublicateIndexes), Error= {ex.Message}");
                return report;
            }
        }
    }
}
