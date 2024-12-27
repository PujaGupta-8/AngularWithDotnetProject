using Neosoft_puja_backend.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Neosoft_puja_backend.Models
{
    public class CountryModel
    {
    
        public int RowId { get; set; }


        public string CountryName { get; set; } = null!;

    
    }
}

