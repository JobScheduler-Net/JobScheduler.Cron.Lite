# ADR 002: Sequential Job Execution

## Context

In designing the job scheduler library, a decision was needed regarding whether to allow parallel execution of jobs. While concurrent job execution might increase throughput in some cases, it introduces significant complexity in managing resources and ensuring thread safety.

Concurrency can also pose significant risks, particularly if jobs are resource-intensive. Allowing multiple jobs to run in parallel could exhaust system resources, leading to instability or crashes. Furthermore, managing parallel execution would require the implementation of sophisticated limiting mechanisms to control the number of jobs running simultaneously.

## Decision

The decision was made to not allow parallel job execution within the job scheduler. Instead, jobs are executed sequentially to simplify concurrency management and reduce the risk of resource exhaustion or deadlocks. If parallel execution is desired, it is left to the client to implement their own solution outside of the job scheduler.

## Consequences

- **Pros:**
  - Simplifies the job scheduler implementation by avoiding the need for complex concurrency management.
  - Reduces the risk of resource exhaustion and system instability caused by uncontrolled parallel job execution.

- **Cons:**
  - May limit throughput in scenarios where parallel job execution could be beneficial.
  - Requires clients who need parallel execution to implement their own concurrency solutions.

- **Mitigations:**
  - Clearly document the decision to not support parallel execution and provide guidelines for clients on how to implement parallelism if needed.
