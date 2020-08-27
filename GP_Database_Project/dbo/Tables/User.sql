CREATE TABLE [dbo].[User] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (100) NOT NULL,
    [Surname]        VARCHAR (100) NOT NULL,
    [Email]          VARCHAR (100) NOT NULL,
    [Password]       VARCHAR (MAX) NOT NULL,
    [PhoneNumber]    CHAR (10)     NOT NULL,
    [Status]         VARCHAR (MAX) NULL,
    [DateRegistered] DATE          NULL,
    [UserType]       VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

