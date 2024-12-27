
using AutoMapper;
using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.Helper
{
    public class MapperClass : Profile
    {
        public MapperClass()
        {
            CreateMap<EmployeeMaster, EmployeeMasterModel>()
            .ForMember(dest => dest.ProfileImagepath, opt => opt.Ignore());
            CreateMap<EmployeeMasterModel, EmployeeMaster>();

            CreateMap<EmployeeMaster, fetchEmployeeModel>();
            CreateMap<fetchEmployeeModel, EmployeeMaster>();
            CreateMap<EmployeeMaster, UpdateEmployeeMasterModel>()
            .ForMember(dest => dest.ProfileImagepath, opt => opt.Ignore());
            CreateMap<UpdateEmployeeMasterModel, EmployeeMaster>();

            CreateMap<Country, CountryModel>();
            CreateMap<CountryModel, Country>();
            CreateMap<City, CityModel>();
            CreateMap<CityModel, City>();
            CreateMap<State, StateModel>();
            CreateMap<StateModel, State>();
            CreateMap<Gender, GenderModel>();
            CreateMap<GenderModel, Gender>();
        }
    }
}
