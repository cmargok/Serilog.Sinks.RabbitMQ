using Serilog;
using Serilog.Events;
using Serilog.Sinks.RabbitMQ.Publisher.Expose.Middleware;
using Serilog.Sinks.RabbitMQ.Publisher.Core.Provider;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services
    .AddEventLogPublisher((EventLogConfig, sinkSetting) =>
    {
        EventLogConfig.CopyFrom(builder.Configuration.GetEventLogPublisherSettings());
        sinkSetting.BatchPostingLimit = 50;
        sinkSetting.Period = TimeSpan.FromSeconds(10);
        sinkSetting.QueueLimit = 500;
        sinkSetting.LogMinimumLevel = LogEventLevel.Warning;
    })
    .RegisterPublisher();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//==================== MiddleWares ===============================
app.UseMiddleware<EventLogBusExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
