using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neosoft_puja_backend.BAL.Interfaces;
using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.Models;

namespace Neosoft_puja_backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    { private readonly IEmployeeService _employeeService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        public EmployeeController( IEmployeeService employeeService, ICountryService countryService,IStateService stateService,ICityService cityService)
        {
           _employeeService = employeeService;
            _countryService = countryService;
            _stateService = stateService;
            _cityService = cityService;
        }
      /// <summary>
      /// To Fetch all employee list
      /// </summary>
      /// <returns></returns>
        [HttpGet("EmployeeList")]
public async Task<ActionResult<IEnumerable<fetchEmployeeModel>>> EmployeeList(int pageNumber = 1, int pageSize = 5)
{
    IEnumerable<fetchEmployeeModel> transactions = null;
    int totalCount = 0;  // To store the total count of active employees

    try
    {
        // Get total count of active employees (for pagination)
        totalCount = await _employeeService.GetActiveEmployeeCount();

        // Get the employees list based on pagination
        transactions = await _employeeService.GetEmployeeMasterList(pageNumber, pageSize);

        if (transactions != null)
        {
            foreach (var transaction in transactions)
            {
                var country = await _countryService.GetCountryByCountryId(transaction.CountryId);
                transaction.Country = country.CountryName;
                
                var state = await _stateService.GetStateStateId(transaction.StateId);
                transaction.State = state.StateName;
                
                var city = await _cityService.GetCityByCityId(transaction.CityId);
                transaction.City = city.CityName;
            }
        }
    }
    catch
    {
        throw;
    }

    // Return a response with both employees and the total count for pagination
    return Ok(new { Employees = transactions, TotalCount = totalCount });
}

        /// <summary>
        /// Get Gender List
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenderList")]
        public async Task<ActionResult<IEnumerable<GenderModel>>> GenderList()
        {

            IEnumerable<GenderModel> transactions = null;
            try
            {
                transactions = await _employeeService.GetGenderMaster();
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }
        /// <summary>
        /// Get Employee By Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeById")]
        public async Task<ActionResult<fetchEmployeeModel>> GetEmployeeById(int Id)
        {
            fetchEmployeeModel transactions = null;


            try
            {
                transactions = await _employeeService.GetEmployeeById(Id);
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }

        /// <summary>
        /// Add Employee Details
        /// </summary>
        /// <param name="employeeMaster"></param>
        /// <returns></returns>
        [HttpPost("AddEmployeeDetails")]
        public async Task<ActionResult<EmployeeMasterModel>> AddEmployeeDetails(EmployeeMasterModel employeeMaster)
        {
            var employee= await _employeeService.AddEmployeeDetails(employeeMaster);
            
            return Ok(employee);
        }
        /// <summary>
        /// Update Emplyee by employee id
        /// </summary>
        /// <param name="updatedEntry"></param>
        /// <returns></returns>
        [HttpPut("UpdateEmployeeDetails")]
        public async Task<ActionResult<UpdateEmployeeMasterModel>> UpdateEmployeeDetails([FromForm] UpdateEmployeeMasterModel updatedEntry)
        {
            // Validate date fields before updating
            if (updatedEntry.DateOfBirth < new DateTime(1753, 1, 1) || updatedEntry.DateOfBirth > new DateTime(9999, 12, 31))
            {
                return BadRequest("Invalid Date of Birth. It must be between 1/1/1753 and 12/31/9999.");
            }

            if (updatedEntry.DateOfJoinee < new DateTime(1753, 1, 1) || updatedEntry.DateOfJoinee > new DateTime(9999, 12, 31))
            {
                return BadRequest("Invalid Date of Joining. It must be between 1/1/1753 and 12/31/9999.");
            }
            updatedEntry.UpdatedDate= DateTime.Now;
            var employee = await _employeeService.UpdateEmployeeDetails(updatedEntry);

            return Ok(employee);
        }
        /// <summary>
        ///Soft Delete Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteEmployeeDetails")]
        public async Task<ActionResult<EmployeeMasterModel>> DeleteEmployeeDetails(int id)
        {
            var employee = await _employeeService.DeleteEmployeeDetails(id);

            return Ok(employee);
        }
        /// <summary>
        /// Fetch EmployeeBy name and email
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet("EmployeeListByNameEmail")]
        public async Task<ActionResult<IEnumerable<fetchEmployeeModel>>> EmployeeListByNameEmail(string? Name, string? Email)
        {

            IEnumerable<fetchEmployeeModel> transactions = null;
            try
            {
                transactions = await _employeeService.GetEmployeeByName(Name,Email);
                if (transactions != null)
                {
                    foreach (var transaction in transactions)
                    {
                        var c = await _countryService.GetCountryByCountryId(transaction.CountryId);
                        transaction.Country = c.CountryName;
                        var s = await _stateService.GetStateStateId(transaction.StateId);
                        transaction.State = s.StateName;

                        var ci = await _cityService.GetCityByCityId(transaction.CityId);
                        transaction.City = ci.CityName;
                    }
                }
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }

        [HttpGet("CountryList")]
        public async Task<ActionResult<IEnumerable<CountryModel>>> CountryList()
        {

            IEnumerable<CountryModel> transactions = null;
            try
            {
                transactions = await _countryService.GetCountrylist();
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }
        /// <summary>
        /// Get State List by countryid
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("StateListByCountry")]
        public async Task<ActionResult<IEnumerable<StateModel>>> StateListByCountry(int Id)
        {

            IEnumerable<StateModel> transactions = null;
            try
            {
                transactions = await _stateService.GetAllStateByCountryId(Id);
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }
        /// <summary>
        /// Get City List By state id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("CityListByState")]
        public async Task<ActionResult<IEnumerable<StateModel>>> CityListByState(int Id)
        {

            IEnumerable<CityModel> transactions = null;
            try
            {
                transactions = await _cityService.GetAllCitiesByStateId(Id);
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }
        /// <summary>
        /// Get State by stateid
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet("GetStateById")]
        public async Task<ActionResult<StateModel>> GetStateById(int stateId)
        {
            StateModel transactions = null;


            try
            {
                transactions = await _stateService.GetStateStateId(stateId);
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }
        /// <summary>
        /// Get Country By country id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpGet("GetCountryById")]
        public async Task<ActionResult<CountryModel>> GetCountryById(int countryId)
        {
            CountryModel transactions = null;


            try
            {
                transactions = await _countryService.GetCountryByCountryId(countryId);
            }
            catch
            {
                throw;
            }
            return Ok(transactions);
        }

    }
}
