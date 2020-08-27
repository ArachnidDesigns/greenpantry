CREATE TABLE [dbo].[SubCategory] (
    [SubID]      INT          IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [CategoryID] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([SubID] ASC),
    FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[ProductCategory] ([ID])
);

