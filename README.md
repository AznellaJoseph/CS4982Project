![waypointz logo](https://user-images.githubusercontent.com/59812915/152658685-106725b3-42c9-4acb-a9bc-3a78a6e187a9.png)
# Waypointz by The Left Handers
## Purpose
## Installing and Running Waypointz
### Requirments
To run Waypointz you must have the following
- Windows/Linux
- .NET
- Docker
### How to Use Capstone Script
Now that you have meet the requirements you are able to run the capstone.sh file in the code folder.

#### Build
To build the entire solution run:
```
./capstone.sh build
```
To build a specific project you can pass the project name as follows:
```
./capstone.sh build ProjectName
```

#### Run
To run a project you can pass the project name as follows:
```
./capstone.sh run ProjectName
```
Most of these project require the server to be running to work.

#### Server
To start the server make sure that you have docker setup; for more information on docker click this [link](https://www.docker.com/get-started). Once that is setup you can run the following
```
./capstone.sh server run
```
To stop the server you can run:
```
./capstone.sh server stop
```

#### Test
To test the solution you can run:
```
./capstone.sh test
```

#### Cover
To generate a dotCover coverage report you can run:
```
./capstone.sh cover [sprintNumber]
```
The report will be saved in the sprints/sprint<_sprintNumber_> folder.

### Inspect
To generate a ReSharper inspect report you can run
```
./capstone.sh inspect [sprintNumber]
```
The report will be saved in the sprints/sprint<_sprintNumber_> folder.

## Vendors

To find points of interest and location we are using the Foursquare Places API. To learn more about the API click on this [link](https://foursquare.com/)

## Who are The Left Handers

### Origin
The Left Handers where form when we all relize the we were left handed, so hence the name.

### Developers
- Aznella
- Darrell
- Alex
