using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = string.Empty;
            //HubConnection connection = new HubConnectionBuilder().WithUrl("http://localhost:61285").Build();
            HubConnection connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:61285/chatHub")
            .Build();

            connection.Closed += async (error) =>
            {
                Console.WriteLine("Connection closed...");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };




            connection.On<string>("Connected",
                                    (connectionid) =>
                                    {
                                        Console.WriteLine($"Your connection id is: {connectionid}");
                                        Console.WriteLine("Enter your name");
                                    });

            connection.On<string, string>("ReceiveMessage",
                                            (user, message) =>
                                            {
                                                Console.WriteLine($"{user}:{message}");
                                            });

            try
            {
                connection.StartAsync();
                Console.WriteLine("Connected to hub");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Write your name");
            name = Console.ReadLine();
            replay(connection, name);

        }

        static void replay(HubConnection connection, string name)
        {
            string message = string.Empty;
            while (message != "x")
            {
                Console.WriteLine($"Write your message {name}");
                Console.WriteLine("(x to exit)");
                message = Console.ReadLine();
                //connection.InvokeAsync("SendMessage", name, message);
                //connection.InvokeAsync("title", "hey");
                connection.InvokeAsync("SendMessage", name, message);
            }
        }
    }
}
