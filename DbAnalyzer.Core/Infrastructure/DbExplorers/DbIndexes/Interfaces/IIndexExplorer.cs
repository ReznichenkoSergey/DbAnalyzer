using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Models;

namespace DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces
{
    public interface IIndexExplorer
    {
        Task<IEnumerable<UnusedIndexStatistic>> GetUnusedIndexesAsync();
        Task<IEnumerable<DbIndex>> GetDublicateIndexesAsync();
    }
}
