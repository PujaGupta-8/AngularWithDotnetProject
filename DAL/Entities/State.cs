using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Neosoft_puja_backend.DAL.Entities;

[Table("State")]
public partial class State
{
    [Key]
    [Column("Row_Id")]
    public int RowId { get; set; }

    public int CountryId { get; set; }

    [StringLength(100)]
    public string StateName { get; set; } = null!;

    [InverseProperty("State")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [ForeignKey("CountryId")]
    [InverseProperty("States")]
    public virtual Country Country { get; set; } = null!;

    [InverseProperty("State")]
    public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; } = new List<EmployeeMaster>();
}
