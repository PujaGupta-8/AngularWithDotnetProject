using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.BAL.Interfaces
{
    public interface ICityService
    {
        Task<IEnumerable<CityModel>> GetAllCitiesByStateId(int id);
        Task<CityModel> GetCityByCityId(int CityId);
    }
}
