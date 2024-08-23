# ADR 003: Handling Exceptions in BackgroundService

## Context

In the implementation of `AllJobsExecutorBackgroundService`, handling exceptions is crucial for ensuring the service operates correctly. The `BackgroundService` is designed to work within .NET's hosting model, but since this implementation is internal and not directly exposed to the user, it is important to handle exceptions gracefully within the service itself.

## Decision

To handle the `OperationCanceledException` in the `AllJobsExecutorBackgroundService` implementation, we use a `try-catch` block. This exception is expected during normal operation when the background service is canceled. By catching this exception:

- We adhere to good practices by avoiding unhandled exceptions within the internal background service implementation.
- We avoid coupling the implementation with `ILogger` or other logging mechanisms, keeping the service focused on its core functionality.

Since this background service implementation is internal and not directly accessible for modification or extension by the client, it is essential to handle such exceptions within the service to ensure robustness and prevent potential disruptions.

## Consequences

- **Pros:**
  - Ensures that the service handles expected cancellations without impacting overall functionality.
  - Maintains good practice by preventing unhandled exceptions and ensuring the service behaves predictably.

- **Cons:**
  - Adds complexity to the library by handling exceptions internally, which might require additional maintenance and careful consideration.

- **Mitigations:**
  - Document the behavior and expectations for the internal background service to guide developers on proper exception handling practices within their own job implementations.

## References

- [Unhandled exceptions from a BackgroundService](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/hosting-exception-handling)
