CREATE TABLE [dbo].[tblProducts] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [Brand]    VARCHAR (50) NULL,
    [Price]    DECIMAL(10, 10)          NULL,
    [Quantity] INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

