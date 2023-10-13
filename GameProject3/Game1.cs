using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameProject3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Player _player;
        private Car[] _cars;

        ExplosionParticleSystem _explosions;

        private bool _shaking;
        private float _shakeTime;

        /// <summary>
        /// constructs the game
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 400;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// initializes the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            System.Random rand = new System.Random();
            _cars = new Car[]
            {
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -75), Color.Red),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -225), Color.Blue),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -375), Color.Orange),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -525), Color.Purple),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -675), Color.Yellow),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -825), Color.Green),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -975), Color.Pink),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -1125), Color.White),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -1275), Color.Brown),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -150), Color.Yellow),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -300), Color.Green),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -450), Color.White),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -600), Color.Brown),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -750), Color.Red),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -900), Color.Purple),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -1050), Color.Blue),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -1200), Color.Orange),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -1350), Color.Silver)
            };
            _player = new Player(this);

            _explosions = new ExplosionParticleSystem(this, 20);
            Components.Add(_explosions);

            base.Initialize();
        }

        /// <summary>
        /// loads game content
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var car in _cars) car.LoadContent(Content);
            _player.LoadContent(Content);
        }
        
        /// <summary>
        /// updates the game world
        /// </summary>
        /// <param name="gameTime">an object representing time in the game</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (var car in _cars)
            {
                car.Update(gameTime);
                if (car.Bounds.CollidesWith(_player.Bounds))
                {
                    car.Crash = true;
                    _player.Crash = true;
                    foreach (var car2 in _cars) car2.CrashHasOccured = true;
                    _shaking = true;
                    _explosions.PlaceExplosion(new Vector2(_player.Position.X + 12, _player.Position.Y + 20));
                }
            }
            _player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// draws the game world
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Matrix shakeTransform = Matrix.Identity;
            if (_shaking)
            {
                _shakeTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                // Matrix shakeRotation = Matrix.CreateRotationZ(MathF.Cos(_shakeTime));
                Matrix shakeTranslation = Matrix.CreateTranslation(10 * MathF.Sin(_shakeTime), 10 * MathF.Cos(_shakeTime), 0);
                shakeTransform = shakeTranslation;
                if (_shakeTime > 500) _shaking = false;
            }

            _spriteBatch.Begin(transformMatrix: shakeTransform);
            _player.Draw(gameTime, _spriteBatch);
            foreach (var car in _cars) car.Draw(gameTime, _spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}