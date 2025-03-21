﻿// <auto-generated />
using System;
using AccountManager.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountManager.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AccountManager.Domain.Entities.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Is2FAEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<bool>("IsIPFilterEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSessionTimeoutEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("LocalTimeZone")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SessionTimeOut")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("AccountManager.Domain.Entities.AccountChangesLog", b =>
                {
                    b.Property<int>("AccountChangesLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountChangesLogId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("ChangedField")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NewValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("AccountChangesLogId");

                    b.ToTable("AccountChangesLogs");
                });

            modelBuilder.Entity("AccountManager.Domain.Entities.AccountSubscriptionStatus", b =>
                {
                    b.Property<int>("SubscriptionStatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubscriptionStatusId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCancelled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("SubscriptionStatusId");

                    b.ToTable("AccountSubscriptionStatuses");
                });

            modelBuilder.Entity("AccountManager.Domain.Entities.Subscription", b =>
                {
                    b.Property<int>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubscriptionId"));

                    b.Property<bool>("AvailableYearly")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Is2FAAllowed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIPFilterAllowed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSessionTimeoutAllowed")
                        .HasColumnType("bit");

                    b.HasKey("SubscriptionId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("AccountSubscription", b =>
                {
                    b.Property<int>("AccountSubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountSubscriptionId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<bool>("Is2FAAllowed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIPFilterAllowed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSessionTimeoutAllowed")
                        .HasColumnType("bit");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("int");

                    b.Property<int>("SubscriptionStatusId")
                        .HasColumnType("int");

                    b.HasKey("AccountSubscriptionId");

                    b.HasIndex("AccountId");

                    b.HasIndex("SubscriptionId");

                    b.HasIndex("SubscriptionStatusId");

                    b.ToTable("AccountSubscriptions");
                });

            modelBuilder.Entity("AccountSubscription", b =>
                {
                    b.HasOne("AccountManager.Domain.Entities.Account", "Account")
                        .WithMany("AccountSubscriptions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AccountManager.Domain.Entities.Subscription", "Subscription")
                        .WithMany()
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("AccountManager.Domain.Entities.AccountSubscriptionStatus", "AccountSubscriptionStatus")
                        .WithMany()
                        .HasForeignKey("SubscriptionStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("AccountSubscriptionStatus");

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("AccountManager.Domain.Entities.Account", b =>
                {
                    b.Navigation("AccountSubscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
