using System.Data;
using Microsoft.Data.SqlClient;
using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var quartzDb = builder.AddConnectionString("quartzdb");

// Execute an initialization script on the quartzdb connection if configured.
// The script path is read from configuration key "QuartzDb:InitScriptPath".
var scriptPath = builder.Configuration["QuartzDb:InitScriptPath"];
if (!string.IsNullOrWhiteSpace(scriptPath))
{
    var connectionString = await  quartzDb.Resource.GetConnectionStringAsync();
    if (string.IsNullOrWhiteSpace(connectionString))
    {
        Console.WriteLine("Connection string 'quartzdb' not found in configuration.");
    }
    else if (!File.Exists(scriptPath))
    {
        Console.WriteLine($"Quartz DB init script not found: {scriptPath}");
    }
    else
    {
        try
        {
            var script = File.ReadAllText(scriptPath);
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = script;
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Executed script '{scriptPath}' on 'quartzdb'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to execute quartzdb script: {ex.Message}");
            throw;
        }
    }
}

var quartzApi = builder.AddProject<Projects.Quartz_Sample_Api>("quartz-api")
    .WithUrlForEndpoint("https", url =>
    {
        url.DisplayText = "Scalar UI (HTTPS)";
        url.Url = $"{url.Url}/scalar"; // Or the path you configured in your API project
    })
    .WithReference(quartzDb);


builder.AddProject("quartz-runner", "../Quartz.Sample.Runner/Quartz.Sample.Runner.csproj")
    .WithReference(quartzDb);

builder.Build().Run();
