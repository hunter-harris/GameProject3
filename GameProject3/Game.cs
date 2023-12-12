using FinalProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FinalProject
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private Player _player;
        private Car[] _cars;
        private Semi[] _semis;
        private Truck[] _trucks;

        private SpriteFont _text;

        ExplosionParticleSystem _explosions;

        private bool _shaking;
        private float _shakeTime;

        private string _time;

        /// <summary>
        /// constructs the game
        /// </summary>
        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 250;
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
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -150), Color.Yellow),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -225), Color.Orange),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -399), Color.Red),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -474), Color.Yellow),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -549), Color.Orange),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -723), Color.Red),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -798), Color.Yellow),
                new Car(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 12, -873), Color.Orange)
            };
            _semis = new Semi[]
            {
                new Semi(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 17, -324)),
                new Semi(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 17, -648))
            };
            _trucks = new Truck[]
            {
                new Truck(this, new Vector2((float)rand.NextDouble() * GraphicsDevice.Viewport.Width - 28, -1022))
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
            foreach (var semi in _semis) semi.LoadContent(Content);
            foreach (var truck in _trucks) truck.LoadContent(Content);
            _player.LoadContent(Content);
            _text = Content.Load<SpriteFont>("arial");
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
                    foreach (var semi in _semis) semi.CrashHasOccured = true;
                    foreach (var truck in _trucks) truck.CrashHasOccured = true;
                    _shaking = true;
                    _explosions.PlaceExplosion(new Vector2(_player.Position.X + 12, _player.Position.Y + 20));
                }
            }
            foreach (var semi in _semis)
            {
                semi.Update(gameTime);
                if (semi.Bounds.CollidesWith(_player.Bounds))
                {
                    semi.Crash = true;
                    _player.Crash = true;
                    foreach (var semi2 in _semis) semi2.CrashHasOccured = true;
                    foreach (var car in _cars) car.CrashHasOccured = true;
                    foreach (var truck in _trucks) truck.CrashHasOccured = true;
                    _shaking = true;
                    _explosions.PlaceExplosion(new Vector2(_player.Position.X + 12, _player.Position.Y + 20));
                }
            }
            foreach (var truck in _trucks)
            {
                truck.Update(gameTime);
                if (truck.Bounds.CollidesWith(_player.Bounds))
                {
                    truck.Crash = true;
                    _player.Crash = true;
                    foreach (var truck2 in _trucks) truck2.CrashHasOccured = true;
                    foreach (var car in _cars) car.CrashHasOccured = true;
                    foreach (var semi in _semis) semi.CrashHasOccured = true;
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
            GraphicsDevice.Clear(Color.DimGray);

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
            foreach (var semi in _semis) semi.Draw(gameTime, _spriteBatch);
            foreach (var truck in _trucks) truck.Draw(gameTime, _spriteBatch);

            if (!_player.Crash)
            {
                _spriteBatch.DrawString(_text, $"{gameTime.TotalGameTime.TotalSeconds}", new Vector2(2, 570), Color.Black);
                _time = $"{gameTime.TotalGameTime.TotalSeconds}";
            }
            else
            {
                _spriteBatch.DrawString(_text, _time, new Vector2(2, 570), Color.Black);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}