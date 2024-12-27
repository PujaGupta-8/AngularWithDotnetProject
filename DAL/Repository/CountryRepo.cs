using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;

namespace Neosoft_puja_backend.DAL.Repository
{
    public class CountryRepo : Repository<Country, NeosoftDBContext>, ICountryRepo
    {
        public CountryRepo(NeosoftDBContext context) : base(context)
        {
        }
    }
}
