CREATE TABLE [dbo].[ShoppingList] (
    [ID]         INT IDENTITY (1, 1) NOT NULL,
    [CustomerID] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[User] ([ID])
);

