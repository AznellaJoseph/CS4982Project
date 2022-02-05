#!/bin/bash

SCRIPT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" &> /dev/null && (pwd -W 2> /dev/null || pwd))

if [[ $1 == "build" ]]
then
  if [[ $2 != "" ]]
  then
    dotnet build $2
  else
    dotnet build
  fi
elif [[ $1 == "server" ]]
then
  if [[ $2 == "run" ]]
  then
    docker kill capstone;
    docker run --rm --name capstone -e MYSQL_ROOT_PASSWORD="test" -e MYSQL_DATABASE="capstone" -v $SCRIPT_DIR'/CapstoneDatabase:/docker-entrypoint-initdb.d' -p 3308:3306 -d mysql;
  elif [[ $2 == "stop" ]]
  then
    docker kill capstone;
  else
    echo "Not Valid Command. capstone server [ run | stop ]."
  fi
elif [[ $1 == "test" ]]
then
  dotnet test
elif [[ $1 == "cover" ]]
then
  if [[ $2 =~ ^[0-9]$ ]]
  then
    # Install dotCover
    dotnet new tool-manifest
    dotnet tool install JetBrains.dotCover.GlobalTool
    dotnet tool restore

    # Builds Tests
    dotnet build CapstoneTest

    if [ ! -d "../sprints/sprint$2" ]
    then
      mkdir "../sprints/sprint$2"
    fi
    # Run Tests with dotcover
    dotnet dotnet-dotcover test --no-build --dcOutput="../sprints/sprint$2/capstone_cover_report.html" --dcReportType=HTML
  else
    echo "You must input the sprint number. capstone cover [sprintNumber]"
  fi
elif [[ $1 == "inspect" ]]
then
  if [[ $2 =~ ^[0-9]$ ]]
  then
    # Install ReSharper
    dotnet new tool-manifest
    dotnet tool install JetBrains.ReSharper.GlobalTools
    dotnet tool restore

    if [ ! -d "../sprints/sprint$2" ]
    then
      mkdir "../sprints/sprint$2"
    fi
    # Inspect Code
    dotnet jb inspectcode CapstoneProject.sln -o="../sprints/sprint$2/capstone_inspect_repost.html" --profile="ReSharperUWGSettings.DotSettings"
  else
    echo "You must input the sprint number. capstone cover [sprintNumber]"
  fi
else
  echo "Not Valid Command. capstone [ build | server | test | cover | inspect ]."
fi
