What is a Priority Queue in RabbitMQ?

A Priority Queue in RabbitMQ allows messages to be prioritized, ensuring that higher-priority 
messages are consumed before lower-priority ones, regardless of the order in which they were 
published. This mechanism is useful for systems where certain messages require faster handling 
based on their importance.

Key Features of Priority Queues

    Message Priority:
        Each message can be assigned a priority value, which determines its processing order.
        Higher priority values are processed before lower ones.

    Queue-Level Configuration:
        The queue must be declared with a maximum priority level (x-max-priority) during its creation.
        If not specified, the queue does not support prioritization.

    Fairness and Ordering:
        Messages with the same priority are processed in the order they were published (FIFO).
        Messages with higher priorities are consumed first.

    Limited Priority Levels:
        RabbitMQ supports a maximum of 255 priority levels, but it is recommended to use a 
        smaller range (e.g., 10 levels) for better performance.