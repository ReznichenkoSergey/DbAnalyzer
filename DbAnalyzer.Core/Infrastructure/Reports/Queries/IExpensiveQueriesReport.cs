using DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries.Models;
using DbAnalyzer.Core.Models.ReportModels;

namespace DbAnalyzer.Core.Infrastructure.Reports.Queries
{
    public interface IExpensiveQueriesReport
    {
        Task<Report> GetReportAsync(ExpensiveQueryOrderingEnum ordering, int topAmount);
    }
}
