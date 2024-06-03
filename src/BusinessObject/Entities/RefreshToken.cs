using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;

namespace BusinessObject.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public string JwtID { get; set; }
    public DateTimeOffset ExpiryDateTime { get; set; }
    public bool IsExpired => DateTimeOffset.UtcNow >= ExpiryDateTime;
    public int UserID { get; set; }
    
    [ForeignKey(nameof(UserID))]
    public virtual User User { get; set; }
}