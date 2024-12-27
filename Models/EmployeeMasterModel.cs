using Microsoft.EntityFrameworkCore;
using Neosoft_puja_backend.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Neosoft_puja_backend.Models
{
    public class EmployeeMasterModel
    {
        public int RowId { get; set; }


        public string? EmployeeCode { get; set; }


        public string? FirstName { get; set; }


        public string? LastName { get; set; }

        public int? CountryId { get; set; }

        public int? StateId { get; set; }

        public int? CityId { get; set; }


        public string? EmailAddress { get; set; }


        public string? MobileNumber { get; set; }


        public string? PanNumber { get; set; }


        public string? PassportNumber { get; set; }


        public string? ProfileImagepath { get; set; }

        public byte? Gender { get; set; }

        public bool IsActive { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoinee { get; set; }


        public DateTime? CreatedDate { get; set; }


        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }


        public DateTime? DeletedDate { get; set; }
          
        public IFormFile? ProfileImage { get; set; }

    }
    public class UpdateEmployeeMasterModel
    {
        public int RowId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string? EmailAddress { get; set; }
        public string? MobileNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string? ProfileImagepath { get; set; }
        public byte? Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfJoinee { get; set; }  
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public IFormFile? ProfileImage { get; set; }

    }
    public class fetchEmployeeModel
    {
        public int RowId { get; set; }


        public string? EmployeeCode { get; set; }


        public string? FirstName { get; set; }


        public string? LastName { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public string? City { get; set; }
        public string? Country { get; set; }

        public string? State { get; set; }

        public int CityId { get; set; }


        public string? EmailAddress { get; set; }


        public string? MobileNumber { get; set; }


        public string? PanNumber { get; set; }


        public string? PassportNumber { get; set; }

            
        public string? ProfileImage { get; set; }

        public byte? Gender { get; set; }

        public bool IsActive { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public DateTime? DateOfJoinee { get; set; }


        public DateTime CreatedDate { get; set; }


        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }


        public DateTime? DeletedDate { get; set; } 

    }
}
