using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Serilog;

namespace Demo.Ios
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private Texture2D _ballTexture;
        private Vector2 _ballPosition;
        private float _ballSpeed;
        private const float DefaultSpeed = 125.0f;

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private bool? _wasContinuePressed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _ballSpeed = DefaultSpeed;

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: Use Content to load your game content here 
            _ballTexture = Content.Load<Texture2D>("ball");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here            
            ProcessTouch(TouchPanel.GetState(), gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_ballTexture, _ballPosition, null, Color.White, 0f, new Vector2(_ballTexture.Width / 2, _ballTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Game Logic to process touch event

        void ProcessTouch(TouchCollection touchCollection, GameTime gameTime)
        {
            Log.Information("touchs:\r\n{@TouchCollection}\r\n", touchCollection);

            var lastTouch = touchCollection.GetLastTouch();

            if (_wasContinuePressed ?? false
                &&
                (lastTouch.HasValue && lastTouch.Value.State == TouchLocationState.Released))
            {
                Log.Information("stopping touch...");
                _wasContinuePressed = false;
                return;
            }

            var hasPress = touchCollection.HasAnyActualTouch();
            if (!hasPress) { return; }

            Log.Information("start touching...");
            _wasContinuePressed = hasPress;

            var directionVector = lastTouch.Value.Position - _ballPosition;
            directionVector.Normalize();
            Log.Information("direction: {@directionVector}", directionVector);

            var distance = _ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Log.Information("distance = {0}", distance);
            _ballPosition.X += distance * directionVector.X;
            _ballPosition.Y += distance * directionVector.Y;

            var maxX = _graphics.PreferredBackBufferWidth - _ballTexture.Width / 2;
            var minX = _ballTexture.Width / 2;

            if (_ballPosition.X > maxX)
            {
                _ballPosition.X = maxX;
            }
            else if (_ballPosition.X < minX)
            {
                _ballPosition.X = minX;
            }

            var maxY = _graphics.PreferredBackBufferHeight - _ballTexture.Height / 2;
            var minY = _ballTexture.Height / 2;

            if (_ballPosition.Y > maxY)
            {
                _ballPosition.Y = maxY;
            }
            else if (_ballPosition.Y < minY)
            {
                _ballPosition.Y = minY;
            }
        }

        #endregion
    }
}

