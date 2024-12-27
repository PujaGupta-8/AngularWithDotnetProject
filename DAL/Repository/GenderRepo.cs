using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;

namespace Neosoft_puja_backend.DAL.Repository
{
    public class GenderRepo : Repository<Gender, NeosoftDBContext>, IGenderRepo
    {
        public GenderRepo(NeosoftDBContext context) : base(context)
        {
        }
    }
}
