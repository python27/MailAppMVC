CREATE TABLE [dbo].[Mail] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (50)  NULL,
    [LastName]  VARCHAR (50)  NULL,
    [Email]     VARCHAR (100) NULL,
    [Subject]   VARCHAR (100) NULL,
    [Message]   VARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);