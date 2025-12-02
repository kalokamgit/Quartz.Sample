namespace Quartz.Sample.Api.SchedulerApi.Requests
{
    public record UnscheduleRequest
    {
        public required string TriggerName { get; init; }
    }
}
