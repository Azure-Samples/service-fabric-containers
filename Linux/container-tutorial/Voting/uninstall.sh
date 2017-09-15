#!/bin/bash

sfctl application delete --application-id TestContainer
sfctl application unprovision --application-type-name TestContainerType --application-type-version 1.0.0
sfctl store delete --content-path TestContainer
