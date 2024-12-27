using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;

namespace Neosoft_puja_backend.DAL.Repository
{
    public class StateRepo : Repository<State, NeosoftDBContext>, IStateRepo
    {
        public StateRepo(NeosoftDBContext context) : base(context)
        {
        }
    }
}
