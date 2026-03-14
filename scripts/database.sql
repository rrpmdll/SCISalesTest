-- ============================================================
-- SCISalesTest Database Setup Script
-- SQL Server - Products Management System
-- ============================================================

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'SCISalesTestDb')
BEGIN
    CREATE DATABASE [SCISalesTestDb];
END
GO

USE [SCISalesTestDb];
GO

-- ============================================================
-- Table: Products
-- ============================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Products]
    (
        [Id]              INT            IDENTITY(1,1) NOT NULL,
        [Name]            NVARCHAR(200)  NOT NULL,
        [Description]     NVARCHAR(1000) NOT NULL,
        [Price]           DECIMAL(18,2)  NOT NULL,
        [UnitsInStock]    INT            NOT NULL DEFAULT 0,
        [CreatedDate]     DATETIME       NOT NULL DEFAULT GETUTCDATE(),
        [ModifiedDate]    DATETIME       NULL,
        [IsActive]        BIT            NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- ============================================================
-- Stored Procedure: sp_CreateProduct
-- ============================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateProduct]') AND type in (N'P'))
    DROP PROCEDURE [dbo].[sp_CreateProduct];
GO

CREATE PROCEDURE [dbo].[sp_CreateProduct]
    @Name           NVARCHAR(200),
    @Description    NVARCHAR(1000),
    @Price          DECIMAL(18,2),
    @UnitsInStock   INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Products] ([Name], [Description], [Price], [UnitsInStock], [CreatedDate], [IsActive])
    VALUES (@Name, @Description, @Price, @UnitsInStock, GETUTCDATE(), 1);

    SELECT 
        [Id],
        [Name],
        [Description],
        [Price],
        [UnitsInStock],
        [CreatedDate],
        [ModifiedDate],
        [IsActive]
    FROM [dbo].[Products]
    WHERE [Id] = SCOPE_IDENTITY();
END
GO

-- ============================================================
-- Stored Procedure: sp_GetAllProducts
-- ============================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetAllProducts]') AND type in (N'P'))
    DROP PROCEDURE [dbo].[sp_GetAllProducts];
GO

CREATE PROCEDURE [dbo].[sp_GetAllProducts]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [Id],
        [Name],
        [Description],
        [Price],
        [UnitsInStock],
        [CreatedDate],
        [ModifiedDate],
        [IsActive]
    FROM [dbo].[Products]
    ORDER BY [CreatedDate] DESC;
END
GO

-- ============================================================
-- Stored Procedure: sp_GetProductById
-- ============================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_GetProductById]') AND type in (N'P'))
    DROP PROCEDURE [dbo].[sp_GetProductById];
GO

CREATE PROCEDURE [dbo].[sp_GetProductById]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [Id],
        [Name],
        [Description],
        [Price],
        [UnitsInStock],
        [CreatedDate],
        [ModifiedDate],
        [IsActive]
    FROM [dbo].[Products]
    WHERE [Id] = @Id;
END
GO

-- ============================================================
-- Stored Procedure: sp_UpdateProduct
-- ============================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_UpdateProduct]') AND type in (N'P'))
    DROP PROCEDURE [dbo].[sp_UpdateProduct];
GO

CREATE PROCEDURE [dbo].[sp_UpdateProduct]
    @Id              INT,
    @Name            NVARCHAR(200),
    @Description     NVARCHAR(1000),
    @Price           DECIMAL(18,2),
    @UnitsInStock    INT,
    @IsActive        BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Products]
    SET 
        [Name]            = @Name,
        [Description]     = @Description,
        [Price]           = @Price,
        [UnitsInStock]    = @UnitsInStock,
        [IsActive]        = @IsActive,
        [ModifiedDate]    = GETUTCDATE()
    WHERE [Id] = @Id;

    SELECT 
        [Id],
        [Name],
        [Description],
        [Price],
        [UnitsInStock],
        [CreatedDate],
        [ModifiedDate],
        [IsActive]
    FROM [dbo].[Products]
    WHERE [Id] = @Id;
END
GO

-- ============================================================
-- Stored Procedure: sp_DeleteProduct
-- ============================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteProduct]') AND type in (N'P'))
    DROP PROCEDURE [dbo].[sp_DeleteProduct];
GO

CREATE PROCEDURE [dbo].[sp_DeleteProduct]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[Products]
    WHERE [Id] = @Id;

    SELECT @@ROWCOUNT AS [RowsAffected];
END
GO

-- ============================================================
-- Seed Data (Optional)
-- ============================================================
IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Products])
BEGIN
    INSERT INTO [dbo].[Products] ([Name], [Description], [Price], [UnitsInStock], [CreatedDate], [IsActive])
    VALUES 
        (N'Wireless Keyboard', N'Ergonomic wireless keyboard with Bluetooth connectivity', 49.99, 150, GETUTCDATE(), 1),
        (N'USB-C Hub', N'7-in-1 USB-C hub with HDMI, USB 3.0, and SD card reader', 34.50, 200, GETUTCDATE(), 1),
        (N'Noise Cancelling Headphones', N'Over-ear headphones with active noise cancellation and 30h battery', 129.99, 75, GETUTCDATE(), 1),
        (N'Portable SSD 1TB', N'High-speed portable solid state drive with USB 3.2 Gen 2', 89.00, 120, GETUTCDATE(), 1),
        (N'Webcam HD 1080p', N'Full HD webcam with built-in microphone and auto-focus', 59.99, 90, GETUTCDATE(), 1);
END
GO

PRINT 'Database setup completed successfully.';
GO
