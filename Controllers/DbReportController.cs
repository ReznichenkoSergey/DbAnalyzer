using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Models;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries.Models;
using DbAnalyzer.Core.Infrastructure.Reports.DbIndexes;
using DbAnalyzer.Core.Infrastructure.Reports.DbProcedures;
using DbAnalyzer.Core.Infrastructure.Reports.Queries;
using DbAnalyzer.Core.Models.ReportModels;
using Microsoft.AspNetCore.Mvc;

namespace DbAnalyzer.Controllers
{
    [ApiController]
    public class DbReportController : ControllerBase
    {
        private readonly ILogger<DbExplorerController> _logger;
        private readonly IProceduresUsageReport _proceduresUsageReport;
        private readonly IExpensiveQueriesReport _expensiveQueriesReport;
        private readonly IDbIndexReports _dbIndexReports;

        public DbReportController(ILogger<DbExplorerController> logger,
            IProceduresUsageReport proceduresUsageReport,
            IExpensiveQueriesReport expensiveQueriesReport,
            IDbIndexReports dbIndexReports)
        {
            _logger = logger;
            _proceduresUsageReport = proceduresUsageReport;
            _expensiveQueriesReport = expensiveQueriesReport;
            _dbIndexReports = dbIndexReports;
        }

        [Route("api/1.0/reports/optimization/procedures")]
        [HttpGet]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetNewIndexReportAsync()
        {
            try
            {
                var result = await _proceduresUsageReport.GetReportAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetNewIndexReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/reports/optimization/indexes/dublicates")]
        [HttpGet]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetDublicateIndexesReportAsync()
        {
            try
            {
                var result = await _dbIndexReports.GetDublicateIndexesReportAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetDublicateIndexesReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/reports/optimization/indexes/unused")]
        [HttpGet]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetUnusedIndexesReportAsync()
        {
            try
            {
                var result = await _dbIndexReports.GetUnusedIndexesReportAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetUnusedIndexesReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/reports/optimization/expensivequeries")]
        [HttpGet]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetExpensiveQueriesReportAsync([FromQuery] ExpensiveQueryOrderingEnum ordering, int topAmount)
        {
            try
            {
                var result = await _expensiveQueriesReport.GetReportAsync(ordering, topAmount);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetExpensiveQueriesReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/reports/statistic/indexes/usage/procedures")]
        [HttpGet]
        [ProducesResponseType(typeof(List<NonclusterIndexUsageStatistic>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetNonclusterIndexesUsageReportAsync()
        {
            try
            {
                var result = await _dbIndexReports.GetNonClusterIndexesReportAsync();
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetNonclusterIndexesUsageReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Route("api/1.0/reports/statistic/indexes/usage")]
        [HttpGet]
        [ProducesResponseType(typeof(List<NonclusterIndexUsageStatistic>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetNonclusterIndexesUsageReport1Async()
        {
            try
            {
                var result = await _dbIndexReports.GetNonClusterIndexesReportAsync();
                var t = result
                    .SelectMany(x => x.Indexes)
                    .GroupBy(x => x.IndexName)
                    .Select(x => new
                    {
                        IndexName = x.Key,
                        Count = x.Count()
                    })
                    .OrderByDescending(x => x.Count);
                return new OkObjectResult(t);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetNonclusterIndexesUsageReportAsync, Error= {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}