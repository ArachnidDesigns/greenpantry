CREATE TABLE [dbo].[Card] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]  INT           NOT NULL,
    [Description] VARCHAR (MAX) NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Number]      CHAR (16)     NOT NULL,
    [Expiry]      DATE          NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[User] ([ID]),
    UNIQUE NONCLUSTERED ([Number] ASC)
);

