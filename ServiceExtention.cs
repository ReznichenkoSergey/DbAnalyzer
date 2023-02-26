using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbQueries.Interfaces;
using DbAnalyzer.Core.Infrastructure.Reports.DublicateIndexes;
using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;
using DbAnalyzer.Core.Infrastructure.Reports.Procedures;
using DbAnalyzer.Core.Infrastructure.Reports.UnusedIndexes;
using DbAnalyzer.Core.Models.ReportModels;

namespace DbAnalyzer
{
    public static class ServiceExtention
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services
                .AddTransient<IIndexExplorer, IndexExplorer>()
                .AddTransient<IProcedureExplorer, ProcedureExplorer>()
                .AddTransient<IExecPlanExplorer, ExecPlanExplorer>()
                .AddTransient<IDbQueryExplorer, DbQueryExplorer>()
                .AddTransient<IReportGenerator<Report, ProceduresReportQueryDto>, ProceduresReport>()
                .AddTransient<IReportGenerator<Report, DublicateIndexesQueryDto>, DublicateIndexesReport>()
                .AddTransient<IReportGenerator<Report, UnusedIndexesQueryDto>, UnusedIndexesReport>()
                .AddTransient<IReportGenerator<Report, ExpensiveQueriesReportDto>, ExpensiveQueriesReport>();
        }
    }
}
