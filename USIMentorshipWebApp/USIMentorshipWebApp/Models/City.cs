using System;
using System.Collections.Generic;

namespace USIMentorshipWebApp.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public virtual State StateCodeNavigation { get; set; } = null!;
}
