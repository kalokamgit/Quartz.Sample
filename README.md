# Quartz.Sample

- A compact sample demonstrating scheduling with Quartz.NET for .NET 10 from one appliaction (Quartz.Sample.Api) and run those jobs from another application (Quartz.Sample.Runner).
- This repository includes an API, job implementations, a host, and a runner to exercise scheduled jobs.

with

## Projects

- `Quartz.Sample.Api` — REST API that exposes sample scheduling endpoints.
- `Quartz.Sample.Jobs` — It's a class library, contains common jobs and job-related extensions. This will register the jobs, it will not execute the jobs. refer form both (Quarz.Sample.Api and Quartz.Sample.Runner).
- `Quartz.Sample.AppHost` — Aspire Host that runs the scheduler api & jobs runner and connects to same backing sql storage.
- `Quartz.Sample.Runner` — Console runner application to just execute the scheduled triggered jobs locally.
- `Quartz.Sample.ServiceDefaults` — Shared Aspire project configuration and helpers with traces & Open telemetry.

## Requirements

- .NET 10 SDK
- Aspire 13+
- Quartz.NET 3.15+
-  SQL Server for persistent job store — see `Quartz.Sample.AppHost/scripts/QuartzDbInit.sql`.

## Quick start

1. Clone the repository.
2. Initialize the database if you plan to use the persistent store:
3. Build and run the API project

## Configuration notes

- See `Quartz.Sample.AppHost/appsettings.json` and `Quartz.Sample.Api/appsettings.json` for sample configuration.
- See the SQL script at `Quartz.Sample.AppHost/scripts/QuartzDbInit.sql` to create the persistent job store schema when using a database-backed store.
- Adjust logging levels in `appsettings.json` as needed. Script will excute automatically on startup if database connection is configured if quartz tables not exists.

## Sequence Diagram
```mermaid
sequenceDiagram
    HTTP->>API (Application): POST /scheduler/jobs (create)
    API (Application)->>Host: Request scheduling (trigger)
    Host->>DB: Persist trigger (if persistent store configured)
    Host-->>API (Application): Scheduling confirmation / status
    API (Application)-->>HTTP: status (success / failure)
    DB->>Runner: Instantiate and run job when trigger fires
    Runner-->>DB: Job completed (result/logs)
