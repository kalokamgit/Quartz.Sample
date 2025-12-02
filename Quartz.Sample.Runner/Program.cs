using Microsoft.Extensions.Hosting;
using Quartz;

using Quartz.Sample.Jobs;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddQuartz(builder.Configuration, executeJobs: true);

var app = builder.Build();

await app.RunAsync();