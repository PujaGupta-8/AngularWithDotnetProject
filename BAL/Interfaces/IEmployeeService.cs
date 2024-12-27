using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.BAL.Interfaces
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<fetchEmployeeModel>> GetEmployeeMasterList(int pageNumber = 1, int pageSize = 5);
        Task<fetchEmployeeModel> GetEmployeeById(int id);
        Task<bool> AddEmployeeDetails(EmployeeMasterModel entry);
        Task<IEnumerable<fetchEmployeeModel>> GetEmployeeByName(string? Name, string? Email, int pageNumber = 1, int pageSize = 5);
        Task<bool> UpdateEmployeeDetails(UpdateEmployeeMasterModel entry);
        Task<bool> DeleteEmployeeDetails(int id);
        Task<IEnumerable<GenderModel>> GetGenderMaster();
        Task<int> GetActiveEmployeeCount();
    }
}
