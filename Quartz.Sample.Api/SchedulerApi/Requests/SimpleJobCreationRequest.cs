namespace Quartz.Sample.Api.SchedulerApi.Requests
{
    public record SimpleJobCreationRequest
    {
        public required int IntervalSeconds { get; init; }
        public required string TriggerName { get; init; }
        public bool AvoidHoliday { get; init; } = false;
        public string Message { get; init; } = string.Empty;
    }
}
