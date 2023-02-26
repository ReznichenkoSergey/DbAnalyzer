using DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries;
using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;

namespace DbAnalyzer.Core.Infrastructure.Reports.Procedures
{
    public class ExpensiveQueriesReportDto : IQueryParams
    {
        public ExpensiveQueryOrderingEnum Ordering { get; set; }

        public int TakeTopAmount { get; set; } = 50;

        public bool IsValid() => TakeTopAmount > 0;
    }
}