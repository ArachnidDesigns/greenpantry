CREATE TABLE [dbo].[Product] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [SubCategoryID]  INT           NOT NULL,
    [Price]          MONEY         NOT NULL,
    [Cost]           MONEY         NOT NULL,
    [StockOnHand]    INT           NOT NULL,
    [Image_Location] VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([SubCategoryID]) REFERENCES [dbo].[SubCategory] ([SubID])
);

