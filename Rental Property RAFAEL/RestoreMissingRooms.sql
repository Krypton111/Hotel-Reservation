/*
    Aurelia Grand Hotel - restore missing starter room records
    Safe to run more than once: existing RoomNumber values are left unchanged.
    Run in SQL Server Management Studio against the AureliaGrandHotel database.
*/
USE AureliaGrandHotel;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.Rooms', N'U') IS NULL
BEGIN
    THROW 50001, 'dbo.Rooms was not found in AureliaGrandHotel.', 1;
END;

INSERT INTO dbo.Rooms
    (RoomNumber, RoomType, FloorNumber, Capacity, BedType,
     RatePerNight, Status, Description)
SELECT
    seed.RoomNumber,
    seed.RoomType,
    seed.FloorNumber,
    seed.Capacity,
    seed.BedType,
    seed.RatePerNight,
    seed.Status,
    seed.Description
FROM
(
    VALUES
    (N'101', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
    (N'102', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
    (N'103', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
    (N'104', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
    (N'105', N'Deluxe Room', 1, 2, N'King Bed', 4200.00, N'Occupied', N'Deluxe Room with king bed, complimentary breakfast, Wi-Fi and city views.'),
    (N'106', N'Deluxe Room', 1, 2, N'King Bed', 4200.00, N'Occupied', N'Deluxe Room with king bed, complimentary breakfast, Wi-Fi and city views.'),
    (N'110', N'Superior Room', 1, 3, N'King Bed', 5200.00, N'Reserved', N'Superior Room with king bed, complimentary breakfast, Wi-Fi and city views.')
) AS seed
    (RoomNumber, RoomType, FloorNumber, Capacity, BedType,
     RatePerNight, Status, Description)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Rooms AS existing
    WHERE existing.RoomNumber = seed.RoomNumber
);

COMMIT TRANSACTION;

SELECT RoomID, RoomNumber, RoomType, FloorNumber, Capacity,
       RatePerNight, Status
FROM dbo.Rooms
WHERE RoomNumber IN (N'101', N'102', N'103', N'104', N'105', N'106', N'110')
ORDER BY TRY_CONVERT(INT, RoomNumber), RoomID;
