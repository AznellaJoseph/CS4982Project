DROP PROCEDURE IF EXISTS uspGetUserByUsername;

DELIMITER $

CREATE PROCEDURE uspGetUserByUsername(
        username VARCHAR(45)
)
BEGIN
        SELECT userId, fname, lname, password
        FROM user
        WHERE user.username = username;
END$

DELIMITER ;
