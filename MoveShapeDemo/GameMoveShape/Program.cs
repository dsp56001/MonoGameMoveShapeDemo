#region Using Statements
using MoveShapeDemoClient;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace GameMoveShape
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Init SignalR before Game to start the event stream
            SignalRGameInput.SignalRInitialize();
            
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
