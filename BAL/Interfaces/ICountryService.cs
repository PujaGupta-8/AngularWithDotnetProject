using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.BAL.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryModel>> GetCountrylist();
        Task<CountryModel> GetCountryByCountryId(int countryId);
    }
}
