using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbAnalyzer.Domain.Configurations
{
    [Table("DataSources", Schema = "config")]
    public class DataSource
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "SMALLDATETIME")]
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(250)")]
        public string Value { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string Comment { get; set; }
    }
}
