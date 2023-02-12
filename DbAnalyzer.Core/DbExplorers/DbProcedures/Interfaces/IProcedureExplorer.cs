using System.Data.SqlClient;

namespace DbAnalyzer.Core.DbExplorers.DbProcedures.Interfaces
{
    public interface IProcedureExplorer
    {
        Task<IEnumerable<string>> GetProcNamesFromDbAsync();
        IList<SqlParameter> GetParamsFromProcedure(string procName);
        Task<IEnumerable<ProcedureExecStatistic>> GetFullExecutionStatisticsAsync();
    }
}
