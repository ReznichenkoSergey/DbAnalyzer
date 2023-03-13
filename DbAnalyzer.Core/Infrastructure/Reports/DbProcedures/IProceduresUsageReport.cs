using DbAnalyzer.Core.Models.ReportModels;

namespace DbAnalyzer.Core.Infrastructure.Reports.DbProcedures
{
    public interface IProceduresUsageReport
    {
        Task<Report> GetReportAsync();
    }
}
