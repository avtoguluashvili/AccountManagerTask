using System.ComponentModel.DataAnnotations;

namespace AccountManager.Domain.Entities;

/// <summary>
///     Logs changes made to an account or its subscription fields.
/// </summary>
public class AccountChangesLog
{
    /// <summary>
    ///     Primary key for the AccountChangesLog table.
    /// </summary>
    [Key]
    public int LogId { get; set; }

    /// <summary>
    ///     Which account was changed?
    /// </summary>
    public int AccountId { get; set; }

    /// <summary>
    ///     Name of the field that changed.
    /// </summary>
    public string ChangedField { get; set; } = string.Empty;

    /// <summary>
    ///     The previous value of the field.
    /// </summary>
    public string OldValue { get; set; } = string.Empty;

    /// <summary>
    ///     The new value of the field.
    /// </summary>
    public string NewValue { get; set; } = string.Empty;

    /// <summary>
    ///     When the change occurred (UTC).
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}