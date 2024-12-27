using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;

namespace Neosoft_puja_backend.DAL.Repository
{
    public class CityRepo : Repository<City, NeosoftDBContext>, ICityRepo
    {
        public CityRepo(NeosoftDBContext context) : base(context)
        {
        }
    }
}
