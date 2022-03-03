#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ToDo-Mvc-App.csproj", "ToDo-Mvc-App/"]
RUN dotnet restore "ToDo-Mvc-App/ToDo-Mvc-App.csproj"
COPY . .
WORKDIR "/src/ToDo-Mvc-App"
RUN dotnet build "ToDo-Mvc-App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDo-Mvc-App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDo-Mvc-App.dll"]
