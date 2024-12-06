Dead Letter Exchanges
What is a Dead Letter Exchange

Messages from a queue can be "dead-lettered", which means these messages are republished to an exchange 
when any of the following four events occur.

1-The message is negatively acknowledged by an AMQP 1.0 receiver using the rejected outcome or 
by an AMQP 0.9.1 consumer using basic. reject or basic.nack with requeue parameter set to false, or
2-The message expires due to per-message TTL, or
3-The message is dropped because its queue exceeded a length limit, or
4-The message is returned more times to a quorum queue than the delivery-limit.

https://www.rabbitmq.com/docs/dlx