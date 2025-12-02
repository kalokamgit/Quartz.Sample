namespace Quartz.Sample.Jobs
{
    [DisallowConcurrentExecution]
    public class SimpleJob : IJob
    {
        public static readonly JobKey Key = new("simple-job", "quartz");

        public Task Execute(IJobExecutionContext context)
        {
            var data = context.MergedJobDataMap;

            Console.WriteLine($"Executing simple Job...");
            if (data.Contains("message"))
            {
                Console.WriteLine($"Data: {data["message"]}");
            }
            return Task.CompletedTask;
        }
    }
}
