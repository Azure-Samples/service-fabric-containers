#!/bin/bash
sfctl application delete --application-id Voting
sfctl application unprovision --application-type-name VotingType --application-type-version 1.0.0
sfctl store delete --content-path Voting
