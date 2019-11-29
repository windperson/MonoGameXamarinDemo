using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;
using Serilog;
using Serilog.Core;

namespace Demo.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            InitLogger();
            _game = new Game1();
            _view = _game.Services.GetService(typeof(View)) as View;

            SetContentView(_view);
            _game.Run();
        }

        private void InitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.AndroidLog()
                        .Enrich.WithProperty(Constants.SourceContextPropertyName, "MyGame") //Sets the Tag field.
                        .Destructure.ByTransforming<Vector2>(_ => new { _.X, _.Y })
                        .CreateLogger();
        }
    }
}
