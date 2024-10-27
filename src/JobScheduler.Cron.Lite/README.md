## Overview

Provides a small, flexible, and easy-to-use solution for scheduling a cron job — built with only 20 executable lines of code, making it simple to understand, customize, and expand to fit your needs.

## Comparison with Other Job Scheduling Libraries

When choosing a job scheduling library for .NET, it's important to consider various factors like codebase size, scheduling capabilities, and the ease of customization. Below is a comparison of `JobScheduler.Cron.Lite` with other popular libraries, such as Hangfire and Quartz.NET:

| Feature / Library         | Hangfire                                                                                      | Quartz.NET                                                        | JobScheduler.Cron.Lite                                                                                 |
|---------------------------|-----------------------------------------------------------------------------------------------|-------------------------------------------------------------------|---------------------------------------------------------------------------------------------------|
| **Codebase Size**         | +40,000 lines of code, with +7,000 lines being executable                                     | +55,000 lines of code, with +12,000 lines being executable        | 120 lines of code, with only 20 lines being executable                                  |
| **Scheduling**            | Recurring jobs, delayed jobs, fire-and-forget jobs                                            | Cron-based scheduling, simple triggers, calendars                 | Cron-based scheduling only                                                                        |
| **Customization**         | Possible but requires significant time to understand and expand                               | Possible but requires significant time to understand and expand   | Easily customizable due to its simplicity                                                         |
| **Default Configuration** | Requires configuring parallelism, logging, database setup, and scoped dependency injection    | Requires configuring parallelism, logging, and database setup     | Requires minimal configuration; runs out-of-the-box with basic settings; logging added on demand  |
| **Testability**           | Limited flexibility in mocking time                                                           | Limited flexibility in mocking time                               | High testability with easy mocking of time and other interfaces                                   |

## Get Started

### Creating a Custom Job

To create a custom job, you need to implement the `IJob` interface. The `Execute` method will contain the logic that should be executed when the job is triggered.

Here’s an example of a custom job implementation:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using JobScheduler.Cron.Lite;

public class MyCustomJob : IJob
{
    public Task Execute(CancellationToken cancellationToken)
    {
        try
        {
            // Your job logic goes here
        } 
        catch (Exception exception) 
        {
            // Handle exception, not throwing exception
        }
    }
}
```

### Configure a Custom Job

To configure the job scheduler, use the `AddJobScheduler` method.

```csharp
using JobScheduler.Cron.Lite.DependencyInjection;
using JobScheduler.Cron.Lite.Configurations;

// Configure a job scheduler
services.AddJobScheduler<MyCustomJob>(new JobConfiguration
{
    Cron = "* * * * * *",
});

// Configure another job scheduler
services.AddJobScheduler<MyOtherCustomJob>(new JobConfiguration
{
    Cron = "* * * * * *",
});
```

This method inject `IJob`. If you want to run configured jobs manually, 
use this interface to do so.

**Manual Execution**

```csharp
using JobScheduler.Cron.Lite;

// Resolve the job from the service provider
var job = serviceProvider.GetRequiredService<IJob>();

// Manually run configured jobs
await job.Execute(default);
```

### Configure hosted service

To configure a background service using the hosted service pattern, 
use `AddHostedJobScheduler`.

**Configuration**

```csharp
using JobScheduler.Cron.Lite.DependencyInjection;

// Configure the hosted service to run all configured jobs
services.AddHostedJobScheduler();
```

### Job Configuration

The `JobConfiguration` class allows you to specify the schedule and execution logic for your jobs:

- **Cron**: A cron expression compatible with the NCrontab library for defining the job schedule.
- **GetTimeReference**: A `Func<IServiceProvider, DateTime>` delegate that provides the reference date and time for running the job. By default, this function uses the respective code:

```csharp
static serviceProvider => serviceProvider.GetRequiredService<TimeProvider>().GetUtcNow().DateTime;
```

### Error Handling

The job executor does not handle exceptions. It is the responsibility of the job configuration to ensure that all necessary error handling is implemented within the job itself.