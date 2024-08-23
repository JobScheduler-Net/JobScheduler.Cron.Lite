# ADR 001: Error handling

## Context

In designing the job scheduler library, a decision was needed regarding how to handle exceptions that occur during job execution. Typically, centralized error handling might be implemented to catch and log exceptions, creating a record of failures.

However, in this library, we decided to delegate all error handling to the job implementations themselves. This means that the `JobExecutor` does not catch exceptions or log any errors when executing jobs.

## Decision

The job executor will not handle exceptions or perform any logging. Instead, it is the responsibility of the individual job implementations to handle their own errors. This decision was made to avoid creating unnecessary exception events and logging, which can clutter logs and obscure meaningful information.

## Consequences

- **Pros:**
  - Reduces noise in logs by avoiding generic exception handling and logging at the job executor level.
  - Provides flexibility for job implementers to handle errors in ways that make the most sense for their specific scenarios.
  - Keeps the job executor codebase simple and focused on executing jobs rather than managing error states.

- **Cons:**
  - Requires each job to have its own error handling, which can lead to inconsistencies if not properly managed.

- **Mitigations:**
  - Provide clear documentation and examples for developers on how to implement effective error handling within their jobs.
