namespace Quartz.Sample.Jobs
{
    public static class JobsExtention
    {
        extension(IServiceCollectionQuartzConfigurator quartzConfigurator) {
        
            public void AddJob()
            {
                quartzConfigurator.AddJob<SimpleJob>(opts => opts.WithIdentity(SimpleJob.Key).StoreDurably());
            }
        }
    }
}
