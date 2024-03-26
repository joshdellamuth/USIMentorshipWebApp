using System;
using System.Collections.Generic;

namespace USIMentorshipAPI.Models;

public partial class ConditionDetail
{
    public int ConditionDetailId { get; set; }

    public int? MatchId { get; set; }

    public string? ConditionName { get; set; }

    public string? ConditionValue { get; set; }

    public virtual Match? Match { get; set; }
}
