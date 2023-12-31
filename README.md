# Serilog.Sinks.RabbitMQ by Cmargok

## Why I made this new version

Picture a world in constant evolution, where C# and .NET advance by leaps and bounds. In this scenario, updating and migrating libraries becomes crucial. And what happens in distributed systems? Centralizing logs emerges as a brilliant option. That's why we've implemented a Serilog Sink that sends logs via events to RabbitMQ. We used the library https://github.com/lyng-dev/serilog-sinks-rabbitmq as a base, leveraging the new features offered by .NET 8 and C# 12 to expand its functionalities.

### Dependencies

|Serilog.Sinks.RabbitMQ|.NETCore|Serilog|RabbitMQ.Client|Newtonsoft.Json|
|---|---|---|---|---|
|1.0.3|8.*|3.1.1|6.8.1|13.0.3|

## Installation

Using [Nuget](https://www.nuget.org/packages/Serilog.Sinks.RabbitMQ/):

```
dotnet add package Serilog.Sinks.RabbitMQ.Publisher --version 1.0.3
```
## Version 1.0.3 configuration

We can configure the RabbitMQSink in many ways

* usings 
```csharp
using Serilog.Sinks.RabbitMQ.Publisher.Core.Provider;
using Serilog.Sinks.RabbitMQ.Publisher.Configuration;
```

Declare UseSerilog
```csharp
builder.Host.UseSerilog();
```
* Using appsettings.json

```json
 "EventLogPublisher": {
    "ApiName": "MyApi",
    "Username": "userMyUser",
    "Password": "asda",
    "Port": 5672,
    "Hostnames": [ "localhost" ],
    "Exchange": {
      "ExchangeName": "logsExchange",
      "ExchangeType": "direct",
      "RouteKey": "logs"
    }
  }

```


To call it from program.cs, we use the extension method builder.Configuration.GetEventLogPublisherSettings(). This will read the settings we've added in the JSON file. Don't forget to call the CopyFrom method to copy the data into the sink.

```csharp
builder.Services
    .AddEventLogPublisher((EventLogConfig, sinkSetting) =>
    {
        EventLogConfig.Clone(builder.Configuration.GetEventLogPublisherSettings());
        EventLogConfig.Exchange.DeliveryMode = RabbitMQDeliveryMode.Durable;// this is the default value, its not mandatory
        sinkSetting.BatchPostingLimit = 50;
        sinkSetting.Period = TimeSpan.FromSeconds(10);
        sinkSetting.QueueLimit = 500;
        sinkSetting.LogMinimumLevel = LogEventLevel.Warning;
    })
    .RegisterPublisher();

```

* Register properties by creating a new instace of EventClientConfiguration


```csharp
builder.Services
    .AddEventLogPublisher((EventLogConfig, sinkSetting) =>
    {
        EventLogConfig.Clone(new EventClientConfiguration()
        {
            ApiName = "MyApi",
            Username = "userMyUser",
            Password = "asda",
            Hostnames = ["localhost"],
            Port = 5672,
            Exchange = new ExchangeConfiguration()
            {
                ExchangeName = "logsExchange",
                ExchangeType = "direct",
                DeliveryMode = RabbitMQDeliveryMode.Durable,
                RouteKey = "logs",
            }

        });
        sinkSetting.BatchPostingLimit = 50;
        sinkSetting.Period = TimeSpan.FromSeconds(10);
        sinkSetting.QueueLimit = 500;
        sinkSetting.LogMinimumLevel = LogEventLevel.Information;
    })
    .RegisterPublisher();

```
Don't forget to call the CopyFrom method to copy the data into the sink.

* Register properties once at time

```csharp
builder.Services
      .AddEventLogPublisher((EventLogConfig, sinkSetting) =>
        {

            EventLogConfig.ApiName = "MyApi";
            EventLogConfig.Username = "asdas";
            EventLogConfig.Password = "asdafgf@1";
            EventLogConfig.Hostnames = ["localhost"];
            EventLogConfig.Port = 5672;
            EventLogConfig.Exchange.ExchangeName = "logsExchange";
            EventLogConfig.Exchange.ExchangeType = "direct";
            EventLogConfig.Exchange.DeliveryMode = RabbitMQDeliveryMode.Durable;
            EventLogConfig.Exchange.RouteKey = "logs";
            sinkSetting.BatchPostingLimit = 50;
            sinkSetting.Period = TimeSpan.FromSeconds(10);
            sinkSetting.QueueLimit = 500;
            sinkSetting.LogMinimumLevel = LogEventLevel.Information;
        })
       .RegisterPublisher();

```

* Of Course you can mix them however you want

### Adding more sinks into RabbitMQ.Sink Configuration

The method RegisterPublisher has 3 params overload

#### RegisterPublisher()
It will use the default rabbitmq logger
```csharp
.RegisterPublisher();
```  
#### RegisterPublisher(LogEventLevel logLevel = LogEventLevel.Debug, bool enableRabbitMQClientLogger = false, bool UseDefaultLogger = true)
It has three params
* addDefaultConsole 
  -> set if the log level por default console log
*  enableRabbitMQClientLogger 
    -> enable or disable rabbitMQlogger
* UseDefaultLogger 
    -> enable or disable default centralized logger

#### RegisterPublisher(Action<LoggerConfiguration> loggerConfig = null!, bool enableRabbitMQClientLogger = false, bool UseDefaultLogger = true) 

It has three params
* loggerConfig 
```csharp
    Action<LoggerConfiguration> loggerConfig = null!, 
```  
we can add new sinks or let it in null


```csharp

    .RegisterPublisher(logger =>
    {
        logger.WriteTo.Logger(cl => cl
            .Filter.ByIncludingOnly(e => e.Level > LogEventLevel.Debug)
            .WriteTo.Console());
    }
);  

```
*  enableRabbitMQClientLogger 
    -> enable or disable rabbitMQlogger
* UseDefaultLogger 
    -> enable or disable default centralized logger


### Centralized Logger 

I've implemented a centralized logger called EventLogger who uses the interface IEventLogger 
```csharp
public interface IEventLogger
{
    public void LoggingInformation(string message);
    public void LoggingWarning(string message);        
    public void LoggingError(Exception ex, string message);
}
```
If you set the propertie in false, the logger I've made wont registered in serviceCollection, you are free to use the centralized implementation made by your own or by using the Ilogger<T> implementation.

## Middleware to catch dispose sink Exceptions

use the EventLogBusExceptionMiddleware to catch any exception caused by disposing all the stuff used to run this library treating with rabbitmq client and threadsafe implementation. This middleware uses the HandleExceptionAsync method to handle the list of exceptions, you can overwrite it to re implement the functionality.
The exception we use to catch is EventLogBusException

```csharp
public virtual async Task HandleExceptionAsync(EventLogBusException exceptions)
{
   foreach (var ex in exceptions._innerExceptions)
   {
       await Console.Out.WriteLineAsync("Source ->" + ex.Source + "\n message -> " + ex.Message);
   }
}
```

Don't forget register the middleware
```csharp

using Serilog.Sinks.RabbitMQ.Publisher.Expose.Middleware;

//==================== MiddleWares ===============================
app.UseMiddleware<EventLogBusExceptionMiddleware>();
```

## References

- [Serilog](https://serilog.net/)
- [Logging in ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging)
- [Dependency Injection in ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
