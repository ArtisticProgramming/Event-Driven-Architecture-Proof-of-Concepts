# Saga Pattern

The Saga Pattern is a design pattern used to manage distributed transactions in microservices architectures. It ensures data consistency across multiple services without using traditional two-phase commit (2PC), which can lead to performance bottlenecks.

## Key Points

1. **Definition**
   - A Saga is a sequence of distributed operations executed across multiple services, with each step being a transaction. If any step fails, compensating transactions are triggered to undo the previous actions.

2. **Types of Sagas**
   - **Choreography**: Each service performs its local transaction and publishes an event to trigger the next step in the process.
   - **Orchestration**: A central coordinator controls the saga flow, invoking each step explicitly.

3. **Characteristics**
   - **Decentralized Coordination**: In choreography, services react to events without a central coordinator.
   - **Compensating Transactions**: Each step has a compensating action to revert its changes if needed.
   - **Event-Driven**: Relies heavily on events for communication between services.
   - **Asynchronous Execution**: Steps are often executed asynchronously to ensure scalability and resilience.

4. **Advantages**
   - **Resilience**: Isolates failures to individual steps, ensuring partial operations don't compromise the entire system.
   - **Scalability**: Avoids the bottleneck of locking resources as in 2PC.
   - **Loose Coupling**: Promotes independent development and deployment of microservices.

5. **Challenges**
   - **Complexity**: Managing compensations and handling failures across services can be challenging.
   - **Debugging**: Tracking failures and understanding the saga's state requires robust monitoring and logging.
   - **Data Consistency**: Achieving eventual consistency requires careful design.

---

# Saga Coordination: Choreography vs Orchestration

## Choreography

Choreography is a way to coordinate sagas where participants exchange events without a centralized point of control. With choreography, each local transaction publishes domain events that trigger local transactions in other services.

### Choreography Overview

#### Benefits
- Good for simple workflows that require few participants and don't need a coordination logic.
- Doesn't require additional service implementation and maintenance.
- Doesn't introduce a single point of failure, since the responsibilities are distributed across the saga participants.

#### Drawbacks
- Workflow can become confusing when adding new steps, as it's difficult to track which saga participants listen to which commands.
- There's a risk of cyclic dependency between saga participants because they have to consume each other's commands.
- Integration testing is difficult because all services must be running to simulate a transaction.

---

## Orchestration

Orchestration is a way to coordinate sagas where a centralized controller tells the saga participants what local transactions to execute. The saga orchestrator handles all the transactions and tells the participants which operation to perform based on events. The orchestrator executes saga requests, stores and interprets the states of each task, and handles failure recovery with compensating transactions.

### Orchestration Overview

#### Benefits
- Good for complex workflows involving many participants or new participants added over time.
- Suitable when there is control over every participant in the process, and control over the flow of activities.
- Doesn't introduce cyclical dependencies, because the orchestrator unilaterally depends on the saga participants.
- Saga participants don't need to know about commands for other participants. Clear separation of concerns simplifies business logic.

#### Drawbacks
- Additional design complexity requires an implementation of a coordination logic.
- There's an additional point of failure, because the orchestrator manages the complete workflow.
