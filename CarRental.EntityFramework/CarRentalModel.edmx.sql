
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/19/2016 21:48:46
-- Generated from EDMX file: c:\users\ivan grgurina\documents\visual studio 2013\Projects\CarRentalWebSite\CarRental.EntityFramework\CarRentalModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CarRentalDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CarCarDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarDetailSet] DROP CONSTRAINT [FK_CarCarDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_CarPrice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarSet] DROP CONSTRAINT [FK_CarPrice];
GO
IF OBJECT_ID(N'[dbo].[FK_OfficeCar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CarSet] DROP CONSTRAINT [FK_OfficeCar];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReservationSet] DROP CONSTRAINT [FK_ClientReservation];
GO
IF OBJECT_ID(N'[dbo].[FK_CarReservation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReservationSet] DROP CONSTRAINT [FK_CarReservation];
GO
IF OBJECT_ID(N'[dbo].[FK_CarReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReviewSet] DROP CONSTRAINT [FK_CarReview];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CarSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarSet];
GO
IF OBJECT_ID(N'[dbo].[CarDetailSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarDetailSet];
GO
IF OBJECT_ID(N'[dbo].[OfficeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfficeSet];
GO
IF OBJECT_ID(N'[dbo].[ClientSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientSet];
GO
IF OBJECT_ID(N'[dbo].[PriceSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PriceSet];
GO
IF OBJECT_ID(N'[dbo].[ReservationSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReservationSet];
GO
IF OBJECT_ID(N'[dbo].[ReviewSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReviewSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CarSet'
CREATE TABLE [dbo].[CarSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Manufacturer] nvarchar(max)  NOT NULL,
    [Model] nvarchar(max)  NOT NULL,
    [Price_Id] int  NOT NULL,
    [Office_Id] int  NOT NULL
);
GO

-- Creating table 'CarDetailSet'
CREATE TABLE [dbo].[CarDetailSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] int  NOT NULL,
    [Value] nvarchar(max)  NOT NULL,
    [Car_Id] int  NOT NULL
);
GO

-- Creating table 'OfficeSet'
CREATE TABLE [dbo].[OfficeSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [City] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ClientSet'
CREATE TABLE [dbo].[ClientSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PriceSet'
CREATE TABLE [dbo].[PriceSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Season] int  NOT NULL,
    [OffSeason] int  NOT NULL
);
GO

-- Creating table 'ReservationSet'
CREATE TABLE [dbo].[ReservationSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateStarted] nvarchar(max)  NOT NULL,
    [DateEnded] nvarchar(max)  NOT NULL,
    [Client_Id] int  NOT NULL,
    [Car_Id] int  NOT NULL
);
GO

-- Creating table 'ReviewSet'
CREATE TABLE [dbo].[ReviewSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Rating] int  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [Car_Id] int  NOT NULL,
    [Reservation_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CarSet'
ALTER TABLE [dbo].[CarSet]
ADD CONSTRAINT [PK_CarSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CarDetailSet'
ALTER TABLE [dbo].[CarDetailSet]
ADD CONSTRAINT [PK_CarDetailSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OfficeSet'
ALTER TABLE [dbo].[OfficeSet]
ADD CONSTRAINT [PK_OfficeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ClientSet'
ALTER TABLE [dbo].[ClientSet]
ADD CONSTRAINT [PK_ClientSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PriceSet'
ALTER TABLE [dbo].[PriceSet]
ADD CONSTRAINT [PK_PriceSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReservationSet'
ALTER TABLE [dbo].[ReservationSet]
ADD CONSTRAINT [PK_ReservationSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [PK_ReviewSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Car_Id] in table 'CarDetailSet'
ALTER TABLE [dbo].[CarDetailSet]
ADD CONSTRAINT [FK_CarCarDetail]
    FOREIGN KEY ([Car_Id])
    REFERENCES [dbo].[CarSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarCarDetail'
CREATE INDEX [IX_FK_CarCarDetail]
ON [dbo].[CarDetailSet]
    ([Car_Id]);
GO

-- Creating foreign key on [Price_Id] in table 'CarSet'
ALTER TABLE [dbo].[CarSet]
ADD CONSTRAINT [FK_CarPrice]
    FOREIGN KEY ([Price_Id])
    REFERENCES [dbo].[PriceSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarPrice'
CREATE INDEX [IX_FK_CarPrice]
ON [dbo].[CarSet]
    ([Price_Id]);
GO

-- Creating foreign key on [Office_Id] in table 'CarSet'
ALTER TABLE [dbo].[CarSet]
ADD CONSTRAINT [FK_OfficeCar]
    FOREIGN KEY ([Office_Id])
    REFERENCES [dbo].[OfficeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfficeCar'
CREATE INDEX [IX_FK_OfficeCar]
ON [dbo].[CarSet]
    ([Office_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'ReservationSet'
ALTER TABLE [dbo].[ReservationSet]
ADD CONSTRAINT [FK_ClientReservation]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[ClientSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientReservation'
CREATE INDEX [IX_FK_ClientReservation]
ON [dbo].[ReservationSet]
    ([Client_Id]);
GO

-- Creating foreign key on [Car_Id] in table 'ReservationSet'
ALTER TABLE [dbo].[ReservationSet]
ADD CONSTRAINT [FK_CarReservation]
    FOREIGN KEY ([Car_Id])
    REFERENCES [dbo].[CarSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarReservation'
CREATE INDEX [IX_FK_CarReservation]
ON [dbo].[ReservationSet]
    ([Car_Id]);
GO

-- Creating foreign key on [Car_Id] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [FK_CarReview]
    FOREIGN KEY ([Car_Id])
    REFERENCES [dbo].[CarSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarReview'
CREATE INDEX [IX_FK_CarReview]
ON [dbo].[ReviewSet]
    ([Car_Id]);
GO

-- Creating foreign key on [Reservation_Id] in table 'ReviewSet'
ALTER TABLE [dbo].[ReviewSet]
ADD CONSTRAINT [FK_ReservationReview]
    FOREIGN KEY ([Reservation_Id])
    REFERENCES [dbo].[ReservationSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReservationReview'
CREATE INDEX [IX_FK_ReservationReview]
ON [dbo].[ReviewSet]
    ([Reservation_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------