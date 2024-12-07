NOTES:

What is Competing Consumers pattern?

Enable multiple concurrent consumers to process messages received on the same messaging channel. 
With multiple concurrent consumers, a system can process multiple messages concurrently to 
optimize throughput, to improve scalability and availability, and to balance the workload.

----------------------------------------------------------------------------------------

WHAT IS BasicQos?

It Control how messages are sent to consumers to ensure that they don't get overwhelmed.
prefetchSize: 0: Ignore the size limit of messages.
prefetchCount: 1: Only send one message at a time to the consumer. The consumer must acknowledge the message 
before receiving the next one. 
global: false: Apply this setting to individual consumers rather than the whole channel.

example: 
> channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

----------------------------------------------------------------------------------------