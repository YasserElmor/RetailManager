CREATE TABLE [dbo].[User]
(   /* Cashier */
    /* Identity Authentication User ID ForeignKey (GUID) used as a primary key here since it's a one-to-one relationship between a Cashier and their Auth Account */
	Id NVARCHAR(128) PRIMARY KEY NOT NULL, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(256) NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(),
)
