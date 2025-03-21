using System.ComponentModel.DataAnnotations;

namespace AccountManager.Domain.Entities;

/// <summary>
///     Represents a subscription plan template.
/// </summary>
public class Subscription
{
    /// <summary>
    ///     Primary key for the Subscription table.
    /// </summary>
    [Key]
    public int SubscriptionId { get; set; }

    /// <summary>
    ///     Description or name of this subscription plan.
    /// </summary>
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///     Indicates if this subscription plan is the default plan.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    ///     Indicates if this subscription plan is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    ///     Indicates if plan is available yearly (versus monthly).
    /// </summary>
    public bool AvailableYearly { get; set; }

    /// <summary>
    ///     Indicates if plan allows 2FA.
    /// </summary>
    public bool Is2FaAllowed { get; set; }

    /// <summary>
    ///     Indicates if plan allows IP filter.
    /// </summary>
    public bool IsIpFilterAllowed { get; set; }

    /// <summary>
    ///     Indicates if plan allows session timeout.
    /// </summary>
    public bool IsSessionTimeoutAllowed { get; set; }
}