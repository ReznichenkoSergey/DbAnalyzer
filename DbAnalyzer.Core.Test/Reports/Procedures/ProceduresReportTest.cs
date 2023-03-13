using DbAnalyzer.Core.Infrastructure.DbExplorers.DbProcedures.Interfaces;
using DbAnalyzer.Core.Infrastructure.Reports.DbProcedures;
using DbAnalyzer.Core.Models.ReportModels.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DbAnalyzer.Core.Test.Reports.Procedures
{
    public class ProceduresReportTest
    {
        private readonly Mock<IProcedureExplorer> _procedureExplorer;
        private readonly Mock<IExecPlanExplorer> _execPlanExplorer;
        private readonly Mock<ILogger<ProceduresUsageReport>> _logger;
        private readonly string _execPlanContentWithoutIndex = "<ShowPlanXML xmlns=\"http://schemas.microsoft.com/sqlserver/2004/07/showplan\" Version=\"1.539\" Build=\"15.0.2095.3\"><BatchSequence><Batch><Statements><StmtSimple StatementText=\"DECLARE @RETURN_VALUE Int&#xd;&#xa;DECLARE @Id UniqueIdentifier&#xd;&#xa;&#xd;&#xa;EXECUTE @RETURN_VALUE = [dbo].[Table5] @Id\" StatementId=\"1\" StatementCompId=\"1\" StatementType=\"EXECUTE PROC\" RetrievedFromCache=\"false\"><StoredProc ProcName=\"dbo.ProcName\"><Statements><StmtSimple StatementText=\"create procedure [dbo].[ProcName1]&#xd;&#xa;@Id uniqueidentifier&#xd;&#xa;as&#xd;&#xa;delete from [dbo].[Table]&#xd;&#xa;where ([Id] = @Id)\" StatementId=\"2\" StatementCompId=\"3\" StatementType=\"DELETE\" RetrievedFromCache=\"false\" StatementSubTreeCost=\"0.0168726\" StatementEstRows=\"1\" SecurityPolicyApplied=\"false\" StatementOptmLevel=\"TRIVIAL\" QueryHash=\"0x84E2793029BFA92F\" QueryPlanHash=\"0xF038D272FC7B3214\" CardinalityEstimationModelVersion=\"70\"><StatementSetOptions QUOTED_IDENTIFIER=\"true\" ARITHABORT=\"false\" CONCAT_NULL_YIELDS_NULL=\"true\" ANSI_NULLS=\"true\" ANSI_PADDING=\"true\" ANSI_WARNINGS=\"true\" NUMERIC_ROUNDABORT=\"false\"></StatementSetOptions><QueryPlan CachedPlanSize=\"24\" CompileTime=\"3\" CompileCPU=\"3\" CompileMemory=\"248\"><MemoryGrantInfo SerialRequiredMemory=\"0\" SerialDesiredMemory=\"0\" GrantedMemory=\"0\" MaxUsedMemory=\"0\"></MemoryGrantInfo><OptimizerHardwareDependentProperties EstimatedAvailableMemoryGrant=\"138464\" EstimatedPagesCached=\"103848\" EstimatedAvailableDegreeOfParallelism=\"6\" MaxCompileMemory=\"4061544\"></OptimizerHardwareDependentProperties><RelOp NodeId=\"1\" PhysicalOp=\"Assert\" LogicalOp=\"Assert\" EstimateRows=\"1\" EstimateIO=\"0\" EstimateCPU=\"1.8e-07\" AvgRowSize=\"9\" EstimatedTotalSubtreeCost=\"0.0168726\" Parallel=\"0\" EstimateRebinds=\"0\" EstimateRewinds=\"0\" EstimatedExecutionMode=\"Row\"><OutputList></OutputList><Assert StartupExpression=\"0\"><RelOp NodeId=\"2\" PhysicalOp=\"Nested Loops\" LogicalOp=\"Left Semi Join\" EstimateRows=\"1\" EstimateIO=\"0\" EstimateCPU=\"4.18e-06\" AvgRowSize=\"9\" EstimatedTotalSubtreeCost=\"0.0168725\" Parallel=\"0\" EstimateRebinds=\"0\" EstimateRewinds=\"0\" EstimatedExecutionMode=\"Row\"><OutputList><ColumnReference Column=\"Column3\"></ColumnReference></OutputList><NestedLoops Optimized=\"0\"><DefinedValues><DefinedValue><ColumnReference Column=\"Column3\"></ColumnReference></DefinedValue></DefinedValues><OuterReferences><ColumnReference Database=\"[DBAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table1]\" Column=\"Id\"></ColumnReference></OuterReferences><Column1><ColumnReference Column=\"Column3\"></ColumnReference></Column1><RelOp NodeId=\"3\" PhysicalOp=\"Clustered Index Delete\" LogicalOp=\"Delete\" EstimateRows=\"1\" EstimateIO=\"0.01\" EstimateCPU=\"1e-06\" AvgRowSize=\"23\" EstimatedTotalSubtreeCost=\"0.0132841\" Parallel=\"0\" EstimateRebinds=\"0\" EstimateRewinds=\"0\" EstimatedExecutionMode=\"Row\"><OutputList><ColumnReference Database=\"[DBAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table1]\" Column=\"Id\"></ColumnReference></OutputList><SimpleUpdate DMLRequestSort=\"0\"><Object Database=\"[DBAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table1]\" Index=\"[PK_PledgeObject]\" IndexKind=\"Clustered\" Storage=\"RowStore\"></Object><SeekPredicateNew><SeekKeys><Prefix ScanType=\"EQ\"><RangeColumns><ColumnReference Database=\"[DBAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table1]\" Column=\"Id\"></ColumnReference></RangeColumns><RangeExpressions><ScalarOperator ScalarString=\"[@Id]\"><Identifier><ColumnReference Column=\"@Id\"></ColumnReference></Identifier></ScalarOperator></RangeExpressions></Prefix></SeekKeys></SeekPredicateNew></SimpleUpdate></RelOp><RelOp NodeId=\"4\" PhysicalOp=\"Clustered Index Scan\" LogicalOp=\"Clustered Index Scan\" EstimateRows=\"1\" EstimateRowsWithoutRowGoal=\"22390\" EstimatedRowsRead=\"22390\" EstimateIO=\"0.574236\" EstimateCPU=\"0.024786\" AvgRowSize=\"23\" EstimatedTotalSubtreeCost=\"0.00358198\" TableCardinality=\"22390\" Parallel=\"0\" EstimateRebinds=\"0\" EstimateRewinds=\"0\" EstimatedExecutionMode=\"Row\"><OutputList></OutputList><IndexScan Ordered=\"0\" ForcedIndex=\"1\" ForceScan=\"0\" NoExpandHint=\"0\" Storage=\"RowStore\"><DefinedValues></DefinedValues><Object Database=\"[DBAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table4]\" Index=\"[PK_Table4]\" IndexKind=\"Clustered\" Storage=\"RowStore\"></Object><Predicate><ScalarOperator ScalarString=\"[DBAnalyzer].[dbo].[Table4].[Table1Id]=[DBAnalyzer].[dbo].[Table1].[Id]\"><Compare CompareOp=\"EQ\"><ScalarOperator><Identifier><ColumnReference Database=\"[DBAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table4]\" Column=\"Table1Id\"></ColumnReference></Identifier></ScalarOperator><ScalarOperator><Identifier><ColumnReference Database=\"[DBAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table1]\" Column=\"Id\"></ColumnReference></Identifier></ScalarOperator></Compare></ScalarOperator></Predicate></IndexScan></RelOp></NestedLoops></RelOp><Predicate><ScalarOperator ScalarString=\"CASE WHEN NOT [Column3] IS NULL THEN (0) ELSE NULL END\"><IF><Condition><ScalarOperator><Logical Operation=\"NOT\"><ScalarOperator><Logical Operation=\"IS NULL\"><ScalarOperator><Identifier><ColumnReference Column=\"Column3\"></ColumnReference></Identifier></ScalarOperator></Logical></ScalarOperator></Logical></ScalarOperator></Condition><Then><ScalarOperator><Const ConstValue=\"(0)\"></Const></ScalarOperator></Then><Else><ScalarOperator><Const ConstValue=\"NULL\"></Const></ScalarOperator></Else></IF></ScalarOperator></Predicate></Assert></RelOp></QueryPlan></StmtSimple></Statements></StoredProc></StmtSimple></Statements></Batch></BatchSequence></ShowPlanXML>";
        private readonly string _execPlanContentWithIndex = "<MissingIndexes><MissingIndexGroup Impact=\"44.6622\"><MissingIndex Database=\"[DbAnalyzer]\" Schema=\"[dbo]\" Table=\"[Table]\"><ColumnGroup Usage=\"INEQUALITY\"><Column Name=\"[Date]\" ColumnId=\"2\"></Column></ColumnGroup><ColumnGroup Usage=\"INCLUDE\"><Column Name=\"[User]\" ColumnId=\"3\"></Column><Column Name=\"[Column]\" ColumnId=\"4\"></Column><Column Name=\"[Column2]\" ColumnId=\"5\"></Column><Column Name=\"[Column3]\" ColumnId=\"6\"></Column><Column Name=\"[Column4]\" ColumnId=\"7\"></Column><Column Name=\"[Column5]\" ColumnId=\"8\"></Column><Column Name=\"[Column6]\" ColumnId=\"9\"></Column><Column Name=\"[Column7]\" ColumnId=\"10\"></Column><Column Name=\"[Column8]\" ColumnId=\"11\"></Column><Column Name=\"[Column9]\" ColumnId=\"12\"></Column></ColumnGroup></MissingIndex></MissingIndexGroup></MissingIndexes>";

        public ProceduresReportTest()
        {
            _procedureExplorer = new Mock<IProcedureExplorer>();
            _execPlanExplorer = new Mock<IExecPlanExplorer>();
            _logger = new Mock<ILogger<ProceduresUsageReport>>();
        }

        [Fact]
        public async Task GetReportAsync_Should_Generate_Correct_Report()
        {
            //Arrange
            var expectedStatus = ReportItemStatus.Success;
            _procedureExplorer
                .Setup(x => x.GetProcNamesFromDbAsync())
                .ReturnsAsync(new[] { "procName" });
            _procedureExplorer
                .Setup(x => x.GetFullExecutionStatisticsAsync())
                .Verifiable();
            _execPlanExplorer
                .Setup(x => x.GetExecPlanAsync(It.IsAny<string>()))
                .ReturnsAsync(new ExecPlanResultDto(ExecPlanResultType.Success, _execPlanContentWithIndex));

            var repos = new ProceduresUsageReport(_procedureExplorer.Object, _execPlanExplorer.Object, _logger.Object);

            //Act
            var actual = await repos.GetReportAsync();

            //Assert
            actual.Should().NotBeNull();
            actual.ReportItems.Should().HaveCount(1);
            actual.ReportItems[0].ReportItemStatus.Should().Be(expectedStatus);
        }

        [Fact]
        public async Task GetReportAsync_Should_Generate_Correct_Report_With_No_Indexes_Need_Status()
        {
            //Arrange
            var expectedStatus = ReportItemStatus.None;
            _procedureExplorer
                .Setup(x => x.GetProcNamesFromDbAsync())
                .ReturnsAsync(new[] { "procName" });
            _procedureExplorer
                .Setup(x => x.GetFullExecutionStatisticsAsync())
                .Verifiable();
            _execPlanExplorer
                .Setup(x => x.GetExecPlanAsync(It.IsAny<string>()))
                .ReturnsAsync(new ExecPlanResultDto(ExecPlanResultType.Success, _execPlanContentWithoutIndex));

            var repos = new ProceduresUsageReport(_procedureExplorer.Object, _execPlanExplorer.Object, _logger.Object);

            //Act
            var actual = await repos.GetReportAsync();

            //Assert
            actual.Should().NotBeNull();
            actual.ReportItems.Should().HaveCount(1);
            actual.ReportItems[0].ReportItemStatus.Should().Be(expectedStatus);
        }

        [Fact]
        public async Task GetReportAsync_Should_Generate_Correct_Report_With_ExecPlan_Error()
        {
            //Arrange
            var expectedStatus = ReportItemStatus.Error;
            _procedureExplorer
                .Setup(x => x.GetProcNamesFromDbAsync())
                .ReturnsAsync(new[] { "procName" });
            _procedureExplorer
                .Setup(x => x.GetFullExecutionStatisticsAsync())
                .Verifiable();
            _execPlanExplorer
                .Setup(x => x.GetExecPlanAsync(It.IsAny<string>()))
                .ReturnsAsync(new ExecPlanResultDto(ExecPlanResultType.Success, string.Empty));

            var repos = new ProceduresUsageReport(_procedureExplorer.Object, _execPlanExplorer.Object, _logger.Object);

            //Act
            var actual = await repos.GetReportAsync();

            //Assert
            actual.Should().NotBeNull();
            actual.ReportItems.Should().HaveCount(1);
            actual.ReportItems[0].ReportItemStatus.Should().Be(expectedStatus);
        }
    }
}
