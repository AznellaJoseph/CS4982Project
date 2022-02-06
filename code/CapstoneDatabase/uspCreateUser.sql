DROP PROCEDURE IF EXISTS uspCreateUser;

DELIMITER $

CREATE PROCEDURE uspCreateUser(
        username VARCHAR(45),
        password VARCHAR(45),
        fname VARCHAR(45),
        lname VARCHAR(45),
        OUT userId int
)
BEGIN
        INSERT INTO user(username, password, fname, lname)
        VALUES (username, password, fname, lname);

        SELECT id
        INTO userId 
        FROM user
        WHERE user.username = username;
END$

