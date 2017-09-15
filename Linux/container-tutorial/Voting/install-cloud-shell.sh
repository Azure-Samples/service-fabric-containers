#!/bin/bash

az sf application upload --path Voting --show-progress
az sf application provision --application-type-build-path Voting
az sf application create --app-name fabric:/Voting --app-type VotingType --app-version 1.0.0
