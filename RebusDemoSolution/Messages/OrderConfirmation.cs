namespace Messages
{
    public class OrderConfirmation
    {
        public int Id;
        public bool Confirmation;

        public OrderConfirmation(int id, bool confirmation)
        {
            Id = id;
            Confirmation = confirmation;
        }
    }
}