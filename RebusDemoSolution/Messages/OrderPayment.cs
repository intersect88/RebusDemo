namespace Messages
{
    public class OrderPayment
    {
        public int Id;
        public bool Payment;

        public OrderPayment(int id, bool payment)
        {
            Id = id;
            Payment = payment;
        }
    }
}