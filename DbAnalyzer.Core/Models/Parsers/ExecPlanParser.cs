﻿using DbAnalyzer.Core.Models.ExecPlanModels;
using DbAnalyzer.Core.Models.Helpers;
using SqlAnalyzer.Core.Models;
using SqlAnalyzer.Core.Models.ExecPlanModels;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DbAnalyzer.Core.Models.Parsers
{
    public class ExecPlanParser
    {
        private readonly string indexStartValue = "<MissingIndexes>";
        private readonly string indexEndValue = "</MissingIndexes>";

        private readonly string statStartValue = "<ColumnsWithNoStatistics>";
        private readonly string statEndValue = "</ColumnsWithNoStatistics>";

        private readonly CultureInfo _cultureInfo;

        public ExecPlanParser()
        {
            _cultureInfo = new CultureInfo("en-US");
        }

        public IList<string> GetMissingStatistics(string execPlanContent)
        {
            var items = new List<string>();
            var start = execPlanContent.IndexOf(statStartValue);
            if (start > -1)
            {
                var scriptGenerator = new DbScriptGenerator();
                var end = execPlanContent.IndexOf(statEndValue) + statEndValue.Length;
                var xmlSerializer = new XmlSerializer(typeof(ColumnsWithNoStatistics));
                using var xmlReader = new StringReader(execPlanContent.Substring(start, end - start));
                var missingStatistics = (ColumnsWithNoStatistics)xmlSerializer.Deserialize(xmlReader);
                items.AddRange(scriptGenerator.GetCreateStatisticsScript(missingStatistics));
            }
            return items;
        }

        public IList<MissingIndex> GetMissingIndexes(string execPlanContent)
        {
            var items = new List<MissingIndex>();
            var start = execPlanContent.IndexOf(indexStartValue);
            if (start > -1)
            {
                var end = execPlanContent.IndexOf(indexEndValue) + indexEndValue.Length;
                var xmlSerializer = new XmlSerializer(typeof(MissingIndexes));
                using var xmlReader = new StringReader(execPlanContent.Substring(start, end - start));
                var missingIndexes = (MissingIndexes)xmlSerializer.Deserialize(xmlReader);
                items.Add(missingIndexes.GetMissingIndex(_cultureInfo));
            }
            return items;
        }

        public class MissingIndex
        {
            public decimal Impact { get; set; }
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string Query { get; set; }
        }
    }
}
