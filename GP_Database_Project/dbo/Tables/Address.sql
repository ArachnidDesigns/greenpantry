CREATE TABLE [dbo].[Address] (
    [ID]         INT          IDENTITY (1, 1) NOT NULL,
    [Line1]      VARCHAR (50) NOT NULL,
    [Line2]      VARCHAR (50) NOT NULL,
    [Suburb]     VARCHAR (50) NOT NULL,
    [City]       VARCHAR (50) NOT NULL,
    [Billing]    CHAR (1)     NOT NULL,
    [Type]       VARCHAR (25) NOT NULL,
    [CustomerID] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[User] ([ID])
);

