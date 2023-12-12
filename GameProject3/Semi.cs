using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using GameProject1.Collisions;

namespace FinalProject
{
    /// <summary>
    /// A class representing a semi
    /// </summary>
    public class Semi
    {
        // the game this semi is a part of
        private Game _game;

        // the texture atlas for the semi sprite
        private Texture2D _texture;

        // the bounds of the semi within the texture atlas
        private Rectangle _carBounds = new Rectangle(196, 58, 34, 67);

        // the position of the semi
        private Vector2 _position;

        // whether the crash is over
        private bool _crashed = false;

        private BoundingRectangle _bounds;

        /// <summary>
        /// the bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => _bounds;

        /// <summary>
        /// whether this semi crashed
        /// </summary>
        public bool Crash = false;

        /// <summary>
        /// whether a crash has occured
        /// </summary>
        public bool CrashHasOccured = false;

        /// <summary>
        /// creates a new semi sprite
        /// </summary>
        /// <param name="position">the position of the sprite in the game</param>
        public Semi(Game game, Vector2 position)
        {
            _game = game;
            _position = position;
            _bounds = new BoundingRectangle(_position, 34, 67);
        }

        /// <summary>
        /// loads the semi texture atlas
        /// </summary>
        /// <param name="content">the content manager used to load the content</param>
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("cars");
        }

        /// <summary>
        /// updates the semi
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
                            Random rand = new Random();
                            _position = new Vector2((float)rand.NextDouble() * _game.GraphicsDevice.Viewport.Width - 17, -360);
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
                    float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    _position -= Vector2.UnitY * 150 * t;
                }
            }

            _bounds.X = _position.X;
            _bounds.Y = _position.Y;
        }

        /// <summary>
        /// draws the semi sprite
        /// </summary>
        /// <param name="gameTime">an object representing time in the game</param>
        /// <param name="spriteBatch">the sprite batch to draw the semi with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _carBounds, Color.White);
        }
    }
}
