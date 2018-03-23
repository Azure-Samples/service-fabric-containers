#!/bin/bash
sfctl application upload --path Voting --show-progress
sfctl application provision --application-type-build-path Voting
sfctl application create --app-name fabric:/Voting --app-type VotingType --app-version 1.0.0
