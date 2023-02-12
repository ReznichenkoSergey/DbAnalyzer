namespace DbAnalyzer.Core.Models.ExecPlanModels
{
    public class DbIndex
    {
        public string TableName { get; set; }
        public string IndexName { get; set; }
        public string KeyColumnList { get; set; }
        public string IncludeColumnList { get; set; }
        public override bool Equals(object obj)
        {
            var temp = (DbIndex)obj;
            return TableName.Equals(temp.TableName)
                && KeyColumnList.Equals(temp.KeyColumnList)
                && IncludeColumnList.Equals(temp.IncludeColumnList);
        }
    }
}
