using DbAnalyzer.Core.Infrastructure.Configurations;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures;
using DbAnalyzer.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbAnalyzer.Controllers
{
    [ApiController]
    public class AppConfigController : ControllerBase
    {
        private readonly ILogger<AppConfigController> _logger;
        private readonly DBAnalyzerContext _db;
        private readonly IAppConfig _appConfig;

        public AppConfigController(ILogger<AppConfigController> logger,
            DBAnalyzerContext db,
            IAppConfig appConfig)
        {
            _logger = logger;
            _db = db;
            _appConfig = appConfig;
        }

        [Route("api/1.0/configuration/datasources")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProcedureExecStatistic>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetDataSourcesAsync()
        {
            try
            {
                var result = await _db.DataSources.ToListAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDataSourcesAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/configuration/datasource")]
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> SetCurrentDataSourceConnectionStringAsync([FromBody] int dataSourceId)
        {
            try
            {
                await _appConfig.SetCurrentDataSourceConnectionStringAsync(dataSourceId);
                var result = !string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString());
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SetCurrentDataSourceConnectionStringAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/configuration/datasource")]
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetCurrentDataSourceConnectionString()
        {
            try
            {
                var result = _appConfig.GetCurrentDataSourceConnectionString();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCurrentDataSourceConnectionString, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

    }
}