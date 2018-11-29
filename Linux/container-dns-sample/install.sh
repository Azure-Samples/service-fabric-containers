#!/bin/bash

create_app()
{
  sfctl application create --app-name fabric:/ContainerApplication --app-type SimpleContainerAppType --app-version 1.0.0
  sfctl service create --name fabric:/ContainerApplication/nodejsFrontEnd --service-type nodejsfrontendType --stateless --instance-count $1 --app-id ContainerApplication  --singleton-scheme
  sfctl service create --name fabric:/ContainerApplication/pythonBackEnd --service-type pythonbackendType --stateless --instance-count $2 --app-id ContainerApplication  --dns-name pythonbackend.simplecontainerapp --singleton-scheme
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
    create_app 1 1
elif [ $1 = "-onebox" ]
  then
   echo "Onebox environment, proceed with default instanceCount of 1 for both front and back ends."
   create_app 1 1
elif [ $1 = "-multinode" ]
 then
   echo "MultiNode environment proceed with instanceCount of -1 for front end and 3 instance count for back end"
   create_app -1 3

fi
