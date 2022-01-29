#!/bin/bash

docker kill capstone;

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" &> /dev/null && (pwd -W 2> /dev/null || pwd))

docker run --rm --name capstone -e MYSQL_ROOT_PASSWORD="test" -e MYSQL_DATABASE="capstone" -v $SCRIPT_DIR'/CapstoneDatabase:/docker-entrypoint-initdb.d' -p 3308:3306 -d mysql;

