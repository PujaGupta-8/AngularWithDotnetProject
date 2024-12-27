using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;

namespace Neosoft_puja_backend.DAL.Repository
{
    public class EmployeeRepo : Repository<EmployeeMaster, NeosoftDBContext>, IEmployeeRepo
    {
        public EmployeeRepo(NeosoftDBContext context) : base(context)
        {
        }
    }
}
