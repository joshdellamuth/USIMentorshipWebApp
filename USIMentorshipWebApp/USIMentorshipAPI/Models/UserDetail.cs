using System;
using System.Collections.Generic;

namespace USIMentorshipAPI.Models;

public partial class UserDetail
{
    public int UserDetail1 { get; set; }

    public int? UserId { get; set; }

    public string? DetailName { get; set; }

    public string? DetailValue { get; set; }

    public virtual User? User { get; set; }
}
