using DbAnalyzer.Core.DbExplorers.DbIndexes;
using DbAnalyzer.Core.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.DbExplorers.DbProcedures;
using DbAnalyzer.Core.DbExplorers.DbProcedures.Interfaces;
using DbAnalyzer.Core.Models.ReportModels;
using DbAnalyzer.Core.Reports.DublicateIndexes;
using DbAnalyzer.Core.Reports.Interfaces;
using DbAnalyzer.Core.Reports.Procedures;
using DbAnalyzer.Core.Reports.UnusedIndexes;

namespace DbAnalyzer
{
    public static class ServiceExtention
    {
        public static void AddService(this IServiceCollection services)
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
