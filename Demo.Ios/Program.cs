using System;
using Foundation;
using Microsoft.Xna.Framework;
using Serilog;
using Serilog.Core;
using UIKit;

namespace Demo.Ios
{
    [Register("AppDelegate")]
    class Program : UIApplicationDelegate
    {
        private static Game1 game;

        internal static void RunGame()
        {
            game = new Game1();
            game.Run();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            InitLogger();
            UIApplication.Main(args, null, "AppDelegate");
        }

        public override void FinishedLaunching(UIApplication app)
        {
            RunGame();
        }

        private static void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.NSLog()
                        .Enrich.WithProperty(Constants.SourceContextPropertyName, "MyGame") //Sets the Tag field.
                        .Destructure.ByTransforming<Vector2>(_ => new { _.X, _.Y })
                        .CreateLogger();
        }
    }
}
