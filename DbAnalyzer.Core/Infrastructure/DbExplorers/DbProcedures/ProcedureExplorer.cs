using Dapper;
using DbAnalyzer.Core.Infrastructure.Configurations;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces;
using DbAnalyzer.Domain;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures
{
    public class ProcedureExplorer : IProcedureExplorer
    {
        readonly ILogger<IndexExplorer> _logger;
        private readonly IAppConfig _appConfig;
        readonly private string ProcStatExecutionCounterQuery = @"select 
	                                                                '[' + sc.name + '].[' + obj.name + ']' ProcedureName,
                                                                    proc_stats.last_execution_time LastExecTime,
                                                                    obj.modify_date ModifyDate,
                                                                    obj.create_date CreateDate,
                                                                    proc_stats.execution_count ExecCount
                                                                from sys.dm_exec_procedure_stats proc_stats
                                                                inner join sys.objects obj
                                                                    on obj.object_id = proc_stats.object_id
                                                                inner join sys.schemas sc
                                                                    on obj.schema_id = sc.schema_id
                                                                where obj.type = 'P' and db_name(proc_stats.database_id) = @Database";

        readonly private string GetProcedureNamesQuery = @"SELECT '[' + ROUTINE_SCHEMA + '].[' + ROUTINE_NAME + ']' [ProcedureName]
                                                            FROM INFORMATION_SCHEMA.ROUTINES
                                                            WHERE ROUTINE_TYPE = 'PROCEDURE'";

        public ProcedureExplorer(ILogger<IndexExplorer> logger,
            IAppConfig appConfig)
        {
            _logger = logger;
            _appConfig = appConfig;
        }

        public async Task<IEnumerable<string>> GetProcNamesFromDbAsync()
        {
            try
            {
                using var con = new SqlConnection(_appConfig.GetCurrentDataSourceConnectionString());
                return await con.QueryAsync<string>(GetProcedureNamesQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProcedureExplorer; GetProcedureNamesFromDbAsync, Error= {ex.Message}");
                return new List<string>();
            }
        }

        public IList<SqlParameter> GetParamsFromProcedure(string procName)
        {
            var list = new List<SqlParameter>();
            try
            {
                using var con = new SqlConnection(_appConfig.GetCurrentDataSourceConnectionString());
                using var cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandText = procName,
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                SqlCommandBuilder.DeriveParameters(cmd);
                foreach (SqlParameter p in cmd.Parameters)
                {
                    list.Add(p);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProcedureExplorer; GetParamsFromProcedure, Error= {ex.Message}");
            }
            return list;
        }

        public async Task<IEnumerable<ProcedureExecStatistic>> GetFullExecutionStatisticsAsync()
        {
            try
            {
                using var con = new SqlConnection(_appConfig.GetCurrentDataSourceConnectionString());
                var bld = new SqlConnectionStringBuilder(_appConfig.GetCurrentDataSourceConnectionString());
                return await con.QueryAsync<ProcedureExecStatistic>(ProcStatExecutionCounterQuery, new { Database = bld.InitialCatalog });
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProcedureExplorer; GetFullExecutionStatisticsAsync, Error= {ex.Message}");
                return new List<ProcedureExecStatistic>();
            }
        }
    }
}
