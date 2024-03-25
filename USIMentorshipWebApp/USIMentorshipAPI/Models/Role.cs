using System;
using System.Collections.Generic;

namespace USIMentorshipAPI.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleDescription { get; set; }
}
