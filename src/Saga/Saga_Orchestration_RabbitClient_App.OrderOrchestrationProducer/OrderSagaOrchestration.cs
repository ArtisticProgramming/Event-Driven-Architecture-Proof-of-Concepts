namespace Saga_Orchestration_RabbitClient_App.OrderOrchestrationProducer
{
    public class OrderSagaOrchestration
    {
        public OrderSagaOrchestration() { }

        public void Execute()
        {
            StartProcess();
        }   
        
        public void StartProcess()
        {
            ExecutePayment();
        }

        private void ExecutePayment()
        {
            var isPaymentOperationDone= true;
            if (isPaymentOperationDone)
            {
                PrepareOrder();
            }
            else
            {
                // Compensate
                CompensatePayment();
            }
        }

        private void CompensatePayment()
        {
            throw new NotImplementedException();
        }

        private void PrepareOrder()
        {
            var IsOrderPrepared = true;
            if (IsOrderPrepared)
            {
                DeliverOrder();
            }
            else
            {
                // Compensate
                CompensateOrderPreparation();
            }
        }

        private void DeliverOrder()
        {
            throw new NotImplementedException();
        }

        private void CompensateOrderPreparation()
        {
            var isOrderPrepared = true;
            if (isOrderPrepared)
            {
                DeliverOrder();
            }
            else
            {
                CompensatePayment();
                CompensateOrderPreparation();
            }
        }
    }
}
