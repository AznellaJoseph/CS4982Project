DROP TABLE IF EXISTS waypoint;

CREATE TABLE waypoint
(
    waypointId INT UNSIGNED NOT NULL AUTO_INCREMENT,
    tripId INT UNSIGNED NOT NULL, 
    location VARCHAR(75) NOT NULL,
    startDate DATETIME NOT NULL,
    endDate DATETIME,
    PRIMARY KEY (waypointId),
    CONSTRAINT fk_waypoint_tripId
        FOREIGN KEY (tripId)
            REFERENCES trip(tripId)
            ON UPDATE CASCADE
            ON DELETE CASCADE
);
