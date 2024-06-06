using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Configuration
{
    public int Id { get; set; }

    public string? ConfigKey { get; set; }

    public string? Value { get; set; }

    public bool? IsActive { get; set; }
}
