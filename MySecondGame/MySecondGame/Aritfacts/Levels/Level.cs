using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MySecondGame.Aritfacts.Levels
{
    public class Level : Sprite
    {
        public string Name;
        //public string LEVEL_ASSET_NAME;
        public Dictionary<String, Exit> Exits;
        public Boolean Default;
        public int destinationX;
        public int destinationY;

        const int START_POSITION_X = 0;
        const int START_POSITION_Y = 0;
        const int WIDTH = 1280;
        const int HEIGHT = 720;

        public Level()
        {
            spriteHeight = HEIGHT;
            spriteWidth = WIDTH;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, AssetName);
        }

    }
}
