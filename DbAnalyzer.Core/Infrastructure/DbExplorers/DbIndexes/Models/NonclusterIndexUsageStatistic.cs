namespace DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Models
{
    public class NonclusterIndexUsageStatistic
    {
        public string ProcedureName { get; set; }
        public IList<DbIndexBase> Indexes { get; set; }
    }
}
