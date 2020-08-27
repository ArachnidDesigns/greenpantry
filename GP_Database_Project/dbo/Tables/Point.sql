CREATE TABLE [dbo].[Point] (
    [Point_ID]    INT NOT NULL,
    [Customer_ID] INT NOT NULL,
    [Points]      INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Point_ID] ASC),
    FOREIGN KEY ([Customer_ID]) REFERENCES [dbo].[User] ([ID])
);

