DROP TABLE IF EXISTS waypoint;

CREATE TABLE waypoint
(
    tripId INT UNSIGNED NOT NULL,
    waypointNum INT UNSIGNED NOT NULL,
    location VARCHAR(75) NOT NULL,
    startDate DATE NOT NULL,
    endDate DATE NOT NULL,
    PRIMARY KEY (tripId, waypointNum),
    CONSTRAINT fk_waypoint_tripId
        FOREIGN KEY (tripId)
            REFERENCES trip(id)
            ON UPDATE CASCADE
            ON DELETE CASCADE
);