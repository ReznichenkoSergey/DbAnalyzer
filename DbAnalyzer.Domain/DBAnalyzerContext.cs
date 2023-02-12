using DbAnalyzer.Domain.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DbAnalyzer.Domain
{
    public class DBAnalyzerContext: DbContext
    {
        public DbSet<DataSource> DataSources { get; set; }
        public DBAnalyzerContext(DbContextOptions<DBAnalyzerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}