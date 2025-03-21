using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManager.Domain.Entities;

/// <summary>
///     Represents a customer or client account.
/// </summary>
public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AccountId { get; set; }

    public string Token { get; set; } = Guid.NewGuid().ToString();
    public bool IsActive { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual AccountSubscription? AccountSubscription { get; set; }
    public bool Is2FaEnabled { get; set; }
    public bool IsIpFilterEnabled { get; set; }
    public bool IsSessionTimeoutEnabled { get; set; }
    public int SessionTimeOut { get; set; }
    public string? LocalTimeZone { get; set; }
}