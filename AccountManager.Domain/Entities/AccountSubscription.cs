using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManager.Domain.Entities;

public class AccountSubscription
{
    /// <summary>
    ///     Primary key for the AccountSubscription table.
    /// </summary>
    [Key]
    public int AccountSubscriptionId { get; set; }

    /// <summary>
    ///     Foreign key to Subscription entity.
    /// </summary>
    public int SubscriptionId { get; set; }

    /// <summary>
    ///     Navigation property to the Subscription plan.
    /// </summary>
    public virtual Subscription? Subscription { get; set; }

    /// <summary>
    ///     Foreign key to Account.
    /// </summary>
    public int AccountId { get; set; }

    /// <summary>
    ///     Navigation property back to the Account.
    /// </summary>
    public virtual Account? Account { get; set; }

    /// <summary>
    ///     Foreign key to an AccountSubscriptionStatus.
    /// </summary>
    [ForeignKey("AccountSubscriptionStatus")]
    public int SubscriptionStatusId { get; set; }

    /// <summary>
    ///     Navigation property to the subscription status (Active, Cancelled, etc.).
    /// </summary>
    public virtual AccountSubscriptionStatus? AccountSubscriptionStatus { get; set; }

    /// <summary>
    ///     Indicates if 2FA is allowed by the chosen plan.
    /// </summary>
    public bool Is2FaAllowed { get; set; }

    /// <summary>
    ///     Indicates if IP filtering is allowed.
    /// </summary>
    public bool IsIpFilterAllowed { get; set; }

    /// <summary>
    ///     Indicates if session timeout is allowed.
    /// </summary>
    public bool IsSessionTimeoutAllowed { get; set; }
}