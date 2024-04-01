using System;
using System.Collections.Generic;

namespace USIMentorshipWebApp.Models;

public partial class State
{
    public string StateName { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
