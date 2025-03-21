-- 1) Subscriptions
------------------------------------------------------------------
SET IDENTITY_INSERT [Subscriptions] ON;

INSERT INTO [Subscriptions]
       ([SubscriptionId], [Description], [IsDefault], [IsActive],
        [AvailableYearly], [Is2FAAllowed], [IsIPFilterAllowed], [IsSessionTimeoutAllowed])
VALUES (1, 'Free Plan',       1, 1, 0, 0, 0, 0),
       (2, 'Premium Plan',    0, 1, 0, 1, 0, 0),
       (3, 'Enterprise Plan', 0, 1, 1, 1, 1, 1);

SET IDENTITY_INSERT [Subscriptions] OFF;

------------------------------------------------------------------
-- 2) AccountSubscriptionStatuses
------------------------------------------------------------------
SET IDENTITY_INSERT [AccountSubscriptionStatuses] ON;

INSERT INTO [AccountSubscriptionStatuses]
       ([SubscriptionStatusId], [Description])
VALUES (1, 'Active'),
       (2, 'Cancelled'),
       (3, 'Expired');

SET IDENTITY_INSERT [AccountSubscriptionStatuses] OFF;

------------------------------------------------------------------
-- 3) Accounts
------------------------------------------------------------------
SET IDENTITY_INSERT [Accounts] ON;

INSERT INTO [Accounts]
       ([AccountId], [Token], [IsActive], [CompanyName], [Country],
        [CreatedAt], [Is2FAEnabled], [IsIPFilterEnabled], [IsSessionTimeoutEnabled],
        [SessionTimeOut], [LocalTimeZone])
VALUES (1, '00000000-0000-0000-0000-000000000001', 1, 'Alpha Corp',   'USA',      '2025-03-19 12:00:00', 1, 0, 1, 30, 'America/New_York'),
       (2, '00000000-0000-0000-0000-000000000002', 0, 'Beta LLC',     'UK',       '2025-03-19 13:00:00', 0, 1, 0,  0, 'Europe/London'),
       (3, '00000000-0000-0000-0000-000000000003', 1, 'Gamma Systems', 'Canada',   '2025-03-20 09:00:00', 0, 0, 0,  0, 'America/Toronto'),
       (4, '00000000-0000-0000-0000-000000000004', 1, 'Delta Corp',   'Germany',  '2025-03-20 10:00:00', 1, 0, 1,  20, 'Europe/Berlin');

SET IDENTITY_INSERT [Accounts] OFF;

------------------------------------------------------------------
-- 4) AccountSubscriptions
------------------------------------------------------------------
SET IDENTITY_INSERT [AccountSubscriptions] ON;

INSERT INTO [AccountSubscriptions]
       ([AccountSubscriptionId], [SubscriptionId], [AccountId],
        [SubscriptionStatusId], [Is2FAAllowed], [IsIPFilterAllowed], [IsSessionTimeoutAllowed])
VALUES (1, 1, 1, 1, 0, 0, 1),
       (2, 2, 2, 1, 1, 0, 0),
       (3, 3, 3, 2, 1, 1, 1),
       (4, 2, 4, 1, 1, 0, 1);

SET IDENTITY_INSERT [AccountSubscriptions] OFF;