/*
  Aurelia Grand Hotel - special guest requests
  Run once in SQL Server Management Studio against AureliaGrandHotel.
*/
USE AureliaGrandHotel;
GO

IF OBJECT_ID(N'dbo.SpecialGuestRequests', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SpecialGuestRequests
    (
        RequestID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SpecialGuestRequests PRIMARY KEY,
        GuestName NVARCHAR(160) NOT NULL,
        RoomNumber NVARCHAR(20) NULL,
        RequestDetails NVARCHAR(1000) NOT NULL,
        Priority NVARCHAR(20) NOT NULL CONSTRAINT DF_SpecialGuestRequests_Priority DEFAULT ('Normal'),
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_SpecialGuestRequests_Status DEFAULT ('Pending'),
        CreatedByUserID INT NULL,
        CreatedByUsername NVARCHAR(100) NOT NULL,
        CreatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_SpecialGuestRequests_CreatedAt DEFAULT (SYSDATETIME()),
        UpdatedAt DATETIME2(0) NOT NULL CONSTRAINT DF_SpecialGuestRequests_UpdatedAt DEFAULT (SYSDATETIME()),
        CompletedAt DATETIME2(0) NULL,
        CompletedByUsername NVARCHAR(100) NULL,
        CONSTRAINT CK_SpecialGuestRequests_Priority CHECK (Priority IN ('Normal','High','Urgent')),
        CONSTRAINT CK_SpecialGuestRequests_Status CHECK (Status IN ('Pending','In Progress','Completed','Cancelled'))
    );

    CREATE INDEX IX_SpecialGuestRequests_Status_CreatedAt
        ON dbo.SpecialGuestRequests(Status, CreatedAt DESC);
END;
GO

SELECT RequestID, GuestName, RoomNumber, Priority, Status, CreatedAt
FROM dbo.SpecialGuestRequests
ORDER BY CreatedAt DESC;
GO
