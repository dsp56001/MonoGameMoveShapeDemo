using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.AspNet.SignalR.Client.Hubs;
using IntroGameLibrary.Util;
using WebShapeConsoleClient;
using Microsoft.AspNet.SignalR.Client;

namespace MoveShapeDemoClient 
{
    class SignalRGameInput : Microsoft.Xna.Framework.GameComponent
    {

        static HubConnection connection;
        static IHubProxy myHub;
        public static StringBuilder Log;
        public static ShapeModel model;

        GameConsole console;

        Texture2D box;
        int rectWidth, rectHeight, difX, difY;
        Rectangle shape;
        MouseState currentMouse;
        MouseState prevMouse;
        
        public SignalRGameInput(Game game)
            : base(game)
        {
            
            //get console
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            this.rectHeight = 100;
            this.rectWidth = 100;
        }

        public override void Initialize()
        {

            //Create Box 
            box = new Texture2D(this.Game.GraphicsDevice, this.rectWidth, this.rectHeight);
            //Fill texture with red
            Color[] Fdata = new Color[100 * 100];
            for (int i = 0; i < Fdata.Length; ++i) Fdata[i] = Color.Red;
            box.SetData(Fdata);

            base.Initialize();
            
        }

        void currentModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            console.GameConsoleWrite(string.Format("PropertyChanged x:{0} y:(1)",  model.Left, model.Top));
            model.Updated = true; 
            
        }

        public static void SignalRInitialize()
        {
            //SignalR
            //Set connection
            connection = new HubConnection("http://localhost:1910/");
            //connection.Received += connection_Received;
            //Make proxy to hub based on hub name on server
            myHub = connection.CreateHubProxy("MoveShapeHub");
            //Start connection
            //connection.Start().Wait();
            Log = new StringBuilder();
            if (model == null)
            {
                model = new ShapeModel();
            }

            //myHub.On<ShapeModel>("updateShape", shape => Log.Append(string.Format("{0}:{1}", shape.Left, shape.Top)));
            myHub.On<ShapeModel>("updateShape", shape => model = shape);


            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    //console.GameConsoleWrite(string.Format("There was an error opening the connection:{0}", task.Exception.GetBaseException()));
                    Log.Append(string.Format("There was an error opening the connection:{0}", task.Exception.GetBaseException()));
                    //Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    //console.GameConsoleWrite("Connected");
                    //Console.WriteLine("Connected");
                    Log.Append("Connected");
                }
            }).Wait();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.UpdateMouse(gameTime);
            if (model == null)
            {
                model = new ShapeModel();
            }
            if (model != null)
            {
                if (model.Updated)
                {
                    model.Updated = false;
                }
            }
            
            
        }

        private void UpdateMouse(GameTime gameTime)
        {
            //mouse
            if (prevMouse == null) this.prevMouse = Mouse.GetState();
            this.currentMouse = Mouse.GetState();
            console.DebugText = currentMouse.X + " " + currentMouse.Y;
            //Mouse Down
            if (currentMouse.LeftButton == ButtonState.Pressed)
            {
                //MouseDown in Shape
                shape = new Rectangle((int)model.Left, (int)model.Top, rectWidth, rectHeight);
                if (shape.Contains(new Point(currentMouse.X, currentMouse.Y)))
                {
                    console.DebugText += "mouse \n" + box.Bounds + "\n" + shape;

                    if (currentMouse != prevMouse)
                    {
                        //mouse moved
                        difX = prevMouse.X - currentMouse.X;
                        difY = prevMouse.Y - currentMouse.Y;
                        
                        model.Left-= difX;
                        model.Top -= difY;
                        myHub.Invoke("UpdateModel", model);
                    }
                }
                else
                {
                    console.DebugText += "no mouse \n" + box.Bounds + "\n" + shape;
                }
            }
            prevMouse = currentMouse;
        }

        //Test Method for Logging connection data
        static void connection_Received(string obj)
        {
            //Log.Append(string.Format("{0}", obj.ToString()));
            //Console.WriteLine(string.Format("{0}", obj.ToString()));
        }

        internal void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(box, new Vector2((float)model.Left, (float)model.Top), Color.White);
        }
    }
}
