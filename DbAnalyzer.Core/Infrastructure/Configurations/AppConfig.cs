using DbAnalyzer.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DbAnalyzer.Core.Infrastructure.Configurations
{
    public class AppConfig: IAppConfig
    {
        private readonly ILogger<AppConfig> _logger;
        private string _connectionString;
        private readonly IServiceScopeFactory _scopeFactory;

        public AppConfig(ILogger<AppConfig> logger,
            IServiceScopeFactory scopeFactory) 
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<bool> SetCurrentDataSourceConnectionStringAsync(int dataSourceId)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<DBAnalyzerContext>();
                    var item = await _context.DataSources.FirstOrDefaultAsync(x => x.Id == dataSourceId);
                    if (item != null)
                    {
                        _connectionString = item.Value;
                    }
                }
                return string.IsNullOrEmpty(_connectionString);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AppConfig; SetCurrentDataSourceConnectionStringAsync, Error= {ex.Message}");
                return false;
            }
        }

        public string GetCurrentDataSourceConnectionString() => _connectionString;
    }
}
