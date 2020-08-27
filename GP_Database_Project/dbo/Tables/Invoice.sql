CREATE TABLE [dbo].[Invoice] (
    [ID]               INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NOT NULL,
    [Status]           VARCHAR (9)   NOT NULL,
    [Date]             DATE          NOT NULL,
    [DeliveryDatetime] DATETIME      NOT NULL,
    [Notes]            VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[User] ([ID])
);

