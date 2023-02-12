using DbAnalyzer.Core.Models.ReportModels.Interfaces;

namespace DbAnalyzer.Core.Models.ReportModels
{
    public class Report : IReport
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<string> Result { get; set; }
        public IList<IReportItem> ReportItems { get; set; }
    }
}
