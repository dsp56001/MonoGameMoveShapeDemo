using Microsoft.AspNet.SignalR.Client.Hubs;
using System;
using System.Text;
using WebShapeConsoleClient;

namespace MoveShapeDemoClient
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {

        static void Main()
        {
            
            //Init SignalR before Game to start the event stream
            SignalRGameInput.SignalRInitialize();
            //MonoGame start Game
            var factory = new MonoGame.Framework.GameFrameworkViewSource<Game1>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
        }

        
    }
}
