using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Neosoft_puja_backend.DAL.Entities;

[Table("EmployeeMaster")]
[Index("MobileNumber", Name = "UQ__Employee__250375B17EFE2BB7", IsUnique = true)]
[Index("PassportNumber", Name = "UQ__Employee__45809E71B683ACBF", IsUnique = true)]
[Index("EmailAddress", Name = "UQ__Employee__49A147407B76285C", IsUnique = true)]
[Index("PanNumber", Name = "UQ__Employee__7C38BFC88164CC1B", IsUnique = true)]
public partial class EmployeeMaster
{
    [Key]
    [Column("Row_Id")]
    public int RowId { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string EmployeeCode { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string? LastName { get; set; }

    public int? CountryId { get; set; }

    public int? StateId { get; set; }

    public int? CityId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string EmailAddress { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string MobileNumber { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string PanNumber { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string PassportNumber { get; set; } = null!;

    [StringLength(100)]
    public string? ProfileImage { get; set; }

    public byte? Gender { get; set; }

    public bool IsActive { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime? DateOfJoinee { get; set; }
    public string? ProfileImagepath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DeletedDate { get; set; }

    [ForeignKey("CityId")]
    [InverseProperty("EmployeeMasters")]
    public virtual City? City { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("EmployeeMasters")]
    public virtual Country? Country { get; set; }

    [ForeignKey("StateId")]
    [InverseProperty("EmployeeMasters")]
    public virtual State? State { get; set; }
    


}
