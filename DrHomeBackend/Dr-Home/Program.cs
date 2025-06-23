using Dr_Home.BackgroundJobs;
using Dr_Home.Helpers.helpers;
using Hangfire;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var startUp = new ProgramHelper(builder.Configuration);

startUp.ConfigureServices(builder.Services);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);

});

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();
  //  app.MapOpenApi();



app.UseSerilogRequestLogging();

startUp.Configure(app);

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();

var manageSchedulesService = scope.ServiceProvider.GetRequiredService<IManageSchedules>();

RecurringJob.AddOrUpdate("", () => manageSchedulesService.DeleteExpiredSchedules(), Cron.Daily);

app.Run();
