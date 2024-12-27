using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Neosoft_puja_backend.DAL.Entities;

[Table("Country")]
public partial class Country
{
    [Key]
    [Column("Row_Id")]
    public int RowId { get; set; }

    [StringLength(100)]
    public string CountryName { get; set; } = null!;

    [InverseProperty("Country")]
    public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; } = new List<EmployeeMaster>();

    [InverseProperty("Country")]
    public virtual ICollection<State> States { get; set; } = new List<State>();
}
