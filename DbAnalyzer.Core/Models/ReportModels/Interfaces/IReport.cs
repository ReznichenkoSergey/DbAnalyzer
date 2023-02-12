namespace DbAnalyzer.Core.Models.ReportModels.Interfaces
{
    public interface IReport
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<IReportItem> ReportItems { get; set; }
        public IList<string> Result { get; set; }
    }
}
