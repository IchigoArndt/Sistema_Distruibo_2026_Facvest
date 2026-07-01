FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SD_SharedKernel/SD_SharedKernel.csproj SD_SharedKernel/
COPY SD_Api_Base/SD_Api_Base.csproj SD_Api_Base/
COPY SD_Api_Extensions/SD_Api_Extensions.csproj SD_Api_Extensions/
COPY SD_Server.Domain/SD_Server.Domain.csproj SD_Server.Domain/
COPY SD_Server.Application/SD_Server.Application.csproj SD_Server.Application/
COPY SD_Server.Infra/SD_Server.Infra.csproj SD_Server.Infra/
COPY SD_Server.Infra.Data/SD_Server.Infra.Data.csproj SD_Server.Infra.Data/
COPY SD_Server/SD_Server.Api.csproj SD_Server/

RUN dotnet restore SD_Server/SD_Server.Api.csproj

COPY SD_SharedKernel/ SD_SharedKernel/
COPY SD_Api_Base/ SD_Api_Base/
COPY SD_Api_Extensions/ SD_Api_Extensions/
COPY SD_Server.Domain/ SD_Server.Domain/
COPY SD_Server.Application/ SD_Server.Application/
COPY SD_Server.Infra/ SD_Server.Infra/
COPY SD_Server.Infra.Data/ SD_Server.Infra.Data/
COPY SD_Server/ SD_Server/

WORKDIR /src/SD_Server
RUN dotnet publish SD_Server.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5252
EXPOSE 5252

ENTRYPOINT ["dotnet", "SD_Server.Api.dll"]
