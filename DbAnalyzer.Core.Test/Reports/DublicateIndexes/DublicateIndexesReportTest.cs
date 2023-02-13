using DbAnalyzer.Core.Infrastructure.DbExplorers.DbIndexes.Interfaces;
using DbAnalyzer.Core.Infrastructure.Reports.DublicateIndexes;
using DbAnalyzer.Core.Models.ExecPlanModels;
using DbAnalyzer.Core.Models.ReportModels.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DbAnalyzer.Core.Test.Reports.DublicateIndexes
{
    public class DublicateIndexesReportTest
    {
        private readonly Mock<IIndexExplorer> _indexExplorer;
        private readonly Mock<ILogger<DublicateIndexesReport>> _logger;

        public DublicateIndexesReportTest()
        {
            _indexExplorer = new Mock<IIndexExplorer>();
            _logger = new Mock<ILogger<DublicateIndexesReport>>();
        }

        [Fact]
        public async Task GetReportAsync_Should_Generate_Report_With_Indexes()
        {
            //Arrange
            var expectedStatus = ReportItemStatus.Warning;
            var indexDto = GetIndexListWithDublicates();
            var expectedDuplicateIndex = indexDto.First().IndexName;
            var expectedAnnotation = $"A copy of {expectedDuplicateIndex} index";

            _indexExplorer
                .Setup(x => x.GetDublicateIndexesAsync())
                .ReturnsAsync(indexDto);
            var repo = new DublicateIndexesReport(_indexExplorer.Object, _logger.Object);

            //Act
            var actual = await repo.GetReportAsync(GetDefaultQueryDto());

            //Assert
            actual.Should().NotBeNull();
            actual.ReportItems.Should().HaveCount(1);

            var actualItem = actual.ReportItems[0];
            actualItem.Should().NotBeNull();
            actualItem.ReportItemStatus.Should().Be(expectedStatus);
            actualItem.Annotation.Should().BeEquivalentTo(expectedAnnotation);
        }

        [Fact]
        public async Task GetReportAsync_Should_Generate_Report_Without_Indexes()
        {
            //Arrange
            var indexDto = GetIndexListWithoutDublicates();
            _indexExplorer
                .Setup(x => x.GetDublicateIndexesAsync())
                .ReturnsAsync(indexDto);
            var repo = new DublicateIndexesReport(_indexExplorer.Object, _logger.Object);

            //Act
            var actual = await repo.GetReportAsync(GetDefaultQueryDto());

            //Assert
            actual.Should().NotBeNull();
            actual.ReportItems.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetReportAsync_Should_Return_Null()
        {
            //Arrange
            var dto = new DublicateIndexesQueryDto
            {
                GenerateScripts = false,
                ShowAnnotation = false
            };

            _indexExplorer
                .Setup(x => x.GetDublicateIndexesAsync())
                .Verifiable();
            var repo = new DublicateIndexesReport(_indexExplorer.Object, _logger.Object);

            //Act
            var actual = await repo.GetReportAsync(dto);

            //Assert
            actual.Should().BeNull();
            _indexExplorer.Verify(x => x.GetDublicateIndexesAsync(), Times.Never);
        }

        [Fact]
        public async Task GetReportAsync_Should_Return_Null_Due_Exception()
        {
            //Arrange
            //_logger
            //    .Setup(x => x.LogError(It.IsAny<string>()))
            //    .Verifiable();
            _indexExplorer
                .Setup(x => x.GetDublicateIndexesAsync())
                .Throws(new Exception());
            var repo = new DublicateIndexesReport(_indexExplorer.Object, _logger.Object);

            //Act
            var actual = await repo.GetReportAsync(GetDefaultQueryDto());

            //Assert
            actual.Should().BeNull();
            _logger.Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                  It.IsAny<It.IsAnyType>(),
                  It.IsAny<Exception>(),
                  It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                  Times.Once);
        }

        private DublicateIndexesQueryDto GetDefaultQueryDto()
            => new DublicateIndexesQueryDto
            {
                GenerateScripts = true,
                ShowAnnotation = true
            };

        private IList<DbIndex> GetIndexListWithDublicates()
            => new List<DbIndex>()
            {
                new DbIndex()
                {
                    TableName= "[dbo].[Table]",
                    IndexName ="[dbo].[Index1]",
                    IncludeColumnList = "[Name] ASC",
                    KeyColumnList = "[Value] ASC"
                },
                new DbIndex()
                {
                    TableName= "[dbo].[Table]",
                    IndexName ="[dbo].[Index2]",
                    IncludeColumnList = "[Name] ASC",
                    KeyColumnList = "[Value] ASC"
                }
            };

        private IList<DbIndex> GetIndexListWithoutDublicates()
            => new List<DbIndex>()
            {
                new DbIndex()
                {
                    TableName= "[dbo].[Table1]",
                    IndexName ="[dbo].[Index1]",
                    IncludeColumnList = "[Name] ASC",
                    KeyColumnList = "[Value] ASC"
                },
                new DbIndex()
                {
                    TableName= "[dbo].[Table2]",
                    IndexName ="[dbo].[Index2]",
                    IncludeColumnList = "[Name] ASC",
                    KeyColumnList = "[Value] ASC"
                },
                new DbIndex()
                {
                    TableName= "[dbo].[Table2]",
                    IndexName ="[dbo].[Index3]",
                    IncludeColumnList = "[Name] ASC, [Name2] DESC",
                    KeyColumnList = "[Value] ASC"
                },
                new DbIndex()
                {
                    TableName= "[dbo].[Table2]",
                    IndexName ="[dbo].[Index3]",
                    IncludeColumnList = "[Name] ASC",
                    KeyColumnList = "[Value] ASC, [Name2] DESC"
                }
            };
    }
}