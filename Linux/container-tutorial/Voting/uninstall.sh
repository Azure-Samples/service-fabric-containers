#!/bin/bash

if [ -z "$1" ]; then
	echo "Missing http-schema cluster endpoint connection string."
	echo "If running locally, will be http://localhost:19080."
	exit 1
fi

sfctl cluster select --endpoint $1
sfctl application delete --application-id Voting
sfctl application unprovision --application-type-name VotingType --application-type-version 1.0.0
sfctl store delete --content-path Voting
