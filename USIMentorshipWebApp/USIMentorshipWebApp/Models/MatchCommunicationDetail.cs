using System;
using System.Collections.Generic;

namespace USIMentorshipWebApp.Models;

public partial class MatchCommunicationDetail
{
    public int CommunicationDetailId { get; set; }

    public int? MatchId { get; set; }

    public int? SenderUserId { get; set; }

    public string? CommunicationType { get; set; }

    public string? CommunicationContent { get; set; }

    public DateTime? DateOfCommunication { get; set; }

    public bool? Deleted { get; set; }

    public virtual Match? Match { get; set; }

    public virtual User? SenderUser { get; set; }
}
