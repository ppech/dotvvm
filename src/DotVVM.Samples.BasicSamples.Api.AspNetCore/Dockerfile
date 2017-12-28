FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5001

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY DotVVM.Samples.BasicSamples.Api.AspNetCore/DotVVM.Samples.BasicSamples.Api.AspNetCore.csproj DotVVM.Samples.BasicSamples.Api.AspNetCore/
COPY DotVVM.Framework/DotVVM.Framework.csproj DotVVM.Framework/
COPY DotVVM.Core/DotVVM.Core.csproj DotVVM.Core/
COPY DotVVM.Samples.BasicSamples.Api.Common/DotVVM.Samples.BasicSamples.Api.Common.csproj DotVVM.Samples.BasicSamples.Api.Common/
COPY DotVVM.Framework.Hosting.AspNetCore/DotVVM.Framework.Hosting.AspNetCore.csproj DotVVM.Framework.Hosting.AspNetCore/
COPY DotVVM.Framework.Api.Swashbuckle.AspNetCore/DotVVM.Framework.Api.Swashbuckle.AspNetCore.csproj DotVVM.Framework.Api.Swashbuckle.AspNetCore/
RUN dotnet restore
COPY . .
WORKDIR /src/DotVVM.Samples.BasicSamples.Api.AspNetCore
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DotVVM.Samples.BasicSamples.Api.AspNetCore.dll"]
