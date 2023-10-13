using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using GameProject1.Collisions;

namespace GameProject3
{
    /// <summary>
    /// a class representing a car
    /// </summary>
    public class Car
    {
        // the game this car is a part of
        private Game _game;

        // the texture atlas for the car sprite
        private Texture2D _texture;

        // the bounds of the car within the texture atlas
        private Rectangle _carBounds = new Rectangle(912, 75, 23, 51);

        // the position of the car
        private Vector2 _position;

        // the color of the car
        private Color _color;

        // whether the crash is over
        private bool _crashed = false;

        private BoundingRectangle _bounds;

        /// <summary>
        /// the bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// whether this car crashed
        /// </summary>
        public bool Crash = false;

        /// <summary>
        /// whether a crash has occured
        /// </summary>
        public bool CrashHasOccured = false;

        /// <summary>
        /// creates a new car sprite
        /// </summary>
        /// <param name="position">the position of the sprite in the game</param>
        public Car(Game game, Vector2 position, Color color)
        {
            _game = game;
            _position = position;
            _color = color;
            _bounds = new BoundingRectangle(_position, 23, 51);
        }

        /// <summary>
        /// loads the car texture atlas
        /// </summary>
        /// <param name="content">the content manager used to load the content</param>
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("cars");
        }

        /// <summary>
        /// updates the car
        /// </summary>
        /// <param name="gameTime">an object representing time in the game</param>
        public void Update(GameTime gameTime)
        {
            if (!_crashed)
            {
                if (!Crash)
                {
                    if (!CrashHasOccured)
                    {
                        float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _position += Vector2.UnitY * 150 * t;

                        if (_position.Y > 650)
                        {
                            System.Random rand = new System.Random();
                            _position = new Vector2((float)rand.NextDouble() * _game.GraphicsDevice.Viewport.Width - 12, -700);
                        }
                    }
                    else
                    {
                        float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _position -= Vector2.UnitY * 150 * t;
                    }
                }
                else
                {
                    _position.Y -= 5;
                    _crashed = true;
                }
            }
                        
            _bounds.X = _position.X;
            _bounds.Y = _position.Y;
        }

        /// <summary>
        /// draws the car sprite
        /// </summary>
        /// <param name="gameTime">an object representing time in the game</param>
        /// <param name="spriteBatch">the sprite batch to draw the car with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _carBounds, _color);
        }
    }
}
