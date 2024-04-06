using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace USIMentorshipWebApp.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    
    // this will be used to show the full name of a user
    public string FullName => $"{FirstName} {LastName}";

    public string? PhoneNumber { get; set; }

    public string? EmailAddress { get; set; }

    public string? Password { get; set; }

    [NotMapped]
    public string? RepeatPassword { get; set; }

    public string? ProfilePicutre { get; set; }

    public string? LinkedInLink { get; set; }

    public string? Bio { get; set; }

    public string? Status { get; set; }

    public string? Major { get; set; }

    public string? Minor { get; set; }

    public string? Industry { get; set; }

    public string? Company { get; set; }

    public string? CurrentPosition { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? GraduationYear { get; set; }

    public string? BusinessCountry { get; set; }

    public string? BusinessState { get; set; }

    public string? BusinessCity { get; set; }

    public string? Gender { get; set; }

    public string? School { get; set; }

    public string? JobTitles { get; set; }

    public int? MenteesPerSemester { get; set; }

    public virtual ICollection<MatchCommunicationDetail> MatchCommunicationDetails { get; set; } = new List<MatchCommunicationDetail>();

    public virtual ICollection<UserDetail> UserDetails { get; set; } = new List<UserDetail>();

    public virtual ICollection<UserMatch> UserMatches { get; set; } = new List<UserMatch>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
