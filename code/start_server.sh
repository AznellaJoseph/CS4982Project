#!/bin/bash

docker kill capstone;

docker run --rm --name capstone -e MYSQL_ROOT_PASSWORD="test" -e MYSQL_DATABASE="capstone" -v "$PWD"/CapstoneDatabase:/docker-entrypoint-initdb.d -p 3308:3306 -d mysql;

