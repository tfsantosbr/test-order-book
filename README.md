# Companies API

## Patterns Used In This Application

- [Domain Driven Design](https://balta.io/cursos/modelando-dominios-ricos)
- [Feature Folder](https://github.com/tfsantosbr/dotnet-folder-by-feature-structure)
- [CQRS](https://balta.io/blog/aspnet-core-cqrs-mediator)
- [Repository](https://learning.eximia.co/videos/repositorios/)
- [Notification](https://balta.io/blog/exception-vs-domain-notification)

## Running With Docker-Compose

You will need [Docker Desktop](https://docs.docker.com/desktop/install/windows-install/) to run this commands

```bash
docker-compose up -d --build
```

IMPORTANT: To see only the **logs for trade statistics**, use the following command:

```bash
docker-compose up -d --build
docker-compose up worker
```

## Running API Locally

You will need [.NET CLI](https://dotnet.microsoft.com/en-us/download) to run this commands

```bash
dotnet restore
dotnet build
dotnet run --project src/OrderBook.WebApi
dotnet run --project src/OrderBook.Worker
```

## Running Unit Tests

You will need [.NET CLI](https://dotnet.microsoft.com/en-us/download) to run this commands

```bash
dotnet test
```

## Smoke Tests

The smoke test can be found in [OrderBook.WebApi.http](./src/OrderBook.WebApi/OrderBook.WebApi.http) file.
