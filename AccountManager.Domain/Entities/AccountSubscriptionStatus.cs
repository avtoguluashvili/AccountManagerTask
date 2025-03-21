using System.ComponentModel.DataAnnotations;

namespace AccountManager.Domain.Entities;

/// <summary>
///     Represents the status of an account's subscription (e.g., Active, Cancelled, etc.).
/// </summary>
public class AccountSubscriptionStatus
{
    /// <summary>
    ///     Primary key for the SubscriptionStatus table.
    /// </summary>
    [Key]
    public int SubscriptionStatusId { get; set; }

    /// <summary>
    ///     Descriptive name of the status (e.g. "Active", "Cancelled").
    /// </summary>
    public string Description { get; set; } = string.Empty;
}