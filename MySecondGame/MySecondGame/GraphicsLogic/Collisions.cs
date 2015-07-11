using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MySecondGame.Aritfacts;
using MySecondGame.Aritfacts.Characters;
using MySecondGame.Aritfacts.Levels;

namespace MySecondGame.GraphicsLogic
{
    class Collisions
    {

        public static Color IntersectPixelsPlayerBackground(Sprite player, Sprite background)
        {
            Rectangle recA = new Rectangle(Convert.ToInt32(player.Position.X), Convert.ToInt32(player.Position.Y), player.spriteWidth, player.SpriteTexture.Height);
            Rectangle recB = new Rectangle(Convert.ToInt32(background.Position.X), Convert.ToInt32(background.Position.Y), background.SpriteTexture.Width, background.SpriteTexture.Height);
            Color[] colorA = new Color[player.SpriteTexture.Width * player.SpriteTexture.Height];
            Color[] colorB = new Color[background.SpriteTexture.Width * background.SpriteTexture.Height];

            player.SpriteTexture.GetData(colorA);
            background.SpriteTexture.GetData(colorB);
            
            // Check every point within the intersection bounds
            for (int i = recA.Top - Convert.ToInt32(player.Position.Y); i < recA.Bottom - Convert.ToInt32(player.Position.Y); i += 1)
                for (int j = recA.Left - Convert.ToInt32(player.Position.X); j < recA.Right - Convert.ToInt32(player.Position.X); j += 1)
                {
                    // Get the color of player pixels at this point
                    Color colorA1 = colorA[i * recA.Width + j];

                    if (colorA1.A != 0)
                    {
                        Color colorB1 = colorB[(i + Convert.ToInt32(player.Position.Y)) * recB.Width + (j + Convert.ToInt32(player.Position.X))];

                        if (colorB1.A != 0)
                            return colorB1;
                    }

                }

            // No intersection found
            return Color.White;
        }

        public static bool IntersectPixelsNPCBackground(Sprite player, Sprite background)
        {
            Rectangle recA = new Rectangle(Convert.ToInt32(player.Position.X), Convert.ToInt32(player.Position.Y), player.spriteWidth, player.spriteHeight);
            Rectangle recB = new Rectangle(Convert.ToInt32(background.Position.X), Convert.ToInt32(background.Position.Y), background.spriteWidth, background.spriteHeight);
            Color[] colorA = new Color[player.SpriteTexture.Width * player.SpriteTexture.Height];
            Color[] colorB = new Color[background.SpriteTexture.Width * background.SpriteTexture.Height];

            player.SpriteTexture.GetData(colorA);
            background.SpriteTexture.GetData(colorB);

            // Check every point within the intersection bounds
            for (int i = recA.Top - Convert.ToInt32(player.Position.Y); i < recA.Bottom - Convert.ToInt32(player.Position.Y); i += player.spriteHeight / 4)
                for (int j = recA.Left - Convert.ToInt32(player.Position.X); j < recA.Right - Convert.ToInt32(player.Position.X); j += player.spriteWidth / 4)
                {
                    try
                    {
                        Color colorB1 = colorB[(i + Convert.ToInt32(player.Position.Y)) * recB.Width + (j + Convert.ToInt32(player.Position.X))];

                        if (colorB1.A != 0)
                            return true;
                    }
                    catch
                    {

                    }
                    
                }

            // No intersection found
            return false;
        }

        public static bool IntersectPlayerNPC(Player player, NPC npc)
        {
            Rectangle recA = new Rectangle(Convert.ToInt32(player.Position.X), Convert.ToInt32(player.Position.Y), player.spriteWidth, player.spriteHeight);
            Rectangle recB = new Rectangle(Convert.ToInt32(npc.Position.X), Convert.ToInt32(npc.Position.Y), npc.spriteWidth, npc.spriteHeight);

            return recA.Intersects(recB);
        }
    }
}
