namespace DbAnalyzer.Core.Infrastructure.Configurations
{
    public interface IAppConfig
    {
        Task SetCurrentDataSourceConnectionStringAsync(int dataSourceId);

        string GetCurrentDataSourceConnectionString();
    }
}
