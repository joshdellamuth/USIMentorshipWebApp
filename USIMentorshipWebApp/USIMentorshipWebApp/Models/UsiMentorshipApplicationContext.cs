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

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ConditionDetail> ConditionDetails { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Industry> Industries { get; set; }

    public virtual DbSet<JobTitle> JobTitles { get; set; }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchCommunicationDetail> MatchCommunicationDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    public virtual DbSet<UserMatch> UserMatches { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=UsiMentorshipApplication; Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21B76B53F2489");

            entity.Property(e => e.CityName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StateCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.StateCodeNavigation).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cities__StateCod__18EBB532");
        });

        modelBuilder.Entity<ConditionDetail>(entity =>
        {
            entity.HasKey(e => e.ConditionDetailId).HasName("PK__Conditio__84553ACBDCDD8C7D");

            entity.Property(e => e.ConditionName)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.ConditionValue)
                .HasMaxLength(45)
                .IsUnicode(false);

            entity.HasOne(d => d.Match).WithMany(p => p.ConditionDetails)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__Condition__Match__5812160E");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode).HasName("PK__Countrie__5D9B0D2D38B5BDC5");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Industry>(entity =>
        {
            entity.HasKey(e => e.IndustryId).HasName("PK__Industri__808DEDCC039E063F");

            entity.Property(e => e.IndustryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JobTitle>(entity =>
        {
            entity.HasKey(e => e.JobTitleId).HasName("PK__JobTitle__35382FE99C5F8BF1");

            entity.Property(e => e.JobTitleName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MatchId).HasName("PK__Matches__4218C81731F4AB9F");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MatchCommunicationDetail>(entity =>
        {
            entity.HasKey(e => e.CommunicationDetailId).HasName("PK__MatchCom__AC036A0943FBA544");

            entity.ToTable("MatchCommunicationDetail");

            entity.Property(e => e.CommunicationContent)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationType)
                .HasMaxLength(45)
                .IsUnicode(false);
            entity.Property(e => e.DateOfCommunication).HasColumnType("datetime");
            entity.Property(e => e.Deleted).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchCommunicationDetails)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__MatchComm__Match__5AEE82B9");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.MatchCommunicationDetails)
                .HasForeignKey(d => d.SenderUserId)
                .HasConstraintName("FK__MatchComm__Sende__5BE2A6F2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1ADE7FBB1E");

            entity.Property(e => e.RoleDescription)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(45)
                .IsUnicode(false);
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.HasKey(e => e.SchoolId).HasName("PK__Schools__3DA4675BF0410B11");

            entity.Property(e => e.SchoolName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.StateCode).HasName("PK__States__D515E98B1FFEFCE5");

            entity.Property(e => e.StateCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StateName)
                .HasMaxLength(22)
                .IsUnicode(false);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryCode)
                .HasConstraintName("FK_States_OtherTable");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CBD8EF08F");

            entity.Property(e => e.Bio)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.BusinessCountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
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

            entity.HasOne(d => d.BusinessCity).WithMany(p => p.Users)
                .HasForeignKey(d => d.BusinessCityId)
                .HasConstraintName("FK_Users_Cities");

            entity.HasOne(d => d.BusinessCountryCodeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.BusinessCountryCode)
                .HasConstraintName("FK_Users_Countries");

            entity.HasOne(d => d.BusinessStateCodeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.BusinessStateCode)
                .HasConstraintName("FK_Users_States");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.UserDetail1).HasName("PK__UserDeta__BA66F1239BFC9A4B");

            entity.Property(e => e.UserDetail1).HasColumnName("UserDetail");
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
            entity.HasKey(e => e.UserMatchesId).HasName("PK__UserMatc__B6D9FCF4D7952B0E");

            entity.HasOne(d => d.Match).WithMany(p => p.UserMatches)
                .HasForeignKey(d => d.MatchId)
                .HasConstraintName("FK__UserMatch__Match__5535A963");

            entity.HasOne(d => d.User).WithMany(p => p.UserMatches)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserMatch__UserI__5441852A");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRolesId).HasName("PK__UserRole__43D8BF2D0EF6A025");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRoles__RoleI__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoles__UserI__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
