using System.Text.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Logging

builder.Host.UseSerilog((_, lc) => lc
    .WriteTo.Console()
    .WriteTo.Map(
        evt => evt.Level,
        (level, wt) => wt.File($"Logs\\{level}_{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.log")));

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/", (object jsonData, ILogger<Program> logger) =>
{
    logger.LogInformation("Data: {Data}", JsonSerializer.Serialize(jsonData));
});

// Mundet me qene poashtu string
/*app.MapPost("/", (string jsonData) =>
{

});*/

app.Run();