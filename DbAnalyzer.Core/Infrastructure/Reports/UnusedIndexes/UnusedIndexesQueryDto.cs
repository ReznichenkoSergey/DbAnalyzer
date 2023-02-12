using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;

namespace DbAnalyzer.Core.Infrastructure.Reports.UnusedIndexes
{
    public class UnusedIndexesQueryDto : IQueryParams
    {
        public bool ShowAnnotation { get; set; } = true;
        public bool GenerateScripts { get; set; } = true;
        public bool IsValid() => GenerateScripts || ShowAnnotation;
    }
}
