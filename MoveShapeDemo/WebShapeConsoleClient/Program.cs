using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShapeConsoleClient
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            //Set connection
            var connection = new HubConnection("http://localhost:1910/");
            //Make proxy to hub based on hub name on server
            var myHub = connection.CreateHubProxy("MoveShapeHub");

            connection.ConnectionSlow += () => Console.WriteLine("Connection problems.");
            //connection.Received += connection_Received;
            //myHub.On<double, double>("updateShape", connection_Received);
            //myHub.On<double, double>("updateShape", (left, top) => 
            //    { Console.Write(left + ": "); Console.WriteLine(top); }
            //);
            myHub.On<ShapeModel>("updateShape",shape=> Console.WriteLine(string.Format("{0}:{1}",shape.Left,shape.Top))); 
           
            
           //Start connection
            //connection.Start().Wait();
            
            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }
            }).Wait();
            



            string msg = null;

            while ((msg = Console.ReadLine()) != null)
            {
                myHub.Invoke("Send", "Console app", msg).Wait();
            }

            //Console.Read();
            connection.Stop();
        }

        static void connection_Received(string obj)
        {
            Console.WriteLine(string.Format("{0}",obj.ToString()));
        }

    }
}
