# JobScheduler.Cron

## Overview

The `JobScheduler.Cron` library provides a flexible and easy-to-use solution for integrating job
scheduling into .NET applications. It allows you to configure and manage scheduled jobs using cron
expressions and background services.

## Comparison with Other Job Scheduling Libraries

When choosing a job scheduling library for .NET, it's important to consider various factors like codebase size, scheduling capabilities, and the ease of customization. Below is a comparison of `JobScheduler.Cron` with other popular libraries, such as Hangfire and Quartz.NET:

| Feature / Library         | Hangfire                                                                                      | Quartz.NET                                                        | JobScheduler.Cron                                                                                 |
|---------------------------|-----------------------------------------------------------------------------------------------|-------------------------------------------------------------------|---------------------------------------------------------------------------------------------------|
| **Codebase Size**         | +53,000 lines of code                                                                         | +61,000 lines of code                                             | Less than 200 lines of code                                                                       |
| **Scheduling**            | Recurring jobs, delayed jobs, fire-and-forget jobs                                            | Cron-based scheduling, simple triggers, calendars                 | Cron-based scheduling only                                                                        |
| **Customization**         | Possible but requires significant time to understand and expand                               | Possible but requires significant time to understand and expand   | Easily customizable due to its simplicity                                                         |
| **Default Configuration** | Requires configuring parallelism, logging, database setup, and scoped dependency injection    | Requires configuring parallelism, logging, and database setup     | Requires minimal configuration; runs out-of-the-box with basic settings; logging added on demand  |
| **Testability**           | Limited flexibility in mocking time                                                           | Limited flexibility in mocking time                               | High testability with easy mocking of time and other interfaces                                   |

## Installation

You can install the `JobScheduler.Cron` library via NuGet. Use the following command in your package manager console:

```bash
Install-Package JobScheduler.Cron
```

Alternatively, you can add the package directly to your project file:

```xml
<PackageReference Include="JobScheduler.Cron" Version="1.0.0" />
```

## Usage

### Creating a Custom Job

To create a custom job, you need to implement the `IJob` interface. The `Execute` method will contain the logic that should be executed when the job is triggered.

Here’s an example of a custom job implementation:

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

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

### Hosted Job Scheduler

To configure the job scheduler with a hosted service that runs the jobs in the background, use the
`AddHostedJobScheduler` method. This method registers all the services required for job scheduling
and also adds a hosted service to manage job execution.

```csharp
using JobScheduler.Cron.DependencyInjection;

// Configure job scheduler services with hosted service
services.AddHostedJobScheduler<MyCustomJob>(new JobConfiguration
{
    Cron = "0 0 * * *",
});
```

### Only Job Scheduler

Registers the necessary services for job scheduling but does not include the background service
that executes the jobs. It is the responsibility of the user to call IJob.Execute to trigger the
job execution according to the configured schedule.

**Configuration**

```csharp
using JobScheduler.Cron.DependencyInjection;

// Configure job scheduler services
services.AddJobScheduler<MyCustomJob>(new JobConfiguration
{
    Cron = "0 0 * * *",
});
```

**Execution**

```csharp
using JobScheduler.Cron;

// Resolve the job from the service provider
var job = serviceProvider.GetRequiredService<IJob>();

// Manually run configured jobs
await job.Execute(default);
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

## Contributing

Contributions are welcome! Please open an issue or a pull request if you have any suggestions or improvements.
