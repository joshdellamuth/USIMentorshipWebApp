using System;
using System.Collections.Generic;

namespace USIMentorshipWebApp.Models;

public partial class Match
{
    public int MatchId { get; set; }

    public string? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual ICollection<ConditionDetail> ConditionDetails { get; set; } = new List<ConditionDetail>();

    public virtual ICollection<MatchCommunicationDetail> MatchCommunicationDetails { get; set; } = new List<MatchCommunicationDetail>();
}
