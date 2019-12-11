using Messages;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using System;
using System.Configuration;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var activator = new BuiltinHandlerActivator())
            {
                //var message = new DemoMessage("Demo Message");
                var db = ConfigurationManager.ConnectionStrings["SagasDB"].ToString();
                var bus = Configure.With(activator)
                    .Transport(t => t.UseMsmqAsOneWayClient())
                    //.Transport(t => t.UseMsmq("Receiver"))
                    //.Routing(r => r.TypeBased().Map<DemoMessage>("Receiver"))
                    .Subscriptions(s => s.StoreInSqlServer(db, "SubscriptionsTable", isCentralized: true))
                    //.Sagas(s => s.StoreInSqlServer(db, "Sagas", "SagaIndex"))
                    .Start();

                while (true)
                {
                    Console.WriteLine(@"Welcome to the STORE
    
                                        1 - Shoes  
                                        2 - Shirt
                                        3 - Bag
                    ");
                    Console.Write("What you want to order? ");
                    var caseNumber = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(caseNumber))
                    {
                        Console.WriteLine("Quitting...");
                        return;
                    }

                    switch (caseNumber)
                    {
                        case "1":
                            bus.Publish(new Order("Shoes")).Wait();
                            break;
                        case "2":
                            bus.Publish(new Order("Shirts")).Wait();
                            break;
                        case "3":
                            bus.Publish(new Order("Bags")).Wait();
                            break;
                    }


                    Console.Write("Do you want confirm order?(Y/N) > ");

                    var confirmation = Console.ReadLine().ToUpper() == "Y" ? true : false;
                    bus.Publish(new OrderConfirmation(confirmation)).Wait();
                    Console.Write("Total cost amount: 100$. Do you want to pay order?(Y/N) >");
                    var payment = Console.ReadLine().ToUpper() == "Y" ? true : false;
                    bus.Publish(new OrderPayment(payment)).Wait();
                    
                }


            }
        }
    }
}
