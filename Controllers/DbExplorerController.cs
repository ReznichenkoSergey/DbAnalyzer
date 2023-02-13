using DbAnalyzer.Core.Infrastructure.Configurations;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces;
using DbAnalyzer.Core.Models.ExecPlanModels;
using Microsoft.AspNetCore.Mvc;

namespace DbAnalyzer.Controllers
{
    [ApiController]
    public class DbExplorerController : ControllerBase
    {
        private readonly IAppConfig _appConfig;
        private readonly ILogger<DbExplorerController> _logger;
        private readonly IProcedureExplorer _procExplorer;
        private readonly IIndexExplorer _indexExplorer;

        public DbExplorerController(ILogger<DbExplorerController> logger,
            IProcedureExplorer procExplorer,
            IIndexExplorer indexExplorer,
            IAppConfig appConfig)
        {
            _appConfig = appConfig;
            _logger = logger;
            _procExplorer = procExplorer;
            _indexExplorer = indexExplorer;
        }

        [Route("api/1.0/procedures/executionstatistic/full")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProcedureExecStatistic>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetProcedureExecStatisticAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString()))
                {
                    return BadRequest("Set current data source");
                }
                var result = await _procExplorer.GetFullExecutionStatisticsAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProcedureExecStatisticAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/procedures/names")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetProcNamesFromDbAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString()))
                {
                    return BadRequest("Set current data source");
                }
                var result = await _procExplorer.GetProcNamesFromDbAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetProcNamesFromDbAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/indexes/dublicates")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DbIndex>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetDublicateIndexesAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString()))
                {
                    return BadRequest("Set current data source");
                }
                var result = await _indexExplorer.GetDublicateIndexesAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDublicateIndexesAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/indexes/executionstatistic")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UnusedIndexStatistic>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetIndexExecutionStatisticAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString()))
                {
                    return BadRequest("Set current data source");
                }
                var result = await _indexExplorer.GetUnusedIndexesAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetIndexExecutionStatisticAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}