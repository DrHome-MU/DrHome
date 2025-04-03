using Dr_Home.Helpers.helpers;
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
    app.MapOpenApi();

app.UseSerilogRequestLogging();

startUp.Configure(app);


app.Run();
