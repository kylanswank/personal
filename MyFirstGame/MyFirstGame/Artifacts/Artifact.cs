using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyFirstGame.Artifacts
{

    class Artifact
    {
        private Vector2 _position;
        public Vector2 position { get { return _position; } set { _position = value; } }

        protected Texture2D _currentTexture;
        public Texture2D currentTexture 
        { 
            get { return _currentTexture; }
            set 
            { 
                _currentTexture = value;
            } 
        }

        public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        private float _speedMultiplier = 1.0f;
        public float speedMultiplier 
        { 
            get { return _speedMultiplier; } 
            set { _speedMultiplier = value; }
        }

        private float _baseSpeed;
        public float baseSpeed 
        { 
            get { return _baseSpeed; }
            set { _baseSpeed = value; }
        }

        public GraphicsDeviceManager graphics;

        public float _minX;
        public float _maxX;
        public float _minY;
        public float _maxY;

        public float totalSpeed
        {
            get { return baseSpeed * speedMultiplier; }
        }
        
        public Artifact()
        {

        }

        public void setBoundaries()
        {
            _maxX = graphics.GraphicsDevice.Viewport.Width - currentTexture.Width;
            _minX = 0;
            _maxY = graphics.GraphicsDevice.Viewport.Height - currentTexture.Height;
            _minY = 0;
        }
            
        public void moveLeft(float distance)
        {
            _position.X -= distance;

            if (_position.X < _minX)
                _position.X = _minX;

        }

        public void moveRight(float distance)
        {
            _position.X += distance;

            if (_position.X > _maxX)
                _position.X = _maxX;

        }

        public void moveUp(float distance)
        {
            _position.Y -= distance;

            if (_position.Y < _minY)
                _position.Y = _minY;

        }

        public void moveDown(float distance)
        {
            _position.Y += distance;

            if (_position.Y > _maxY)
                _position.Y = _maxY;

        }

        public Boolean CheckCollision(Artifact a)
        {
            Rectangle r1 = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), currentTexture.Bounds.Width, currentTexture.Bounds.Height);
            Rectangle r2 = new Rectangle(Convert.ToInt32(a.position.X), Convert.ToInt32(a.position.Y), a.currentTexture.Bounds.Width, a.currentTexture.Bounds.Height);

            return r1.Intersects(r2);
        }

        public Boolean CheckCollision(SpeedPellet a)
        {
            Rectangle r1 = new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), currentTexture.Bounds.Width, currentTexture.Bounds.Height);
            Rectangle r2 = new Rectangle(Convert.ToInt32(a.position.X), Convert.ToInt32(a.position.Y), a.currentTexture.Bounds.Width, a.currentTexture.Bounds.Height);

            return r1.Intersects(r2);
        }

        public virtual void SetStartPosition() 
        {
        }

    }
}
