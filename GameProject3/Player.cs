using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using GameProject1.Collisions;

namespace FinalProject
{
    /// <summary>
    /// a class representing the player's car
    /// </summary>
    public class Player
    {
        // the game this car is a part of
        private Microsoft.Xna.Framework.Game _game;

        // the texture atlas for the car sprite
        private Texture2D _texture;

        // the bounds of the car within the texture atlas
        private Rectangle _carBounds = new Rectangle(996, 74, 23, 52);

        // the position of the player
        private Vector2 _position = new Vector2(188, 500);

        /// <summary>
        /// the current position of the player
        /// </summary>
        public Vector2 Position => _position;

        /// <summary>
        /// whether a crash has occurred
        /// </summary>
        public bool Crash = false;

        private BoundingRectangle _bounds = new BoundingRectangle(new Vector2(188,500), 23, 52);

        /// <summary>
        /// the bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// creates a new player sprite
        /// </summary>
        /// <param name="game">the game this player is a part of</param>
        public Player(Microsoft.Xna.Framework.Game game)
        {
            _game = game;
        }

        /// <summary>
        /// loads the player texture atlas
        /// </summary>
        /// <param name="content">the content manager to use to load the content</param>
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("cars");
        }

        /// <summary>
        /// updates the player
        /// </summary>
        /// <param name="gameTime">an object representing time in the game</param>
        public void Update(GameTime gameTime)
        {
            if (!Crash)
            {
                var keyboardState = Keyboard.GetState();
                float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //_position -= Vector2.UnitY * 100 * t;

                if (_position.X > 0 && keyboardState.IsKeyDown(Keys.Left)) _position -= Vector2.UnitX * 100 * t;

                if (_position.X <= _game.GraphicsDevice.Viewport.Width - 23 && keyboardState.IsKeyDown(Keys.Right)) _position += Vector2.UnitX * 100 * t;

                _bounds.X = _position.X;
                _bounds.Y = _position.Y;
            }
        }

        /// <summary>
        /// draws the player sprite
        /// </summary>
        /// <param name="gameTime">an object representing the time in the game</param>
        /// <param name="spriteBatch">the sprite batch to draw the player with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _carBounds, Color.White);
        }
    }
}
