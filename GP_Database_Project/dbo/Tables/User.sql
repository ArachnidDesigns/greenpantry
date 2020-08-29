CREATE TABLE [dbo].[User] (
    [ID]             INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (100) NOT NULL,
    [Surname]        VARCHAR (100) NOT NULL,
    [Email]          VARCHAR (100) NOT NULL,
    [Password]       VARCHAR (MAX) NOT NULL,
    [PhoneNumber]    CHAR (10)     NULL,
    [Status]         VARCHAR (MAX) NOT NULL,
    [DateRegistered] DATE          NOT NULL,
    [UserType]       VARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

