![waypointz logo](https://user-images.githubusercontent.com/59812915/152658685-106725b3-42c9-4acb-a9bc-3a78a6e187a9.png)
# Waypointz by The Left-Handers
## Purpose
## Installing and Running Waypointz
### Requirments
To run Waypointz, you must have the following
- Windows/Linux
- .NET
- Docker
### How to Use Capstone Script
Now that you have met the requirements, you can run the capstone.sh file in the code folder.

#### Build
To build the entire solution run:
```
./capstone.sh build
```
To build a specific project, you can pass the project name as follows:
```
./capstone.sh build ProjectName
```

#### Run
To run a project, you can pass the project name as follows:
```
./capstone.sh run ProjectName
```
Most of these projects require the server to be running to work.

#### Server
To start the server, make sure that you have docker setup; for more information on docker, click this [link](https://www.docker.com/get-started). Once that is set up, you need to set up the SQL files to be updated and ready to be loaded into the server. To do this, you must run:
```
./capstone.sh server makemigrations
```

Any time you update or add a SQL file, you need to call this command. The file execution_order.config in the code/CapstoneDatabase is used to describe the execution order. When adding a new SQL file, add it to the list-making sure there is a new line at the end of the file. 

After all this, you start the server by calling:
```
./capstone.sh server run
```

To stop the server, you can run:
```
./capstone.sh server stop
```

#### Test
To test the solution, you can run:
```
./capstone.sh test
```

#### Cover
To generate a dotCover coverage report, you can run:
```
./capstone.sh cover [sprintNumber]
```
The report will be saved in the sprints/sprint<_sprintNumber_> folder.

### Inspect
To generate a ReSharper inspect report, you can run:
```
./capstone.sh inspect [sprintNumber]
```
The report will be saved in the sprints/sprint<_sprintNumber_> folder.

## Vendors

To find points of interest and location, we are using the Foursquare Places API. To learn more about the API, click on this [link](https://foursquare.com/)

## Who are The Left-Handers

### Origin
The Left-Handers were formed when we all realized we were left-handed, hence the name.

### Developers
- Aznella
- Darrell
- Alex
