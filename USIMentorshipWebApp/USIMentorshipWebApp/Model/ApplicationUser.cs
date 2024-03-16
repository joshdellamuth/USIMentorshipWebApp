using Microsoft.AspNetCore.Identity;

namespace USIMentorshipWebApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Additional properties specific to your application
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Default properties from IdentityUser
        public override string? UserName { get; set; }
        public override string? NormalizedUserName { get; set; }
        public override string? Email { get; set; }
        public override string? NormalizedEmail { get; set; }
        public override bool EmailConfirmed { get; set; }
        public override string? PasswordHash { get; set; }
        public override string? SecurityStamp { get; set; }
        public override string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public override string? PhoneNumber { get; set; }
        public override bool PhoneNumberConfirmed { get; set; }
        public override bool TwoFactorEnabled { get; set; }
        public override DateTimeOffset? LockoutEnd { get; set; }
        public override bool LockoutEnabled { get; set; } = true;
        public override int AccessFailedCount { get; set; }

        // Add more properties or methods as needed
    }
}
