using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Neosoft_puja_backend.Models
{
    public class GenderModel
    {     
        public int GenderId { get; set; }    
        public string Description { get; set; } = null!;
    }
}
