using System;
using System.Collections.Generic;

namespace USIMentorshipWebApp.Models;

public partial class School
{
    public int SchoolId { get; set; }

    public string? SchoolName { get; set; }

    public string? WebsiteUrl { get; set; }
}
