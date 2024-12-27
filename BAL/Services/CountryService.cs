using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Neosoft_puja_backend.BAL.Interfaces;
using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;
using Neosoft_puja_backend.DAL.Repository;
using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.BAL.Services
{
    public class CountryService:ICountryService
    {
        private readonly ICountryRepo _countryRepo;
        private readonly IMapper _mapper;
        public CountryService(ICountryRepo countryRepo,IMapper mapper) 
        {
            _countryRepo = countryRepo;
            _mapper = mapper;
        }

        public async Task<CountryModel> GetCountryByCountryId(int countryId)
        {
        
            Country country = await _countryRepo.GetSingleAysnc(a => a.RowId == countryId);
            return _mapper.Map<CountryModel>(country);

        }

        public  async Task<IEnumerable<CountryModel>> GetCountrylist()
        {
            List<Country> countries= (List<Country>)await _countryRepo.GetAllAsync();
            return _mapper.Map<List<CountryModel>>(countries);
        }
    }
}
