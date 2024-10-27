using fleetpanda.dataaccess.Abstractions;
using fleetpanda.dataaccess.Repositories;
using fleetpanda.dataaccess.Repositories.Abstractions;
using Hangfire.SqlServer;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using fleetpanda.dataaccess.Constants;

namespace fleetpanda.webui;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        RegisterTypes(builder);
        AddHangfire(builder);
        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Customers}/{action=Source}/{id?}");
        ConfigureHangfireJob(app.Services);

        app.Run();
    }

    private static void RegisterTypes(WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        var connStringSource = builder.Configuration.GetConnectionString("SourceDb")!;
        builder.Services.AddDbContext<SourceDbContext>(options => options.UseSqlServer(connStringSource));

        var connStringDestination = builder.Configuration.GetConnectionString("TargetDb")!;
        builder.Services.AddDbContext<TargetDbContext>(options => options.UseSqlServer(connStringDestination));
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
        builder.Services.AddScoped<Jobs.RecurringJob>();
    }

    private static void AddHangfire(WebApplicationBuilder builder)
    {
        IConfiguration config = builder.Configuration;
        var conString = config.GetConnectionString("HangfireDb");
        var sqlStorage = new SqlServerStorage(conString, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true,
            PrepareSchemaIfNecessary = true
        });
        builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseStorage(sqlStorage));

        JobStorage.Current = sqlStorage;
        builder.Services.AddHangfireServer();
    }

    public static void ConfigureHangfireJob(IServiceProvider services)
    {
        const string jobName = Job.CUSTOMER_SYNCHRONIZATION;
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TargetDbContext>();
        var jobSetting = dbContext.JobSettings.FirstOrDefault(js => js.IsActive);

        if (jobSetting != null)
        {
            var manager = services.GetService<IRecurringJobManager>();
#pragma warning disable CS0618 // Type or member is obsolete
            var cronExpression = Cron.MinuteInterval(jobSetting.DurationInMinutes);
#pragma warning restore CS0618 // Type or member is obsolete

            manager.RemoveIfExists(jobName);
            manager.AddOrUpdate(jobName, () => ExecuteRecurringJob(services), cronExpression);
        }
    }




    public static async Task ExecuteRecurringJob(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var recurringJob = scope.ServiceProvider.GetRequiredService<Jobs.RecurringJob>();
        await recurringJob.ExecuteAsync();
    }
}

