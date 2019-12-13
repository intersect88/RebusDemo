using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Configuration;

namespace OrderClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                var sagaDbConnectionString = ConfigurationManager.ConnectionStrings["SagasDB"].ToString();
                var bus = Configure.With(activator)
                    .Transport(t => t.UseMsmqAsOneWayClient())
                    .Subscriptions(s => s.StoreInSqlServer(sagaDbConnectionString, "SubscriptionsTable", isCentralized: true))
                    .Start();

                while (true)
                {
                    Console.WriteLine(@"
Welcome to the STORE
    
1 - Shoes  
2 - Shirt
3 - Bag
                    
");
                    Console.Write(@"
What you want to order? ");
                    var choice = Console.ReadLine();
                    var orderId = new Random().Next(100);
                    if (string.IsNullOrWhiteSpace(choice))
                    {
                        Console.WriteLine("Quitting...");
                        return;
                    }

                    switch (choice)
                    {
                        case "1":
                            bus.Publish(new Order(orderId, "Shoes")).Wait();
                            break;
                        case "2":
                            bus.Publish(new Order(orderId, "Shirts")).Wait();
                            break;
                        case "3":
                            bus.Publish(new Order(orderId, "Bags")).Wait();
                            break;
                    }


                    Console.Write(@"
Do you want confirm order?(Y/N) > ");

                    var confirmation = Console.ReadLine().ToUpper() == "Y" ? true : false;
                    bus.Publish(new OrderConfirmation(orderId, confirmation)).Wait();


                }

            }
        }
    }
}

