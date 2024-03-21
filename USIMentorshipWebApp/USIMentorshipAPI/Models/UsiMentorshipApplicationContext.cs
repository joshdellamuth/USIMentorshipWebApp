using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace USIMentorshipAPI.Models;

public partial class UsiMentorshipApplicationContext : DbContext
{
    public UsiMentorshipApplicationContext()
    {
    }

    public UsiMentorshipApplicationContext(DbContextOptions<UsiMentorshipApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ConditionDetail> ConditionDetails { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchCommunicationDetail> MatchCommunicationDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserMatch> UserMatches { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=UsiMentorshipApplication; Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConditionDetail>(entity =>
        {
            entity.HasKey(e => e.ConditionDetailId).HasName("PK__Conditio__84553ACB60D4BBBD");

            entity.Property(e => e.ConditionDetailId).ValueGeneratedNever();
            entity.Property(e => e.ConditionName)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.ConditionValue)
                .HasMaxLength(45)
                .IsUnicode(false);

            entity.HasOne(d => d.Match).WithMany(p => p.ConditionDetails)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__Condition__Match__59063A47");
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PK__Matches__4218C81768811E8D");

            entity.Property(e => e.MatchId).ValueGeneratedNever();
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MatchCommunicationDetail>(entity =>
        {
            entity.HasKey(e => e.CommunicationDetailId).HasName("PK__MatchCom__AC036A097BC25A0B");

            entity.ToTable("MatchCommunicationDetail");

            entity.Property(e => e.CommunicationDetailId).ValueGeneratedNever();
            entity.Property(e => e.CommunicationContent)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationType)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.DateOfCommunication).HasColumnType("datetime");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchCommunicationDetails)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__MatchComm__Match__5BE2A6F2");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.MatchCommunicationDetails)
                .HasForeignKey(d => d.SenderUserId)
                .HasConstraintName("FK__MatchComm__Sende__5CD6CB2B");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AF6DC1C47");

            entity.Property(e => e.RoleId).ValueGeneratedNever();
            entity.Property(e => e.RoleDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CD41691D1");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Bio)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.BusinessCity)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.BusinessCountry)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.BusinessState)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Company)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.CurrentPosition)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.GraduationYear)
                .HasMaxLength(45)
                .IsUnicode(false);
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
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicutre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.UserDetail1).HasName("PK__UserDeta__BA66F12304B4A612");

            entity.Property(e => e.UserDetail1)
                .ValueGeneratedNever()
                .HasColumnName("UserDetail");
            entity.Property(e => e.DetailName)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.DetailValue)
                .HasMaxLength(45)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserDetai__UserI__4BAC3F29");
        });

        modelBuilder.Entity<UserMatch>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserMatc__1788CC4CD56B1BF9");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.Match).WithMany(p => p.UserMatches)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__UserMatch__Match__5629CD9C");

            entity.HasOne(d => d.User).WithOne(p => p.UserMatch)
                .HasForeignKey<UserMatch>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserMatch__UserI__5535A963");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany()
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRoles__RoleI__5070F446");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoles__UserI__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
