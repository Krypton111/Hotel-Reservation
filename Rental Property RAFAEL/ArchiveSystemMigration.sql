/*
    Aurelia Grand Hotel - archive system migration
    Run this script once in SQL Server Management Studio against AureliaGrandHotel.
    It is safe to re-run: existing archive columns are retained.
*/
USE AureliaGrandHotel;
GO

SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Guests', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Guests', N'IsArchived') IS NULL
        ALTER TABLE dbo.Guests ADD IsArchived BIT NOT NULL
            CONSTRAINT DF_Guests_IsArchived DEFAULT (0) WITH VALUES;
    IF COL_LENGTH(N'dbo.Guests', N'ArchivedAt') IS NULL
        ALTER TABLE dbo.Guests ADD ArchivedAt DATETIME2(0) NULL;
END;
GO

IF OBJECT_ID(N'dbo.Rooms', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Rooms', N'IsArchived') IS NULL
        ALTER TABLE dbo.Rooms ADD IsArchived BIT NOT NULL
            CONSTRAINT DF_Rooms_IsArchived DEFAULT (0) WITH VALUES;
    IF COL_LENGTH(N'dbo.Rooms', N'ArchivedAt') IS NULL
        ALTER TABLE dbo.Rooms ADD ArchivedAt DATETIME2(0) NULL;
END;
GO

IF OBJECT_ID(N'dbo.Reservations', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Reservations', N'IsArchived') IS NULL
        ALTER TABLE dbo.Reservations ADD IsArchived BIT NOT NULL
            CONSTRAINT DF_Reservations_IsArchived DEFAULT (0) WITH VALUES;
    IF COL_LENGTH(N'dbo.Reservations', N'ArchivedAt') IS NULL
        ALTER TABLE dbo.Reservations ADD ArchivedAt DATETIME2(0) NULL;
END;
GO

IF OBJECT_ID(N'dbo.Payments', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Payments', N'IsArchived') IS NULL
        ALTER TABLE dbo.Payments ADD IsArchived BIT NOT NULL
            CONSTRAINT DF_Payments_IsArchived DEFAULT (0) WITH VALUES;
    IF COL_LENGTH(N'dbo.Payments', N'ArchivedAt') IS NULL
        ALTER TABLE dbo.Payments ADD ArchivedAt DATETIME2(0) NULL;
END;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Users', N'IsArchived') IS NULL
        ALTER TABLE dbo.Users ADD IsArchived BIT NOT NULL
            CONSTRAINT DF_Users_IsArchived DEFAULT (0) WITH VALUES;
    IF COL_LENGTH(N'dbo.Users', N'ArchivedAt') IS NULL
        ALTER TABLE dbo.Users ADD ArchivedAt DATETIME2(0) NULL;
END;
GO

/* PropertiesForm is a legacy form and may not be present in every copy of the hotel DB. */
IF OBJECT_ID(N'dbo.Properties', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Properties', N'IsArchived') IS NULL
        EXEC(N'ALTER TABLE dbo.Properties ADD IsArchived BIT NOT NULL CONSTRAINT DF_Properties_IsArchived DEFAULT (0) WITH VALUES;');
    IF COL_LENGTH(N'dbo.Properties', N'ArchivedAt') IS NULL
        ALTER TABLE dbo.Properties ADD ArchivedAt DATETIME2(0) NULL;
END;
GO

SELECT N'Guests' AS TableName, COUNT(*) AS ActiveRecords FROM dbo.Guests WHERE IsArchived = 0
UNION ALL SELECT N'Rooms', COUNT(*) FROM dbo.Rooms WHERE IsArchived = 0
UNION ALL SELECT N'Reservations', COUNT(*) FROM dbo.Reservations WHERE IsArchived = 0
UNION ALL SELECT N'Payments', COUNT(*) FROM dbo.Payments WHERE IsArchived = 0
UNION ALL SELECT N'Users', COUNT(*) FROM dbo.Users WHERE IsArchived = 0;
GO
