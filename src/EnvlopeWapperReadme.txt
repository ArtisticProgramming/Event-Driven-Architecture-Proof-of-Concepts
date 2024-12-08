Envelope Wrapper

Most messaging systems divide the message data into a header and a body. 
The header contains fields that are used by the messaging infrastructure to manage the flow of messages. 
However, most endpoint systems that participate in the integration solution generally 
are not aware of these extra data elements. In some cases, systems may even consider 
these fields as erroneous because they do not match the message format used by the application. 
On the other hand, the messaging components that route the messages between the applications may 
require the header fields and would consider a message invalid if it does not contain the proper header fields.

Use a Envelope Wrapper to wrap application data inside an envelope that is compliant with the messaging infrastructure. 
Unwrap the message when it arrives at the destination.

https://www.enterpriseintegrationpatterns.com/patterns/messaging/EnvelopeWrapper.html