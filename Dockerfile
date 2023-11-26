# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ExamScheduleSystem/*.csproj ./ExamScheduleSystem/
RUN dotnet restore

# copy everything else and build app
COPY ExamScheduleSystem/. ./ExamScheduleSystem/
WORKDIR /source/ExamScheduleSystem
RUN dotnet publish --no-restore -c Release -o /app --no-cache /restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ExamScheduleSystem.dll", "urls=http://0.0.0.0:5000"]