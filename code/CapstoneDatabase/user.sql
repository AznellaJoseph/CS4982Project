CREATE TABLE [dbo].[user]
(
	[id] INT NOT NULL PRIMARY KEY, 
    [username] VARCHAR(45) NOT NULL UNIQUE, 
    [password] VARCHAR(45) NOT NULL, 
    [fname] VARCHAR(45) NOT NULL, 
    [lname] VARCHAR(45) NOT NULL 
)
