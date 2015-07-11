using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MySecondGame.GraphicsLogic;

namespace MySecondGame.Aritfacts.Characters
{
    class NPC : Sprite
    {
        const string PLAYER_ASSET_NAME = "player";
        const int PLAYER_SPEED = 25;

        enum State
        {
            Walking
        }

        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        public int steps = 0;
        int direction = 1;
        

        public void LoadContent(ContentManager theContentManager)
        {
            Position = new Vector2(100, 600);
            base.LoadContent(theContentManager, PLAYER_ASSET_NAME);
        }

        public void Update(GameTime theGameTime)
        {
            if (steps == 0)
            {
                steps = new Random().Next(10, 20);
                direction = new Random(DateTime.Now.Second).Next(1, 5);
            }

            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (direction == 1)
                {
                    spriteIndex = 0;
                    mSpeed.Y = PLAYER_SPEED;
                    mDirection.Y = -1;
                }
                else if (direction == 2)
                {
                    spriteIndex = 1;
                    mSpeed.Y = PLAYER_SPEED;
                    mDirection.Y = 1;
                }
                else if (direction == 3)
                {
                    spriteIndex = 2;
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = -1;
                }
                else if (direction == 4)
                {
                    spriteIndex = 3;
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = 1;
                }

                steps--;
            }

            base.Update(theGameTime, mSpeed, mDirection);
        }
    }
}
