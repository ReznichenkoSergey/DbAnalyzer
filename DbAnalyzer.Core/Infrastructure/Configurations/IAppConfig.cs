namespace DbAnalyzer.Core.Infrastructure.Configurations
{
    public interface IAppConfig
    {
        Task<bool> SetCurrentDataSourceConnectionStringAsync(int dataSourceId);

        string GetCurrentDataSourceConnectionString();
    }
}
