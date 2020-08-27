CREATE TABLE [dbo].[InvoiceLine] (
    [ID]        INT IDENTITY (1, 1) NOT NULL,
    [ProductID] INT NOT NULL,
    [InvoiceID] INT NOT NULL,
    [Qty]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([InvoiceID]) REFERENCES [dbo].[Invoice] ([ID]),
    FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ID])
);

