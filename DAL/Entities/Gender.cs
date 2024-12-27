using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Neosoft_puja_backend.DAL.Entities
{

    [Table("Gender")]
    public partial class Gender
    {
        [Key]
        [Column("GenderId")]
        public int GenderId{ get; set; }

        [Column("Description")]
        [StringLength(100)]
        public string Description { get; set; } = null!;

       
    }
}
