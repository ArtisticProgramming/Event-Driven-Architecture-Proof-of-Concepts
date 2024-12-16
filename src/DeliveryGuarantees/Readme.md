# Outbox, Inbox patterns and delivery guarantees

## Delivery Guarantees in Messaging

Messaging systems typically provide three main delivery guarantees:

### 1. At-Most Once
- **Definition:** The simplest guarantee where a message may be delivered once or not at all.
- **Behavior:** If a message fails during processing, it is lost and not handled. This can happen due to:
  - Transient errors (e.g., temporary database outage, network issues).
  - Non-transient errors (e.g., server crashes).
- **Advantages:** No need to handle idempotence, simplifying implementation.
- **Disadvantages:** No guarantee of message delivery.

### 2. At-Least Once
- **Definition:** Ensures that a sent message will always be delivered, but it may be handled multiple times.
- **Behavior:**
  - Messages may be re-published by the producer (e.g., using the Outbox Pattern).
  - Messages may be re-processed by the consumer (e.g., using the Inbox Pattern).
- **Challenges:**
  - Duplicated processing requires idempotency handling to avoid issues (e.g., issuing an invoice multiple times).
  - Without idempotency, repeated handling may lead to corrupted data.
- **Example Issue:** An invoice is saved to the database, but a timeout occurs. Retrying without verifying if the invoice already exists may result in duplication.

### 3. Exactly Once
- **Definition:** Guarantees that a message will be delivered and processed exactly once.
- **Challenges:**
  - Achieving this is complex and sometimes impossible due to potential failures at multiple stages of processing.
  - Requires both retries and robust idempotency support to prevent side effects from repeated operations.
- **Requirement:** Operations must be designed to ensure that repeated execution does not lead to unintended consequences.

---

## Outbox Pattern &  Inbox Pattern

![Outbox, Inbox patterns](./img/In-OutBoxPattern.png)  

To achieve reliable message delivery, particularly for at-least-once and exactly-once semantics, the following patterns are commonly used:

## 1. Outbox Pattern
- **Purpose:** Ensures a message is sent (e.g., to a queue) successfully at least once.
- **How It Works:**
  1. Instead of directly publishing a message to the queue, it is stored in a temporary storage (e.g., a database table).
  2. The entity save operation and message storage are wrapped in a Unit of Work (transaction), ensuring consistency.
  3. A background process periodically checks the table for unsent messages.
  4. The worker sends these messages to the queue and marks them as sent upon receiving confirmation (e.g., an ACK from the queue).
- **Delivery Semantics:** 
  - Provides **at-least-once** delivery but not exactly-once.
  - **Reason:** Writing to the database may fail, leading to retries until the message is correctly marked as sent.

## 2. Inbox Pattern
- **Purpose:** Handles incoming messages (e.g., from a queue) reliably.
- **How It Works:**
  1. Incoming events are stored in a database table.
  2. After saving the event, an ACK is sent back to the queue.
  3. If the save succeeds but the ACK is not sent, the queue retries the delivery.
  4. A background process (similar to Outbox Pattern) triggers message handlers to perform the business logic.
- **Delivery Semantics:** 
  - Provides **at-least-once** delivery.
  - **Reason:** Similar to the outbox pattern, the process ensures eventual delivery but allows for potential retries if failures occur before the message is fully processed.

Both patterns are essential for building reliable, distributed systems that require robust message processing mechanisms.

---

source: https://event-driven.io/en/outbox_inbox_patterns_and_delivery_guarantees_explained/