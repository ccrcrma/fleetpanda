-- For source 
use FleetPanda
CREATE TABLE Customer (
    CustomerID INTEGER PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(20) NOT NULL
);

CREATE TABLE Location (
    LocationID INTEGER PRIMARY KEY,
    CustomerID INTEGER,
    Address VARCHAR(MAX) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);

GO;

INSERT INTO Customer (CustomerID, Name, Email, Phone) VALUES
    (1, 'John Doe', 'johndoe@example.com', '123-456-7890'),
    (2, 'Jane Smith', 'janesmith@example.com', '234-567-8901'),
    (3, 'Alice Brown', 'alicebrown@example.com', '345-678-9012'),
    (4, 'Bob Johnson', 'bobjohnson@example.com', '456-789-0123'),
    (5, 'Emily Davis', 'emilydavis@example.com', '567-890-1234'),
    (6, 'Michael Wilson', 'michaelwilson@example.com', '678-901-2345'),
    (7, 'Sophia Anderson', 'sophiaanderson@example.com', '789-012-3456'),
    (8, 'David Clark', 'davidclark@example.com', '890-123-4567'),
    (9, 'Linda Martinez', 'lindamartinez@example.com', '901-234-5678'),
    (10, 'James Lewis', 'jameslewis@example.com', '012-345-6789'),
    (11, 'Olivia Hall', 'oliviahall@example.com', '123-987-6543'),
    (12, 'Ethan Young', 'ethanyoung@example.com', '234-876-5432'),
    (13, 'Liam King', 'liamking@example.com', '345-765-4321'),
    (14, 'Mia Scott', 'miascott@example.com', '456-654-3210'),
    (15, 'Noah Green', 'noahgreen@example.com', '567-543-2109'),
    (16, 'Ava Adams', 'avaadams@example.com', '678-432-1098'),
    (17, 'Charlotte Turner', 'charlotteturner@example.com', '789-321-0987'),
    (18, 'Elijah Baker', 'elijahbaker@example.com', '890-210-9876'),
    (19, 'Amelia Parker', 'ameliaparker@example.com', '901-109-8765'),
    (20, 'Lucas Collins', 'lucascollins@example.com', '012-098-7654');


	INSERT INTO Location (LocationID, CustomerID, Address) VALUES
    -- Locations for Customer 1
    (1, 1, '123 Main St, Springfield'),
    (2, 1, '456 Oak St, Springfield'),

    -- Locations for Customer 2
    (3, 2, '789 Pine St, Lincoln'),

    -- Locations for Customer 3
    (4, 3, '321 Maple Ave, Lincoln'),
    (5, 3, '654 Birch Rd, Lincoln'),

    -- Locations for Customer 4
    (6, 4, '789 Willow St, Oakville'),
    (7, 4, '101 Elm St, Oakville'),

    -- Locations for Customer 5
    (8, 5, '202 Cedar Ave, Maplewood'),
    (9, 5, '303 Aspen Ln, Maplewood'),

    -- Locations for Customer 6
    (10, 6, '404 Spruce St, Lakeview'),

    -- Locations for Customer 7
    (11, 7, '505 Cypress Rd, Lakeview'),
    (12, 7, '606 Redwood Dr, Riverwood'),

    -- Locations for Customer 8
    (13, 8, '707 Palm St, Baytown'),
    (14, 8, '808 Acacia Ct, Baytown'),

    -- Locations for Customer 9
    (15, 9, '909 Magnolia Ave, Greenfield'),

    -- Locations for Customer 10
    (16, 10, '1010 Poplar St, Greenfield'),
    (17, 10, '1111 Juniper Rd, Greenfield'),

    -- Locations for Customer 11
    (18, 11, '1212 Sycamore St, Hillside'),

    -- Locations for Customer 12
    (19, 12, '1313 Chestnut Ln, Hillside'),
    (20, 12, '1414 Walnut St, Hillside'),

    -- Locations for Customer 13
    (21, 13, '1515 Maple Dr, Townsville'),

    -- Locations for Customer 14
    (22, 14, '1616 Oak Rd, Rivertown'),
    (23, 14, '1717 Cedar St, Rivertown'),

    -- Locations for Customer 15
    (24, 15, '1818 Birch Blvd, Cityville'),

    -- Locations for Customer 16
    (25, 16, '1919 Pine Ln, Lakewood'),

    -- Locations for Customer 17
    (26, 17, '2020 Redwood St, Hillside'),

    -- Locations for Customer 18
    (27, 18, '2121 Aspen Ct, Riverbend'),
    (28, 18, '2222 Oakwood Dr, Riverbend'),

    -- Locations for Customer 19
    (29, 19, '2323 Elm St, Meadowville'),

    -- Locations for Customer 20
    (30, 20, '2424 Cedar Ave, Lakeside'),
    (31, 20, '2525 Pine St, Lakeside');

GO;
-- For Destination
CREATE DATABASE FleetTarget;
USE FleetTarget

CREATE TABLE Customer (
    CustomerID INTEGER PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(20) NOT NULL
);

CREATE TABLE Location (
    LocationID INTEGER PRIMARY KEY,
    CustomerID INTEGER,
    Address VARCHAR(MAX) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);


CREATE TABLE SyncHistory(
SyncHistoryId bigINT Identity(1,1) PRIMARY KEY,
TimeStamp DateTime,
Log nvarchar(max)
)

CREATE TABLE SchedulerSettings(
SchedulerSettingsId INTEGER PRIMARY KEY,
Description VARCHAR(255),
Regex VARCHAR(MAX),
CreatedDate DATETIME,
IsActive bit
)

CREATE TABLE CustomerAuditLog(
CustomerId INT,
Changes NVARCHAR(MAX),
ModifiedAt DateTime,
ChangedBy NVARCHAR(100)
)

INSERT INTO Customer (CustomerID, Name, Email, Phone) VALUES
    (1, 'John Doe', 'johndoe@example.com', '123-456-7890')

select * from Customer

Update Customer 
set Name='Jane Doe',
Email='janedoe@example.com'
WHERE CustomerID=1

GO;

CREATE TRIGGER trigger_customer_after_update_log
ON Customer
AFTER UPDATE
AS
BEGIN
    DECLARE @changed_by NVARCHAR(100) = SUSER_NAME(); 

    INSERT INTO CustomerAuditLog (CustomerId, changes, ModifiedAt, changedby)
    SELECT 
        i.CustomerID,
        (
            CASE 
                WHEN d.Name != i.Name THEN '"name":{"old_value":"' + d.Name + '","new_value":"' + i.Name + '"},'
                ELSE '' 
            END +
            CASE 
                WHEN d.Email != i.Email THEN '"email":{"old_value":"' + d.Email + '","new_value":"' + i.Email + '"},'
                ELSE '' 
            END +
            CASE 
                WHEN d.Phone != i.Phone THEN '"phone":{"old_value":"' + d.Phone + '","new_value":"' + i.Phone + '"}'
                ELSE '' 
            END
        ) AS changes,
        
        GETDATE() AS changed_at,
        @changed_by AS changed_by
    FROM 
        deleted d
    JOIN 
        inserted i ON d.CustomerID = i.CustomerID
    WHERE 
        d.Name != i.Name OR d.Email != i.Email OR d.Phone != i.Phone;  -- Only log rows with actual changes

    -- Remove trailing commas from JSON string
    UPDATE CustomerAuditLog
    SET changes = 
        '{' + LEFT(changes, LEN(changes) - 1) + '}'
    WHERE changes LIKE '%},';
END;

CREATE TYPE CustomerType AS TABLE(
	CustomerId int NULL,
	Name VARCHAR(255) NULL,
	Email VARCHAR(255)  NULL,
	Phone VARCHAR(20) NULL)
GO



CREATE PROCEDURE [dbo].[proc_sync_customers](
 @customer_records dbo.CustomerType ReadOnly 
)
AS
BEGIN
	BEGIN TRY
		BEGIN TRANSACTION;
 
				IF NOT EXISTS (SELECT 1 FROM @customer_records)
				BEGIN;
					THROW 51000, 'Please provide @customer_records field', 1;
					RETURN; 
				END

			--INSERT INTO SyncHistory Table
			INSERT INTO SyncHistory(TimeStamp, Log) VALUES(GETDATE(), 'Data Synchronization');

			MERGE dbo.Customer AS Target
			USING @customer_records	AS Source
			ON Source.CustomerId = Target.CustomerId 

			---FOR INSERTS
			WHEN NOT MATCHED BY Target THEN
				INSERT (CustomerId,Name, Email, Phone) 
				VALUES (Source.CustomerId,Source.Name, Source.Email, Source.Phone)

			---FOR UPDATES
			WHEN MATCHED THEN UPDATE 
				SET Target.Name = Source.Name,
					Target.Email = Source.Email,
					Target.Phone = Source.Phone;

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	-- Rollback the transaction if an error occurs
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    -- Capture error information
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT 
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    -- Raise an error with the information
    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END