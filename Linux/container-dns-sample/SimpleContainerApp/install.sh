#!/bin/bash

create_app()
{
  sfctl application create --app-name fabric:/SimpleContainerApp --app-type SimpleContainerAppType --app-version 1.0.0 --parameters $1
}

print_help()

{
  echo "Additional Options"
  echo "-onebox (Default): If you are deploying application on one box cluster"
  echo "-multinode: If you are deploying application on a multi node cluster"

}


if [ "$1" = "--help" ]

  then

    print_help

    exit 0

fi

sfctl application upload --path SimpleContainerApp --show-progress
sfctl application provision --application-type-build-path SimpleContainerApp

if [ $# -eq 0 ]
  then
    echo "No arguments supplied, proceed with default instance counts"
    create_app {}
elif [ $1 = "-onebox" ]
  then
   echo "Onebox environment, proceed with default instanceCount of 1 for both front and back ends."
   create_app {} 
elif [ $1 = "-multinode" ]
 then
   echo "MultiNode environment proceed with instanceCount of -1 for front end and 3 instance count for back end"
   create_app "{\"pythonbackend_instancecount\":\"3\",\"nodejs_instancecount\":\"-1\"}"

fi
