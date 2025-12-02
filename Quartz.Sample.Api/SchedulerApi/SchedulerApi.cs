using Microsoft.AspNetCore.Mvc;
using Quartz.Impl.Calendar;
using Quartz.Sample.Api.SchedulerApi;
using Quartz.Sample.Api.SchedulerApi.Requests;
using Quartz.Sample.Jobs;

namespace Quartz.Sample.Api.SchedulerApi
{
    public static class SchedulerApi
    {
        extension(IEndpointRouteBuilder app)
        {
            public void MapSchedulerApi()
            {
                app.MapPost("/api/scheduler/schedule", async([FromBody] SimpleJobCreationRequest request, [FromServices] ISchedulerFactory factory) =>
                {
                    var scheduler = await factory.GetScheduler();

                    var builder = TriggerBuilder.Create()
                    .ForJob(SimpleJob.Key)
                    .UsingJobData("message", request.Message)
                    .WithIdentity(request.TriggerName)
                        .StartNow()
                        .WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(request.IntervalSeconds)
                            .RepeatForever());

                    if(request.AvoidHoliday)
                    {
                        builder.ModifiedByCalendar(nameof(HolidayCalendar));
                        builder.ModifiedByCalendar(nameof(WeeklyCalendar));
                    }

                    var trigger = builder.Build();

                    var offset = await scheduler.ScheduleJob(trigger);

                    return Results.Ok(new { Status = $"{trigger.Key.Name} Scheduled with {request.IntervalSeconds} seconds" });
                })
                .WithName("Schedule with interval seconds")
                .WithSummary("Schedules a job with the specified interval in seconds. It will run in every inter")
                .WithTags("Scheduler");

                app.MapPost("/api/scheduler/run", async ([FromServices]  ISchedulerFactory factory) =>
                {
                    var scheduler = await factory.GetScheduler();

                    await scheduler.TriggerJob(SimpleJob.Key);

                    return Results.Ok(new { Status = $"Running the job immediately.." });
                })
                .WithName("Schedule Now")
                .WithSummary("Immediately triggers execution of the scheduled job (on-demand).")
                .WithTags("Scheduler");

                app.MapDelete("/api/scheduler/unschedule", async ([FromBody] UnscheduleRequest request, [FromServices] ISchedulerFactory factory) =>
                {
                    var scheduler = await factory.GetScheduler();
                    var triggerKey = new TriggerKey(request.TriggerName);
                    var unscheduled = await scheduler.UnscheduleJob(triggerKey);
                    if (unscheduled)
                    {
                        return Results.Ok(new { Status = "Unscheduled successfully." });
                    }
                    else
                    {
                        return Results.NotFound(new { Status = "No trigger found to unschedule." });
                    }
                })
                .WithName("Deletes Simple Trigger Now")
                .WithSummary("Immediately triggers execution of the scheduled job (on-demand).")
                .WithTags("Scheduler");
            }
        }   
    }
}
