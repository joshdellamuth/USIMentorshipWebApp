using System;
using System.Collections.Generic;

namespace USIMentorshipWebApp.Models;

public partial class UserMatch
{
    public int? UserId { get; set; }

    public int? MatchId { get; set; }

    public int UserMatchesId { get; set; }

    public virtual Match? Match { get; set; }

    public virtual User? User { get; set; }
}
