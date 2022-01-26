CREATE PROCEDURE [dbo].[uspGetUser]
	@username VARCHAR(45),
	@password VARCHAR(45)

AS
	SELECT id, fname, lname
	FROM [dbo].[user]
	WHERE @username = username AND @password = password;

RETURN 0