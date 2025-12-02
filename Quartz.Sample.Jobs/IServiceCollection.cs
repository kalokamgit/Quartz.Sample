using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Quartz.Sample.Jobs
{
    public static class IServiceCollectionExtenstions
    {
     extension(IServiceCollection services)
        {
            public void AddQuartz(IConfiguration configuration, bool executeJobs = false)
            {
                services.AddQuartz(q =>
                {
                    q.SchedulerId = "quartz";

                    q.UseSimpleTypeLoader();

                    // Use SQL Persistence
                    q.UsePersistentStore(store =>
                    {
                        store.UseProperties = true;
                        store.RetryInterval = TimeSpan.FromSeconds(15);

                        store.UseSystemTextJsonSerializer();


                        var quartzDb = configuration.GetConnectionString("quartzdb");

                        if (string.IsNullOrWhiteSpace(quartzDb))
                        { throw new InvalidOperationException("Connection string 'quartzdb' not found in configuration."); }
                        else
                        {
                            store.UseSqlServer(sqlOpts =>
                            {
                                sqlOpts.ConnectionString = quartzDb;

                                sqlOpts.TablePrefix = "QRTZ_";
                            });

                            store.UseClustering();
                        }
                    });

                    // Register a job
                    q.AddJob();

                    q.AddHolidaysCalendar();

                    if (executeJobs)
                    {
                        // Hosted service automatically starts Quartz
                        services.AddQuartzHostedService(options =>
                        {
                            options.WaitForJobsToComplete = true;
                        });
                    }
                });

                services.AddQuartzOpenTracing(options =>
                {
                    // these are the defaults
                    options.ComponentName = "Quartz";
                    options.IncludeExceptionDetails = false;
                });
            }
    }
}
}
