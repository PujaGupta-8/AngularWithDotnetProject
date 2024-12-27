using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Neosoft_puja_backend.DAL.Entities;

[Table("City")]
public partial class City
{
    [Key]
    [Column("Row_Id")]
    public int RowId { get; set; }

    public int StateId { get; set; }

    [StringLength(100)]
    public string CityName { get; set; } = null!;

    [InverseProperty("City")]
    public virtual ICollection<EmployeeMaster> EmployeeMasters { get; set; } = new List<EmployeeMaster>();

    [ForeignKey("StateId")]
    [InverseProperty("Cities")]
    public virtual State State { get; set; } = null!;
}
