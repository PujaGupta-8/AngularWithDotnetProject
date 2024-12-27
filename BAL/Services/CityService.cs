using AutoMapper;
using Neosoft_puja_backend.BAL.Interfaces;
using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;
using Neosoft_puja_backend.DAL.Repository;
using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.BAL.Services
{
    public class CityService : ICityService
    { private readonly ICityRepo _cityRepo;
        private readonly IMapper _mapper;
        public CityService(ICityRepo cityRepo, IMapper mapper) 
        {
            _cityRepo = cityRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CityModel>> GetAllCitiesByStateId(int id)
        {
            List<City> result = (List<City>)await _cityRepo.GetAllByConditionAsync(a => a.StateId == id);
            return _mapper.Map<List<CityModel>>(result);

        }

        public async Task<CityModel> GetCityByCityId(int CityId)
        {
           City result = await _cityRepo.GetSingleAysnc(a => a.RowId == CityId);
            return _mapper.Map<CityModel>(result);
        }
    }
}
