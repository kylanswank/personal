using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MySecondGame.Aritfacts;
using MySecondGame.Aritfacts.Characters;
using MySecondGame.Aritfacts.Levels;
using MySecondGame.Aritfacts.Windows;
using MySecondGame.GraphicsLogic;

namespace MySecondGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public enum GameStatus
    {
        Explore,
        Dialog
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;
        GameStatus gameStatus = GameStatus.Explore;

        Player[] characters = {null};
        NPC[] npcs = new NPC[1];
        Dialog dialogWindow = new Dialog();

        Dictionary<string, Level> levels;
        Level currentLevel;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;

            this.IsFixedTimeStep = false;
            

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Player p1 = new Player();
            p1.spriteWidth = 25;
            p1.spriteHeight = 51;
            p1.spriteIndex = 1;
            characters[0] = p1;
            
            //Add npc
            NPC n1 = new NPC();
            n1.spriteWidth = 25;
            n1.spriteHeight = 51;
            n1.spriteIndex = 0;
            npcs[0] = n1;

            List<Level> ls = LevelHelper.GetLevels();

            levels = new Dictionary<string, Level>();

            foreach(Level l in ls)
            {
                l.LoadContent(Content);
                levels.Add(l.Name, l);

                if (l.Default)
                    currentLevel = l;
            }
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var pp = graphics.GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat);
            
            // TODO: use this.Content to load your game content here
            characters[0].LoadContent(this.Content);

            for(int i = 0; i< npcs.Length; i++)
            {
                npcs[i].LoadContent(this.Content);
            }

            currentLevel.LoadContent(this.Content);

            dialogWindow.LoadContent(this.Content, graphics.GraphicsDevice);

            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            currentLevel.Draw(this.spriteBatch);

            spriteBatch.End();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();




            if (gameStatus == GameStatus.Explore)
            {
                // TODO: Add your update logic here
                Player p = characters[0];
                Vector2 previousPostion = p.Position;

                p.Update(gameTime);

                dialogWindow.show = Collisions.IntersectPlayerNPC(p, npcs[0]);

                if (dialogWindow.show)
                {
                    p.Position = previousPostion;
                    gameStatus = GameStatus.Dialog;
                }

                bool changeScreen = false;

                if (p.Position.Y > graphics.GraphicsDevice.Viewport.Height - p.spriteHeight)
                {
                    try
                    {
                        Exit levelExit = currentLevel.Exits.Where(i => i.Key == "South").First().Value;
                        currentLevel = levels.Where(i => i.Value.AssetName == levelExit.destination).First().Value;
                        changeScreen = true;
                        p.Position.Y = 40;
                    }
                    catch
                    {

                    }

                }

                if (p.Position.Y < 0)
                {
                    try
                    {
                        Exit levelExit = currentLevel.Exits.Where(i => i.Key == "North").First().Value;
                        currentLevel = levels.Where(i => i.Value.AssetName == levelExit.destination).First().Value;
                        changeScreen = true;
                        p.Position.Y = graphics.GraphicsDevice.Viewport.Height - p.spriteHeight;
                    }
                    catch
                    {

                    }
                }

                if (p.Position.X > graphics.GraphicsDevice.Viewport.Width - p.spriteWidth)
                {
                    try
                    {

                        Exit levelExit = currentLevel.Exits.Where(i => i.Key == "East").First().Value;
                        currentLevel = levels.Where(i => i.Value.AssetName == levelExit.destination).First().Value;
                        changeScreen = true;
                        p.Position.Y = 40;
                    }
                    catch
                    {

                    }

                }

                if (p.Position.X < 0)
                {
                    try
                    {

                        Exit levelExit = currentLevel.Exits.Where(i => i.Key == "West").First().Value;
                        currentLevel = levels.Where(i => i.Value.AssetName == levelExit.destination).First().Value;
                        changeScreen = true;
                        p.Position.Y = graphics.GraphicsDevice.Viewport.Width - p.spriteWidth;
                    }
                    catch
                    {

                    }
                }


                Color retValue = Collisions.IntersectPixelsPlayerBackground(p, currentLevel);
                retValue.A = 255;
                if (retValue == Color.Black)
                    p.Position = previousPostion;
                else if (retValue != Color.White)
                {
                    try
                    {
                        Exit levelExit = currentLevel.Exits.Where(i => i.Key == String.Format("{0},{1},{2},{3}", retValue.R, retValue.G, retValue.B, retValue.A)).First().Value;
                        currentLevel = levels.Where(i => i.Value.AssetName == levelExit.destination).First().Value;
                        changeScreen = true;
                        if (levelExit.destinationX > -1)
                            p.Position.X = levelExit.destinationX;
                        if (levelExit.destinationY > -1)
                            p.Position.Y = levelExit.destinationY;
                    }
                    catch
                    {

                    }
                }

                if (DateTime.Now.Second % 2 == 0)
                {
                    for (int i = 0; i < npcs.Length; i++)
                    {
                        previousPostion = npcs[i].Position;

                        npcs[i].Update(gameTime);

                        if (npcs[i].Position.Y > graphics.GraphicsDevice.Viewport.Height - npcs[i].spriteHeight)
                        {
                            npcs[i].Position = previousPostion;
                        }

                        if (npcs[i].Position.Y < 0)
                        {
                            npcs[i].Position = previousPostion;
                        }

                        if (npcs[i].Position.X > graphics.GraphicsDevice.Viewport.Width - npcs[i].spriteWidth)
                        {
                            npcs[i].Position = previousPostion;
                        }

                        if (npcs[i].Position.X < 0)
                        {
                            npcs[i].Position = previousPostion;
                        }

                        bool boolRetValue = Collisions.IntersectPixelsNPCBackground(npcs[i], currentLevel);

                        if (boolRetValue)
                        {
                            npcs[i].Position = previousPostion;
                            npcs[i].steps = 0;
                        }


                    }
                }

                if (changeScreen)
                {
                    GraphicsDevice.SetRenderTarget(renderTarget);

                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    // TODO: Add your drawing code here
                    spriteBatch.Begin();
                    currentLevel.Draw(this.spriteBatch);

                    spriteBatch.End();
                }

            }
            

            if (gameStatus == GameStatus.Dialog)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    gameStatus = GameStatus.Explore;
                    dialogWindow.show = false;
                }

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(renderTarget, new Vector2(0, 0), Color.White);
            
            characters[0].Draw(this.spriteBatch);

            for (int i = 0; i < npcs.Length; i++)
            {
                npcs[i].Draw(this.spriteBatch);
            }

            dialogWindow.Draw(this.spriteBatch, "Hello!");
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
