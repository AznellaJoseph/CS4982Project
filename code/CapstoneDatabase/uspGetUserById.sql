DROP PROCEDURE IF EXISTS uspGetUserById;

DELIMITER $

CREATE PROCEDURE uspGetUserById(userId INT)
BEGIN
	SELECT user.username, user.password, user.fname, user.lname
	FROM user
	WHERE user.userId = userId;
END$

DELIMITER ;
