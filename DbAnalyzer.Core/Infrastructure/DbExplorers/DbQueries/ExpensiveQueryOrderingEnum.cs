namespace DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries
{
    public enum ExpensiveQueryOrderingEnum
    {
        FrequentlyRanQuery,
        HighDiskReadingQuery,
        HighCPUQuery,
        LongRunningQuery
    }
}
