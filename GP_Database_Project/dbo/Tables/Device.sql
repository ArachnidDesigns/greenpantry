CREATE TABLE [dbo].[Device] (
    [DeviceID]   INT          IDENTITY (1, 1) NOT NULL,
    [OS]         VARCHAR (50) NOT NULL,
    [CustomerID] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([DeviceID] ASC),
    FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[User] ([ID])
);

