# JobScheduler.Cron

## Overview

The `JobScheduler.Cron` library provides a flexible and easy-to-use solution for integrating job 
scheduling into .NET applications. It allows you to configure and manage scheduled jobs using cron 
expressions and background services.

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

### Hosted Job Scheduler

To configure the job scheduler with a hosted service that runs the jobs in the background, use the 
`AddHostedJobScheduler` method. This method registers all the services required for job scheduling 
and also adds a hosted service to manage job execution.

```csharp
using JobScheduler.Cron;

// Configure job scheduler services with hosted service
services.AddHostedJobScheduler(new JobConfiguration
{
    Cron = "0 0 * * *", // Example cron expression
    OnExecute = async (serviceProvider, cancellationToken) =>
    {
        // Job execution logic
    }
});
```

### Only Job Scheduler

Registers the necessary services for job scheduling but does not include the background service 
that executes the jobs. It is the responsibility of the user to call IJobExecuter.Execute to trigger the 
job execution according to the configured schedule.

```csharp
using JobScheduler.Cron;

// Configure job scheduler services
services.AddJobScheduler(new JobConfiguration
{
    Cron = "0 0 * * *", // Example cron expression
    OnExecute = async (serviceProvider, cancellationToken) =>
    {
        // Job execution logic
    }
});
```

### Job Configuration

The `JobConfiguration` class allows you to specify the schedule and execution logic for your jobs:

- **Cron**: A cron expression compatible with the NCrontab library for defining the job schedule.
- **OnExecute**: A `Func<IServiceProvider, CancellationToken, Task>` delegate that defines the logic to 
be executed when the job runs.
- **GetNow**: A `Func<IServiceProvider, DateTime>` delegate that provides the current time, which 
defaults to the system's UTC time.

### Error Handling

The job executor does not handle exceptions. It is the responsibility of the job configuration to ensure 
that all necessary error handling is implemented within the job itself.

## Contributing

Contributions are welcome! Please open an issue or a pull request if you have any suggestions or improvements.