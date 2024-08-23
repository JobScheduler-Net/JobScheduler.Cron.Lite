# ADR 002: Sequential Job Execution

## Context

In our job scheduling system, there was a decision to be made about whether to execute jobs in parallel or sequentially. Executing jobs in parallel can potentially reduce total execution time and increase throughput. However, it also introduces complexities and risks that need careful management.

## Decision

We have decided **not** to execute jobs in parallel. Instead, jobs will be executed sequentially, ensuring that only one job is running at any given time.

## Justification

- **Concurrency Control is Complex and Risky**: Managing concurrency is inherently complex and introduces risks such as race conditions, deadlocks, and inconsistent states. Properly handling these scenarios requires significant effort and expertise.

- **Need for Limiters**: To prevent resource exhaustion, limiters would need to be implemented to control the number of concurrent jobs. This adds further complexity to the system and requires continuous monitoring and adjustment.

- **Risk of Resource Exhaustion**: Without proper controls, parallel job execution could lead to the application running out of resources (e.g., CPU, memory, I/O), potentially causing performance degradation or crashes.

- **Custom Solutions by Clients**: If clients require parallel job execution, they should implement their own solutions tailored to their specific needs and environment. This allows them to manage concurrency according to their own resource availability and performance requirements.

## Alternatives Considered

1. **Parallel Execution with Concurrency Controls**: This alternative would involve running multiple jobs in parallel with mechanisms in place to control the degree of concurrency (such as semaphores or queues). It was rejected due to the increased complexity and potential for error.

2. **Configurable Parallelism**: Allowing a configurable number of concurrent jobs based on the environment and workload. This option was also rejected to keep the system simple and avoid the need for intricate configuration management.

## Conclusion

The decision to not execute jobs in parallel was made to prioritize simplicity, stability, and safety. While this approach may result in longer execution times in some cases, the benefits of avoiding the complexities and risks associated with concurrency outweigh the drawbacks. Clients who require parallel execution can implement their own solutions that best fit their needs.
