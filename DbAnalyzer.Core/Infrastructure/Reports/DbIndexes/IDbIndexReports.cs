using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Models;
using DbAnalyzer.Core.Models.ReportModels;

namespace DbAnalyzer.Core.Infrastructure.Reports.DbIndexes
{
    public interface IDbIndexReports
    {
        Task<Report> GetUnusedIndexesReportAsync();
        Task<IList<NonclusterIndexUsageStatistic>> GetNonClusterIndexesReportAsync();
        Task<Report> GetDublicateIndexesReportAsync();
    }
}
