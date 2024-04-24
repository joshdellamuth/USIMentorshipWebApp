using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace USIMentorshipWebApp.Models;

public partial class UsiMentorshipApplicationContext : DbContext
{
    public UsiMentorshipApplicationContext()
    {
    }

    public UsiMentorshipApplicationContext(DbContextOptions<UsiMentorshipApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=UsiMentorshipApplication; Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CBD8EF08F");

            entity.Property(e => e.Bio)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.BusinessCity)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.BusinessCountry)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.BusinessCountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BusinessState)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.BusinessStateCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Company)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.CurrentPosition)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.GraduationYear).HasColumnType("datetime");
            entity.Property(e => e.Industry)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.LinkedInLink)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Major)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Minor)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicutre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.School)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
