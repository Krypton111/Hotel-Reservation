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

    IF COL_LENGTH(N'dbo.Properties', N'IsActive') IS NOT NULL
        UPDATE dbo.Properties
        SET IsArchived = 1,
            ArchivedAt = COALESCE(ArchivedAt, SYSDATETIME())
        WHERE IsActive = 0 AND IsArchived = 0;
END;
GO

SELECT N'Guests' AS TableName, COUNT(*) AS ActiveRecords FROM dbo.Guests WHERE IsArchived = 0
UNION ALL SELECT N'Rooms', COUNT(*) FROM dbo.Rooms WHERE IsArchived = 0
UNION ALL SELECT N'Reservations', COUNT(*) FROM dbo.Reservations WHERE IsArchived = 0
UNION ALL SELECT N'Payments', COUNT(*) FROM dbo.Payments WHERE IsArchived = 0
UNION ALL SELECT N'Users', COUNT(*) FROM dbo.Users WHERE IsArchived = 0;
GO

/*
    Refresh reporting views so archived records do not appear in active reports.
    Archived rows remain in their base tables and can be restored by the app.
*/
GO
CREATE OR ALTER VIEW dbo.vw_ReservationReport AS
SELECT
    r.ReservationID, r.ReferenceNumber, g.GuestID, g.FullName AS GuestName,
    g.ContactNumber, g.Email, rm.RoomID, rm.RoomNumber, rm.RoomType,
    r.CheckInDate, r.CheckOutDate, r.NumberOfNights, r.NumberOfGuests,
    r.RoomRate, r.TotalAmount, r.ReservationStatus, r.PaymentStatus,
    r.BookingDate, r.SpecialRequests, r.Notes,
    u.FullName AS CreatedBy
FROM dbo.Reservations r
INNER JOIN dbo.Guests g ON r.GuestID = g.GuestID
INNER JOIN dbo.Rooms rm ON r.RoomID = rm.RoomID
LEFT JOIN dbo.Users u ON r.CreatedByUserID = u.UserID
WHERE r.IsArchived = 0;
GO

CREATE OR ALTER VIEW dbo.vw_GuestReport AS
SELECT
    g.GuestID, g.FullName, g.ContactNumber, g.Email, g.City, g.Country,
    COUNT(r.ReservationID) AS ReservationCount,
    COALESCE(SUM(CASE WHEN r.ReservationStatus <> 'Cancelled' THEN r.TotalAmount ELSE 0 END), 0) AS TotalReservationValue
FROM dbo.Guests g
LEFT JOIN dbo.Reservations r ON g.GuestID = r.GuestID AND r.IsArchived = 0
WHERE g.IsArchived = 0
GROUP BY g.GuestID, g.FullName, g.ContactNumber, g.Email, g.City, g.Country;
GO

CREATE OR ALTER VIEW dbo.vw_PaymentReport AS
SELECT
    p.PaymentID, p.PaymentDate, p.PaymentAmount, p.PaymentMethod, p.PaymentStatus,
    p.ReferenceNumber AS PaymentReference,
    r.ReservationID, r.ReferenceNumber AS ReservationReference,
    g.FullName AS GuestName, rm.RoomNumber
FROM dbo.Payments p
INNER JOIN dbo.Reservations r ON p.ReservationID = r.ReservationID
INNER JOIN dbo.Guests g ON r.GuestID = g.GuestID
INNER JOIN dbo.Rooms rm ON r.RoomID = rm.RoomID
WHERE p.IsArchived = 0;
GO

CREATE OR ALTER VIEW dbo.vw_RoomReport AS
SELECT
    rm.RoomID, rm.RoomNumber, rm.RoomType, rm.FloorNumber, rm.FloorNumber AS Floor,
    rm.Capacity, rm.BedType, rm.RatePerNight, rm.Status, rm.Description,
    COUNT(r.ReservationID) AS ReservationCount
FROM dbo.Rooms rm
LEFT JOIN dbo.Reservations r ON rm.RoomID = r.RoomID AND r.IsArchived = 0
WHERE rm.IsArchived = 0
GROUP BY rm.RoomID, rm.RoomNumber, rm.RoomType, rm.FloorNumber, rm.Capacity,
         rm.BedType, rm.RatePerNight, rm.Status, rm.Description;
GO

CREATE OR ALTER VIEW dbo.vw_RoomSummary AS
SELECT
    COUNT(*) AS TotalRooms,
    SUM(CASE WHEN Status = 'Available' THEN 1 ELSE 0 END) AS AvailableRooms,
    SUM(CASE WHEN Status = 'Occupied' THEN 1 ELSE 0 END) AS OccupiedRooms,
    SUM(CASE WHEN Status = 'Reserved' THEN 1 ELSE 0 END) AS ReservedRooms,
    SUM(CASE WHEN Status = 'Maintenance' THEN 1 ELSE 0 END) AS MaintenanceRooms
FROM dbo.Rooms
WHERE IsArchived = 0;
GO

CREATE OR ALTER VIEW dbo.vw_TodaysCheckIns AS
SELECT r.ReservationID, r.ReferenceNumber, g.FullName AS GuestName,
       rm.RoomNumber, rm.RoomType, r.CheckInDate, r.CheckOutDate,
       r.NumberOfGuests, r.TotalAmount, r.ReservationStatus
FROM dbo.Reservations r
JOIN dbo.Guests g ON r.GuestID = g.GuestID
JOIN dbo.Rooms rm ON r.RoomID = rm.RoomID
WHERE r.IsArchived = 0
  AND r.CheckInDate = CONVERT(date, GETDATE())
  AND r.ReservationStatus IN ('Reserved', 'Checked-In');
GO

CREATE OR ALTER VIEW dbo.vw_TodaysCheckOuts AS
SELECT r.ReservationID, r.ReferenceNumber, g.FullName AS GuestName,
       rm.RoomNumber, rm.RoomType, r.CheckInDate, r.CheckOutDate,
       r.NumberOfGuests, r.TotalAmount, r.ReservationStatus
FROM dbo.Reservations r
JOIN dbo.Guests g ON r.GuestID = g.GuestID
JOIN dbo.Rooms rm ON r.RoomID = rm.RoomID
WHERE r.IsArchived = 0
  AND r.CheckOutDate = CONVERT(date, GETDATE())
  AND r.ReservationStatus = 'Checked-In';
GO

CREATE OR ALTER VIEW dbo.vw_RevenueSummary AS
SELECT
    YEAR(p.PaymentDate) AS PaymentYear,
    MONTH(p.PaymentDate) AS PaymentMonth,
    SUM(p.PaymentAmount) AS TotalPayments
FROM dbo.Payments p
WHERE p.IsArchived = 0
GROUP BY YEAR(p.PaymentDate), MONTH(p.PaymentDate);
GO
