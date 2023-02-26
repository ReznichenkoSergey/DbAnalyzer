using DbAnalyzer.Core.Models.ReportModels.Interfaces;

namespace DbAnalyzer.Core.Models.ReportModels
{
    public class Report : IReport
    {
        public IList<string> Result { get; set; }
        public IList<IReportItem> ReportItems { get; set; }
    }
}
