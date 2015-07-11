﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MySecondGame.Aritfacts
{
    public class Sprite
    {
        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        private Texture2D mSpriteTexture;
        public Texture2D SpriteTexture { get { return mSpriteTexture; } }
        private Rectangle rect = new Rectangle();  

        //The asset name for the Sprite's Texture
        public string AssetName;

        //The Size of the Sprite (with scale applied)
        public Rectangle Size;
        public int spriteWidth;
        public int spriteHeight;
        public int spriteIndex = 0;

        //The amount to increase/decrease the size of the original sprite. 
        private float mScale = 1.0f;

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Size = new Rectangle(0, 0, (int)(spriteWidth * Scale), (int)(spriteHeight * Scale));
        }

        //Draw the sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            rect.X = spriteWidth*spriteIndex;
            rect.Y = 0;
            rect.Width = spriteWidth;
            rect.Height = spriteHeight;

            theSpriteBatch.Draw(mSpriteTexture, Position,
                rect,
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        //When the scale is modified throught he property, the Size of the 
        //sprite is recalculated with the new scale applied.
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale
                Size = new Rectangle(0, 0, (int)(spriteWidth * Scale), (int)(spriteHeight * Scale));
            }
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
