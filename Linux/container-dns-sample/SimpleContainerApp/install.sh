#!/bin/bash

sfctl application upload --path SimpleContainerApp --show-progress
sfctl application provision --application-type-build-path SimpleContainerApp
sfctl application create --app-name fabric:/SimpleContainerApp --app-type SimpleContainerAppType --app-version 1.0.0
