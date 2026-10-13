/*
    Aurelia Grand Hotel - restore missing starter room records

    Restores the original RoomID values 1-6 and 10 for rooms 101-106 and 110.
    Existing RoomID and RoomNumber values are never overwritten.
    Run this script in SQL Server Management Studio.
*/
USE AureliaGrandHotel;
GO

SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Rooms', N'U') IS NULL
BEGIN
    THROW 50001, 'dbo.Rooms was not found in AureliaGrandHotel.', 1;
END;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    SET IDENTITY_INSERT dbo.Rooms ON;

    INSERT INTO dbo.Rooms
        (RoomID, RoomNumber, RoomType, FloorNumber, Capacity, BedType,
         RatePerNight, Status, Description)
    SELECT
        seed.RoomID,
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
        (1, N'101', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
        (2, N'102', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
        (3, N'103', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
        (4, N'104', N'Standard Room', 1, 2, N'Queen Bed', 3200.00, N'Occupied', N'Standard Room with queen bed, complimentary breakfast, Wi-Fi and city views.'),
        (5, N'105', N'Deluxe Room', 1, 2, N'King Bed', 4200.00, N'Occupied', N'Deluxe Room with king bed, complimentary breakfast, Wi-Fi and city views.'),
        (6, N'106', N'Deluxe Room', 1, 2, N'King Bed', 4200.00, N'Occupied', N'Deluxe Room with king bed, complimentary breakfast, Wi-Fi and city views.'),
        (10, N'110', N'Superior Room', 1, 3, N'King Bed', 5200.00, N'Reserved', N'Superior Room with king bed, complimentary breakfast, Wi-Fi and city views.')
    ) AS seed
        (RoomID, RoomNumber, RoomType, FloorNumber, Capacity, BedType,
         RatePerNight, Status, Description)
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM dbo.Rooms AS existing
        WHERE existing.RoomID = seed.RoomID
           OR existing.RoomNumber = seed.RoomNumber
    );

    SET IDENTITY_INSERT dbo.Rooms OFF;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    BEGIN TRY
        SET IDENTITY_INSERT dbo.Rooms OFF;
    END TRY
    BEGIN CATCH
        -- Ignore cleanup errors so the original error is preserved.
    END CATCH;

    THROW;
END CATCH;
GO

SELECT RoomID, RoomNumber, RoomType, FloorNumber, Capacity,
       RatePerNight, Status
FROM dbo.Rooms
WHERE RoomID IN (1, 2, 3, 4, 5, 6, 10)
   OR RoomNumber IN (N'101', N'102', N'103', N'104', N'105', N'106', N'110')
ORDER BY RoomID;
