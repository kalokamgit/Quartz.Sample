# Quartz.Sample

- A compact sample demonstrating scheduling with Quartz.NET for .NET 10 from one appliaction (Quartz.Sample.Api) and run those jobs from another application (Quartz.Sample.Runner).
- This repository includes an API, job implementations, a host, and a runner to exercise scheduled jobs.

## Architecture Diagram (SVG)

<?xml version="1.0" encoding="UTF-8"?>
<svg width="1100" height="420" viewBox="0 0 1100 420" xmlns="http://www.w3.org/2000/svg" role="img" aria-labelledby="title desc">
  <title id="title">Quartz.NET Architecture - API, Job Store, Worker</title>
  <desc id="desc">
    Quartz.Sample.Api creates triggers that are stored in SQL Server (Quartz DB), and Quartz.Sample.Runner executes jobs.
  </desc>
  <defs>
    <marker id="arrow" markerWidth="8" markerHeight="8" refX="9" refY="5" orient="auto">
      <path d="M1,1 L8,5 L1,8 Z" fill="#1e3a8a"/>
    </marker>
    <style>
      .box { fill:#ffffff; stroke:#1e3a8a; stroke-width:2.2; rx:10; ry:10; }
      .box-muted { fill:#f0f4ff; stroke:#93c5fd; }
      .title { font: 700 16px &quot;Segoe UI&quot;, Roboto, Arial; fill:#1e3a8a; }
      .subtitle { font: 500 13px &quot;Segoe UI&quot;, Roboto, Arial; fill:#334155; }
      .note { font: 12px &quot;Segoe UI&quot;, Roboto, Arial; fill:#475569; }
      .connector { stroke:#1e3a8a; stroke-width:2; fill:none; marker-end:url(#arrow); }
    </style>
  </defs>
  <g transform="translate(60,50)">
    <rect class="box" width="300" height="120"/>
    <text class="title" x="150" y="32" text-anchor="middle">Quartz.Sample.Api</text>
    <text class="subtitle" x="150" y="58" text-anchor="middle">Creates Triggers via API</text>
    <text class="note" x="150" y="83" text-anchor="middle">Registers jobs on application startup</text>
  </g>
  <g transform="translate(480,20)">
    <rect class="box-muted box" width="320" height="160"/>
    <text class="title" x="160" y="38" text-anchor="middle">Quartz DB</text>
    <text class="subtitle" x="160" y="64" text-anchor="middle">SQL Server (AdoJobStore)</text>
    <text class="note" x="160" y="90" text-anchor="middle">Stores all Jobs &amp; Triggers</text>
    <text class="note" x="160" y="115" text-anchor="middle">Shared by API and Worker</text>
  </g>
  <g transform="translate(410,240)">
    <rect class="box" width="380" height="120"/>
    <text class="title" x="190" y="32" text-anchor="middle">Quartz.Sample.Runner</text>
    <text class="subtitle" x="190" y="58" text-anchor="middle">Executor / Quartz Worker</text>
    <text class="note" x="190" y="83" text-anchor="middle">Polls DB &amp; Executes Jobs</text>
  </g>
  <path class="connector" d="M360 110 L480 110"/>
  <text class="note" x="420" y="90" text-anchor="middle">Creates Triggers</text>
  <path class="connector" d="M640 180 L640 240"/>
  <text class="note" x="720" y="220" text-anchor="start">Uses jobs &amp; triggers</text>
  <rect x="10" y="10" width="1080" height="400" rx="15" ry="15" fill="none" stroke="#cbd5e1" stroke-width="2"/>
</svg>


apply better colors and styles as needed with proper gapping without over


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
