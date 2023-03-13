﻿using System.Data.SqlClient;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Models;

namespace DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces
{
    public interface IProcedureExplorer
    {
        Task<IEnumerable<string>> GetProcNamesFromDbAsync();
        IList<SqlParameter> GetParamsFromProcedure(string procName);
        Task<IEnumerable<ProcedureExecStatistic>> GetFullExecutionStatisticsAsync();
    }
}
