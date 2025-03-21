using AccountManager.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Repository;

/// <summary>
///     EF Core Database Context with Identity, file metadata, and account entities.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<AccountSubscription> AccountSubscriptions { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<AccountSubscriptionStatus> AccountSubscriptionStatuses { get; set; } = null!;
    public DbSet<AccountChangesLog> AccountChangesLogs { get; set; } = null!;
}