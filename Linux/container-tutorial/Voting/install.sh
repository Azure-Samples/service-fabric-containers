#!/bin/bash
set -e

export filePath=Voting
export appName=Voting

echo Uploading Application Files
sfctl application upload --path ${filePath} --show-progress

echo Provisioning Application Type 
sfctl application provision --application-type-build-path ${filePath}

echo Creating Application
sfctl application create --app-name fabric:/${appName} --app-type ${appName}Type --app-version 1.0.0
