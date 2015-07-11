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
    class SpeedPellet : Artifact
    {
        public SpeedPellet()
        {
        }

        public override void SetStartPosition()
        {
            position = new Vector2(new Random().Next(Convert.ToInt32(_minX), Convert.ToInt32(_maxX)), new Random().Next(Convert.ToInt32(_minY), Convert.ToInt32(_maxY)));
        }
    }
}
