using DbAnalyzer.Core.Infrastructure.Reports.Interfaces;

namespace DbAnalyzer.Core.Infrastructure.Reports.DublicateIndexes
{
    public class DublicateIndexesQueryDto : IQueryParams
    {
        public bool GenerateScripts { get; set; } = true;
        public bool ShowAnnotation { get; set; } = true;
        
        public bool IsValid() => GenerateScripts || ShowAnnotation;
    }
}
