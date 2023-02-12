using DbAnalyzer.Core.Reports.Interfaces;

namespace DbAnalyzer.Core.Reports.DublicateIndexes
{
    public class DublicateIndexesQueryDto : IQueryParams
    {
        public bool GenerateScripts { get; set; } = true;
        public bool ShowAnnotation { get; set; } = true;
        public bool IsValid() => GenerateScripts || ShowAnnotation;
    }
}
