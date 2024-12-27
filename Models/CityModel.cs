using System.ComponentModel.DataAnnotations;

namespace Neosoft_puja_backend.Models
{
    public class CityModel
    {
        public int RowId { get; set; }

        public int StateId { get; set; }

        public string CityName { get; set; } = null!;
    }
}
