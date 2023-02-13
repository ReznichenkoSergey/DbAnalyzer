using DbAnalyzer.Core.Infrastructure.Configurations;
using DbAnalyzer.Core.Infrastructure.Reports.DublicateIndexes;
using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;
using DbAnalyzer.Core.Infrastructure.Reports.Procedures;
using DbAnalyzer.Core.Infrastructure.Reports.UnusedIndexes;
using DbAnalyzer.Core.Models.ReportModels;
using Microsoft.AspNetCore.Mvc;

namespace DbAnalyzer.Controllers
{
    [ApiController]
    public class DbReportController : ControllerBase
    {
        private readonly IAppConfig _appConfig;
        private readonly ILogger<DbExplorerController> _logger;
        private readonly IReportGenerator<Report, ProceduresReportQueryDto> _proceduresRG;
        private readonly IReportGenerator<Report, DublicateIndexesQueryDto> _dublicatesIndexesRG;
        private readonly IReportGenerator<Report, UnusedIndexesQueryDto> _unusedIndexesRG;

        public DbReportController(ILogger<DbExplorerController> logger,
            IReportGenerator<Report, ProceduresReportQueryDto> proceduresRG,
            IReportGenerator<Report, DublicateIndexesQueryDto> dublicatesIndexesRG,
            IReportGenerator<Report, UnusedIndexesQueryDto> unusedIndexesRG,
            IAppConfig appConfig)
        {
            _appConfig = appConfig;
            _logger = logger;
            _proceduresRG = proceduresRG;
            _dublicatesIndexesRG = dublicatesIndexesRG;
            _unusedIndexesRG = unusedIndexesRG;
        }

        [Route("api/1.0/reports/optimization/procedures")]
        [HttpPost]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetNewIndexReportAsync([FromBody] ProceduresReportQueryDto queryDto)
        {
            try
            {
                if(string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString()))
                {
                    return BadRequest("Set current data source");
                }
                if(!queryDto.IsValid())
                {
                    return BadRequest("No arguments set");
                }
                var result = await _proceduresRG.GetReportAsync(queryDto);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetNewIndexReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/reports/optimization/indexes/dublicates")]
        [HttpPost]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetDublicateIndexesReportAsync([FromBody] DublicateIndexesQueryDto queryDto)
        {
            try
            {
                if (string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString()))
                {
                    return BadRequest("Set current data source");
                }
                if (!queryDto.IsValid())
                {
                    return BadRequest("No arguments set");
                }
                var result = await _dublicatesIndexesRG.GetReportAsync(queryDto);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDublicateIndexesReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/reports/optimization/indexes/unused")]
        [HttpPost]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetUnusedIndexesReportAsync([FromBody] UnusedIndexesQueryDto queryDto)
        {
            try
            {
                if (string.IsNullOrEmpty(_appConfig.GetCurrentDataSourceConnectionString()))
                {
                    return BadRequest("Set current data source");
                }
                if (!queryDto.IsValid())
                {
                    return BadRequest("No arguments set");
                }
                var result = await _unusedIndexesRG.GetReportAsync(queryDto);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetUnusedIndexesReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}