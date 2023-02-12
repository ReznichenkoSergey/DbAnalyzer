using DbAnalyzer.Core.Reports.Interfaces;

namespace DbAnalyzer.Core.Reports.UnusedIndexes
{
    public class UnusedIndexesQueryDto : IQueryParams
    {
        public bool ShowAnnotation { get; set; } = true; 
        public bool GenerateScripts { get; set; } = true;
        public bool IsValid() => GenerateScripts || ShowAnnotation;
    }
}
