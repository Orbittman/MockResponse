FROM microsoft/aspnetcore-build:2.0 AS build-env
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "MockResponse.Api.dll"]
