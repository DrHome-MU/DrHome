using Dr_Home.Helpers.helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var startUp = new ProgramHelper(builder.Configuration);

startUp.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

startUp.Configure(app);


app.Run();
