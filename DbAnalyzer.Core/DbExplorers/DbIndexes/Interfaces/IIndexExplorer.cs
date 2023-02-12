using DbAnalyzer.Core.Models.ExecPlanModels;

namespace DbAnalyzer.Core.DbExplorers.DbIndexes.Interfaces
{
    public interface IIndexExplorer
    {
        Task<IEnumerable<UnusedIndexStatistic>> GetUnusedIndexesAsync();
        Task<IEnumerable<DbIndex>> GetDublicateIndexesAsync();
    }
}
