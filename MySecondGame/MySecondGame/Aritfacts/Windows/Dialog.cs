using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MySecondGame.GraphicsLogic;

namespace MySecondGame.Aritfacts.Windows
{
    public class Dialog: Sprite
    {
        public bool show = false;
        Texture2D texture;
        SpriteFont Font1;

        int left = 200;
        int top = 50;
        int width = 200;
        int height = 100;

        public void LoadContent(ContentManager theContentManager, GraphicsDevice g)
        {
            Position = new Vector2(left, top);

            texture = new Texture2D(g, width, height);
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < width * height; i++)
                colorData[i] = Color.White;
            texture.SetData<Color>(colorData);

            Font1 = theContentManager.Load<SpriteFont>("Courier New");
        }

        public void Draw(SpriteBatch theSpriteBatch, String text)
        {
            
            if (show)
            {
                theSpriteBatch.Draw(texture, new Rectangle(left, top, width, height), Color.Gray);

                theSpriteBatch.DrawString(Font1, text, new Vector2(left+20, top+20), Color.Black);
            }
        }        

    }
}
