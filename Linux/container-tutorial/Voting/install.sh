#!/bin/bash

if [ -z "$1" ]; then
	echo "Missing http-schema cluster endpoint connection string."
	echo "If running locally, will be http://localhost:19080."
	exit 1
fi

sfctl cluster select --endpoint $1
sfctl application upload --path Voting --show-progress
sfctl application provision --application-type-build-path Voting
sfctl application create --app-name fabric:/Voting --app-type VotingType --app-version 1.0.0
