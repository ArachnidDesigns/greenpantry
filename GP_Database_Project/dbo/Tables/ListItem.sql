CREATE TABLE [dbo].[ListItem] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [ListID]    INT NOT NULL,
    [ProductID] INT NOT NULL,
    [Quantity ] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ListID]) REFERENCES [dbo].[ShoppingList] ([ID]),
    FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);

