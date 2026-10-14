/*
  Aurelia Grand Hotel - special guest requests migration
  Run this script once in SQL Server Management Studio against AureliaGrandHotel.

  If an incompatible table named dbo.SpecialGuestRequests already exists, this
  script preserves it as dbo.SpecialGuestRequests_Legacy before creating the
  schema expected by SpecialRequestsForm.vb.
*/
USE AureliaGrandHotel;
GO

SET XACT_ABORT ON;
GO

/*
  The previous script assumed that any existing table with this name had the
  correct columns. Detect an incompatible table first. SQL Server cannot add
  an IDENTITY primary-key column to an existing table in the same way as a
  fresh CREATE TABLE, so preserve the old table under a separate name.
*/
IF OBJECT_ID(N'dbo.SpecialGuestRequests', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.SpecialGuestRequests', N'RequestID') IS NULL
BEGIN
    IF OBJECT_ID(N'dbo.SpecialGuestRequests_Legacy', N'U') IS NOT NULL
        THROW 51000, 'dbo.SpecialGuestRequests exists with an incompatible schema, and dbo.SpecialGuestRequests_Legacy already exists. Rename or back up one of these tables manually before rerunning this migration.', 1;

    EXEC sys.sp_rename N'dbo.SpecialGuestRequests', N'SpecialGuestRequests_Legacy';
END;
GO

IF OBJECT_ID(N'dbo.SpecialGuestRequests', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SpecialGuestRequests
    (
        RequestID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SpecialGuestRequests PRIMARY KEY,
        RequestNumber AS ('SR-' + RIGHT('000000' + CONVERT(VARCHAR(12), RequestID), 6)) PERSISTED,
        GuestName NVARCHAR(160) NOT NULL,
        RoomNumber NVARCHAR(20) NULL,
        RequestDetails NVARCHAR(1000) NOT NULL,
        Priority NVARCHAR(20) NOT NULL CONSTRAINT DF_SpecialGuestRequests_Priority DEFAULT ('Normal'),
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_SpecialGuestRequests_Status DEFAULT ('Pending'),
        CreatedByUserID INT NULL,
        CreatedByUsername NVARCHAR(100) NOT NULL CONSTRAINT DF_SpecialGuestRequests_CreatedByUsername DEFAULT ('unknown'),
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_SpecialGuestRequests_CreatedAt DEFAULT (SYSDATETIME()),
        UpdatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_SpecialGuestRequests_UpdatedAt DEFAULT (SYSDATETIME()),
        CompletedAt DATETIME2(0) NULL,
        CompletedByUsername NVARCHAR(100) NULL,
        CONSTRAINT CK_SpecialGuestRequests_Priority CHECK (Priority IN ('Normal','High','Urgent')),
        CONSTRAINT CK_SpecialGuestRequests_Status CHECK (Status IN ('Pending','In Progress','Completed','Cancelled'))
    );
END;
GO

/* Add the queue number when the table is otherwise already compatible. */
IF OBJECT_ID(N'dbo.SpecialGuestRequests', N'U') IS NOT NULL
   AND COL_LENGTH(N'dbo.SpecialGuestRequests', N'RequestID') IS NOT NULL
   AND COL_LENGTH(N'dbo.SpecialGuestRequests', N'RequestNumber') IS NULL
BEGIN
    ALTER TABLE dbo.SpecialGuestRequests
    ADD RequestNumber AS ('SR-' + RIGHT('000000' + CONVERT(VARCHAR(12), RequestID), 6)) PERSISTED;
END;
GO

IF NOT EXISTS
(
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.SpecialGuestRequests')
      AND name = N'IX_SpecialGuestRequests_Status_CreatedAt'
)
BEGIN
    CREATE INDEX IX_SpecialGuestRequests_Status_CreatedAt
        ON dbo.SpecialGuestRequests(Status, CreatedAt DESC);
END;
GO

/* Verify that the expected columns now exist. */
SELECT RequestID, RequestNumber, GuestName, RoomNumber, Priority, Status, CreatedAt
FROM dbo.SpecialGuestRequests
ORDER BY CreatedAt DESC;
GO
