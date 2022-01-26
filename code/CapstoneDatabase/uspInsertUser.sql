CREATE PROCEDURE [dbo].[uspInsertUser]
	@username VARCHAR(45),
	@password VARCHAR(45),
	@fname VARCHAR(45),
	@lname VARCHAR(45)

AS
	INSERT INTO [dbo].[user](username, password, fname, lname)
	VALUES (@username, @password, @fname, @lname)
RETURN 0
