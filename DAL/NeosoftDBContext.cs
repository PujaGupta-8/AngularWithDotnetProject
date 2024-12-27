using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Neosoft_puja_backend.DAL.Entities;

namespace Neosoft_puja_backend.DAL;

public partial class NeosoftDBContext : DbContext
{
    public NeosoftDBContext()
    {
    }

    public NeosoftDBContext(DbContextOptions<NeosoftDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; }

    public virtual DbSet<State> States { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-49M2G22\\SQLEXPRESS;Database=Neosoft_Puja_Gupta;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.RowId).HasName("PK__City__7C36D07EB40FCBBE");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__City__StateId__4E88ABD4");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.RowId).HasName("PK__Country__7C36D07EF6BEC284");
        });
        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Country__7C36D07EF6BEC284");
        });

        modelBuilder.Entity<EmployeeMaster>(entity =>
        {
            entity.HasKey(e => e.RowId).HasName("PK__Employee__7C36D07EC79C79B5");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.City).WithMany(p => p.EmployeeMasters).HasConstraintName("FK__EmployeeM__CityI__571DF1D5");

            entity.HasOne(d => d.Country).WithMany(p => p.EmployeeMasters).HasConstraintName("FK__EmployeeM__Count__5535A963");

            entity.HasOne(d => d.State).WithMany(p => p.EmployeeMasters).HasConstraintName("FK__EmployeeM__State__5629CD9C");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.RowId).HasName("PK__State__7C36D07EF9722D28");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__State__CountryId__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
