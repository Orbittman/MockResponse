#!/bin/bash

echo "Starting"

dotnet restore mockresponse.sln
dotnet publish ./MockResponse.Api/MockResponse.Api.csproj -c c-configuration -o ./obj/Docker/publish
dotnet publish ./MockResponse.Web/MockResponse.Web.csproj -c c-configuration -o ./obj/Docker/publish

docker login -u "orbittman" -p "larda55";
docker build -t orbittman/mockresponse.api ./MockResponse.Api;
docker build -t orbittman/mockresponse.web ./MockResponse.Web;
docker push orbittman/mockresponse.api;
docker push orbittman/mockresponse.web;