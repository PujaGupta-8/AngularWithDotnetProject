using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neosoft_puja_backend.BAL.Interfaces;
using Neosoft_puja_backend.DAL;
using Neosoft_puja_backend.DAL.Entities;
using Neosoft_puja_backend.DAL.Interface;
using Neosoft_puja_backend.DAL.Repository;
using Neosoft_puja_backend.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Neosoft_puja_backend.BAL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IStateRepo _stateRepo;
        private readonly IGenderRepo _genderRepo;
        private readonly NeosoftDBContext _dbContext;
        private readonly IMapper _mapper;
        public EmployeeService(IMapper mapper, IEmployeeRepo employeeRepo, NeosoftDBContext dbContext,IGenderRepo genderRepo,IStateRepo stateRepo)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            _dbContext = dbContext;
            _genderRepo = genderRepo;
            _stateRepo = stateRepo;
        }


        //public async Task<IEnumerable<fetchEmployeeModel>> GetEmployeeMasterList(int pageNumber = 1, int pageSize = 5)
        //{
        //    // Fetch all non-deleted and active employees
        //    List<EmployeeMaster> employees = (List<EmployeeMaster>)await _employeeRepo.GetAllByConditionAsync(a => a.IsDeleted == false && a.IsActive == true);
        //    employees = employees.OrderByDescending(e => e.RowId).ToList();

        //    // Pagination logic
        //    var totalEmployees = employees.Count();
        //    var totalPages = (int)Math.Ceiling(totalEmployees / (double)pageSize);

        //    // Ensure the page number is within the valid range
        //    pageNumber = pageNumber < 1 ? 1 : (pageNumber > totalPages ? totalPages : pageNumber);

        //    // Get the employees for the current page
        //    var pagedEmployees = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        //    // Define the folder path for profile images
        //    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        //    foreach (var employee in pagedEmployees)
        //    {
        //        var state = await _stateRepo.GetSingleAysnc(a => a.RowId == employee.StateId);

        //        if (!string.IsNullOrEmpty(employee.ProfileImagepath))
        //        {
        //            // Combine folder path and file name to get the full file path
        //            string filePath = Path.Combine(folderPath, employee.ProfileImagepath);

        //            // Check if the file exists
        //            if (File.Exists(filePath))
        //            {
        //                try
        //                {
        //                    // Read file bytes and convert to Base64 string
        //                    byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
        //                    string base64String = Convert.ToBase64String(fileBytes);

        //                    // Get the MIME type based on file extension
        //                    var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
        //                    var contentType = GetContentType(fileExtension);

        //                    // Create the Base64 data URL
        //                    employee.ProfileImage = $"data:{contentType};base64,{base64String}";
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Handle file reading exceptions
        //                    employee.ProfileImage = ""; // Or use a default placeholder Base64 string
        //                }
        //            }
        //            else
        //            {
        //                // File does not exist
        //                employee.ProfileImage = ""; // Or use a default placeholder Base64 string
        //            }
        //        }
        //        else
        //        {
        //            // No profile image path specified
        //            employee.ProfileImage = ""; // Or use a default placeholder Base64 string
        //        }
        //    }

        //    // Map the paged EmployeeMaster entities to fetchEmployeeModel and return
        //    return _mapper.Map<List<fetchEmployeeModel>>(pagedEmployees);
        //}
        public async Task<IEnumerable<fetchEmployeeModel>> GetEmployeeMasterList(int pageNumber = 1, int pageSize = 5)
        {
            // Fetch all non-deleted and active employees
            List<EmployeeMaster> employees = (List<EmployeeMaster>)await _employeeRepo.GetAllByConditionAsync(a => a.IsDeleted == false && a.IsActive == true);

            // Order by descending RowId for consistent pagination
            employees = employees.OrderByDescending(e => e.RowId).ToList();

            // Calculate the total number of employees after filtering
            var totalEmployees = employees.Count();

            // Calculate the total number of pages
            var totalPages = (int)Math.Ceiling(totalEmployees / (double)pageSize);

            // Ensure page number is valid (within range)
            pageNumber = pageNumber < 1 ? 1 : (pageNumber > totalPages ? totalPages : pageNumber);

            // Get employees for the current page
            var pagedEmployees = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Log the pagination values for debugging
            Console.WriteLine($"Total Employees: {totalEmployees}");
            Console.WriteLine($"Total Pages: {totalPages}");
            Console.WriteLine($"Page Number: {pageNumber}");
            Console.WriteLine($"Paged Employees Count: {pagedEmployees.Count()}");

            // Process profile images for the employees
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            foreach (var employee in pagedEmployees)
            {
                var state = await _stateRepo.GetSingleAysnc(a => a.RowId == employee.StateId);

                if (!string.IsNullOrEmpty(employee.ProfileImagepath))
                {
                    string filePath = Path.Combine(folderPath, employee.ProfileImagepath);

                    if (File.Exists(filePath))
                    {
                        try
                        {
                            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                            string base64String = Convert.ToBase64String(fileBytes);
                            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
                            var contentType = GetContentType(fileExtension);

                            employee.ProfileImage = $"data:{contentType};base64,{base64String}";
                        }
                        catch (Exception ex)
                        {
                            employee.ProfileImage = ""; // Handle errors
                        }
                    }
                    else
                    {
                        employee.ProfileImage = ""; // No file found
                    }
                }
                else
                {
                    employee.ProfileImage = ""; // No image specified
                }
            }

            // Map the employees to a fetchEmployeeModel and return the paginated result
            return _mapper.Map<List<fetchEmployeeModel>>(pagedEmployees);
        }

        public async Task<int> GetActiveEmployeeCount()
        {
            // Assuming 'Employees' is the DbSet for employee records and 'isActive' is a boolean property
            return await _dbContext.EmployeeMasters
                                 .Where(emp => emp.IsDeleted==false)  // Filter for active employees
                                 .CountAsync();  // Return the count of active employees
        }


        public async Task<fetchEmployeeModel> GetEmployeeById(int Id)
        {
            // Fetch the employee entity using the repository
            var employee = await _employeeRepo.GetSingleAysnc(t => t.RowId == Id);

            if (employee == null)
                return null;

            var employeeModel = _mapper.Map<fetchEmployeeModel>(employee);

            // Add logic to fetch the profile image as a base64 string
            if (!string.IsNullOrEmpty(employee.ProfileImagepath))
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                string filePath = Path.Combine(folderPath, employee.ProfileImagepath);

                if (File.Exists(filePath))
                {
                    byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                    string base64String = Convert.ToBase64String(fileBytes);

                    var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
                    var contentType = GetContentType(fileExtension);

                    employeeModel.ProfileImage = $"data:{contentType};base64,{base64String}";
                }
            }

            return employeeModel;
        }
         

        public async Task<bool> AddEmployeeDetails(EmployeeMasterModel entry)
        {
            // Handle profileImage upload
            if (entry.ProfileImage != null)
            {
                // Generate a unique file name for the image
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(entry.ProfileImage.FileName);
                //string relativePath = Path.Combine("uploads", fileName).Replace("\\", "/");

                string filePath = Path.Combine(folderPath, fileName);
               

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await entry.ProfileImage.CopyToAsync(stream);
                }

                entry.ProfileImagepath = fileName;
                entry.IsActive = true;
            }

            // Map the EmployeeMasterModel to EmployeeMaster entity
            EmployeeMaster data = _mapper.Map<EmployeeMaster>(entry);
            data.ProfileImage = entry.ProfileImagepath;
            // Generate the employee code based on the last employee code in the database
            var lastEmployee = _employeeRepo.GetAll().OrderByDescending(e => e.RowId).FirstOrDefault();
            int lastEmployeeCode = 0;

            if (lastEmployee != null && !string.IsNullOrEmpty(lastEmployee.EmployeeCode))
            {
                // Parse the numeric part of the last employee code
                lastEmployeeCode = int.Parse(lastEmployee.EmployeeCode);
            }

            // Increment the last code to generate the new code
            string newEmployeeCode = (lastEmployeeCode + 1).ToString("D3"); // Format to ensure "001", "002", etc.

            // Check if the new code already exists in the database
            while (_employeeRepo.GetAll().Any(e => e.EmployeeCode == newEmployeeCode))
            {
                lastEmployeeCode++; // Increment and generate a new code
                newEmployeeCode = lastEmployeeCode.ToString("D3");
            }

            data.EmployeeCode = newEmployeeCode;

            // Save the entity to the database
            _employeeRepo.Add(data);
            _employeeRepo.SaveChangesManaged();
            return true;
        }

       

        public async Task<bool> UpdateEmployeeDetails(UpdateEmployeeMasterModel entry)
        {
            try
            {
                if (entry.RowId > 0) // Update
                {
                    // Fetch the existing employee
                    var existingEmployee = await _employeeRepo.GetSingleAysnc(a => a.RowId == entry.RowId);
                    if (existingEmployee == null)
                    {
                        return false; // Employee record not found
                    }

                    // Handle Profile Image update if a new file is provided
                    if (entry.ProfileImage != null)
                    {
                        // Define upload directory
                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                        // Ensure the directory exists
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(existingEmployee.ProfileImagepath))
                        {
                            string oldFilePath = Path.Combine(folderPath, existingEmployee.ProfileImagepath);
                            if (File.Exists(oldFilePath))
                            {
                                try
                                {
                                    File.Delete(oldFilePath);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Failed to delete old image: {oldFilePath}. Error: {ex.Message}");
                                }
                            }
                        }

                        // Save the new image
                        string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(entry.ProfileImage.FileName);
                        string newFilePath = Path.Combine(folderPath, newFileName);

                        try
                        {
                            using (var stream = new FileStream(newFilePath, FileMode.Create))
                            {
                                await entry.ProfileImage.CopyToAsync(stream);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Failed to save new image: {newFilePath}", ex);
                        }

                        // Update the new file path in the entry
                        entry.ProfileImagepath = newFileName;
                    }
                    else
                    {
                        // Preserve the existing image path if no new file is uploaded
                        entry.ProfileImagepath = existingEmployee.ProfileImagepath;
                    }

                    // Map the updated fields
                    _mapper.Map(entry, existingEmployee);

                    // Explicitly set the new or preserved image path
                    existingEmployee.ProfileImagepath = entry.ProfileImagepath;

                    _employeeRepo.Update(existingEmployee);
                }
                else // Add new employee
                {
                    EmployeeMaster newEmployee = _mapper.Map<EmployeeMaster>(entry);

                    if (entry.ProfileImage != null)
                    {
                        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(entry.ProfileImage.FileName);
                        string filePath = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await entry.ProfileImage.CopyToAsync(stream);
                        }

                        newEmployee.ProfileImagepath = fileName;
                    }

                    _employeeRepo.Add(newEmployee);
                }

                // Save changes
                _employeeRepo.SaveChangesManaged();
                return true;
            }
            catch (Exception ex)
            {
                // Log detailed error
                Console.WriteLine($"Error updating employee details: {ex.Message}");
                throw;
            }
        }


        private string GetContentType(string extension)
        {
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }


        public async Task<bool> DeleteEmployeeDetails(int id)
        {
            try
            {
                //// Retrieve the existing employee record
                //var existingEmployee = await _employeeRepo.GetSingleAysnc(a => a.RowId == id);
                //if (existingEmployee == null)
                //{
                //    return false; // Or throw a NotFoundException
                //}

                //// Delete and save changes
                //_employeeRepo.Delete(existingEmployee);
                //_employeeRepo.SaveChangesManaged();
                // Retrieve the existing employee record
                var existingEmployee = await _employeeRepo.GetSingleAysnc(a => a.RowId == id);
                if (existingEmployee == null)
                {
                    return false; // Or throw a NotFoundException
                }

                // Perform a soft delete by updating IsDeleted and DeletedDate
                existingEmployee.IsDeleted = true;
                existingEmployee.DeletedDate = DateTime.Now;

                _employeeRepo.Update(existingEmployee);
                _employeeRepo.SaveChangesManaged();
                return true;
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("Error deleting employee details", ex);
            }
        }

        public async Task<IEnumerable<fetchEmployeeModel>> GetEmployeeByName(string? Name, string? Email,int pageNumber = 1, int pageSize = 5)
        {
            var employees = await _dbContext.EmployeeMasters
            .Where(a =>
                (!string.IsNullOrEmpty(Name) && a.FirstName.Contains(Name)) ||
                (!string.IsNullOrEmpty(Email) && a.EmailAddress.Contains(Email))
            )
            .ToListAsync();
           var totalEmployees = employees.Count();
            var totalPages = (int)Math.Ceiling(totalEmployees / (double)pageSize);

            // Ensure the page number is within the valid range
            pageNumber = pageNumber < 1 ? 1 : (pageNumber > totalPages ? totalPages : pageNumber);

            // Get the employees for the current page
            var pagedEmployees = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return _mapper.Map<List<fetchEmployeeModel>>(employees);
        }

        public async Task<IEnumerable<GenderModel>> GetGenderMaster()
        {
            List<Gender> gender = (List<Gender>) await _genderRepo.GetAllAsync();
            return _mapper.Map<List<GenderModel>>(gender);
        }
    }
}
