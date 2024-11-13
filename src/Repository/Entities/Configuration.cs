using Repository.Entities.Base;

namespace Repository.Entities;

public class Configuration : BaseEntity
{
    public string ConfigKey { get; set; }
    public string Value { get; set; }
    public string? Description { get; set; }
}