A Topic Exchange in RabbitMQ routes messages to queues based on a pattern matching between the 
routing key of the message and the binding key of the queue. 

Routing keys are dot-separated strings (e.g., log.info), and binding keys can include
wildcards: * (matches exactly one word) and # (matches zero or more words). 
This allows for flexible and dynamic message routing, ideal for scenarios like logging
systems where different queues subscribe to messages based on specific 
topics (e.g., log.* for one-word severity logs or log.# for all logs).


Comparison of Wildcards in Topic Exchange

    * (asterisk):
        Matches exactly one word.
        Example:
            Binding key: log.*
            Matches: log.info, log.error
            Does not match: log, log.info.debug.

    # (hash):
        Matches zero or more words.
        Example:
            Binding key: log.#
            Matches: log, log.info, log.error.debug.