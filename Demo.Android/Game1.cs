using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Serilog;

namespace Demo.Android
{
    public class Game1 : Game
    {
        private Texture2D _ballTexture;
        private Vector2 _ballPosition;
        private float _ballSpeed;
        private const float DefaultSpeed = 125.0f;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private bool? _wasContinuePressed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            _ballSpeed = DefaultSpeed;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _ballTexture = Content.Load<Texture2D>("ball");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ProcessTouch(TouchPanel.GetState(), gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
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
            if(!hasPress) { return; }

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
