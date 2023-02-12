using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures;
using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces;
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
            services.AddTransient<IIndexExplorer, IndexExplorer>();
            services.AddTransient<IProcedureExplorer, ProcedureExplorer>();
            services.AddTransient<IExecPlanExplorer, ExecPlanExplorer>();
            services.AddTransient<IReportGenerator<Report, ProceduresReportQueryDto>, ProceduresReport>();
            services.AddTransient<IReportGenerator<Report, DublicateIndexesQueryDto>, DublicateIndexesReport>();
            services.AddTransient<IReportGenerator<Report, UnusedIndexesQueryDto>, UnusedIndexesReport>();
        }
    }
}
