FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY DotVVM.Samples.BasicSamples.AspNetCore/DotVVM.Samples.BasicSamples.AspNetCore.csproj DotVVM.Samples.BasicSamples.AspNetCore/
COPY DotVVM.Samples.Common/DotVVM.Samples.Common.csproj DotVVM.Samples.Common/
COPY DotVVM.Framework/DotVVM.Framework.csproj DotVVM.Framework/
COPY DotVVM.Core/DotVVM.Core.csproj DotVVM.Core/
COPY EmbeddedResourceControls/EmbeddedResourceControls.csproj EmbeddedResourceControls/
COPY DotVVM.Framework.Hosting.AspNetCore/DotVVM.Framework.Hosting.AspNetCore.csproj DotVVM.Framework.Hosting.AspNetCore/
RUN dotnet restore
COPY . .
WORKDIR /src/DotVVM.Samples.BasicSamples.AspNetCore
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY DotVVM.Samples.Common/Views/ /DotVVM.Samples.Common/Views/
ENTRYPOINT ["dotnet", "DotVVM.Samples.BasicSamples.AspNetCore.dll"]
