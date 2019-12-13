namespace Messages
{
    public class Order
    {
        public int Id;
        public string Type;

        public Order(int id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}