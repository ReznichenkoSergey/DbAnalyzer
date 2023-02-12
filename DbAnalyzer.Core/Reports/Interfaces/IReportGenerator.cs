using DbAnalyzer.Core.Models.ReportModels.Interfaces;

namespace DbAnalyzer.Core.Reports.Interfaces
{
    public interface IReportGenerator<T,V>
        where T : IReport
        where V : IQueryParams
    {
        Task<T> GetReportAsync(V queryParams);
    }
}
