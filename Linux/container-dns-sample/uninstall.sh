#!/bin/bash

sfctl application delete --application-id ContainerApplication
sfctl application unprovision --application-type-name SimpleContainerAppType --application-type-version 1.0.0
sfctl store delete --content-path SimpleContainerApp
