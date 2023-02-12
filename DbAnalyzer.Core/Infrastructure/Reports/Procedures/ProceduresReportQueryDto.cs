using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;

namespace DbAnalyzer.Core.Infrastructure.Reports.Procedures
{
    public class ProceduresReportQueryDto : IQueryParams
    {
        public bool ShowWithRecommendations { get; set; } = true;
        public bool ShowWithoutOptimizations { get; set; }
        public bool ShowWithErrors { get; set; } = true;
        public bool IsValid() => ShowWithRecommendations || ShowWithoutOptimizations || ShowWithErrors;
    }
}