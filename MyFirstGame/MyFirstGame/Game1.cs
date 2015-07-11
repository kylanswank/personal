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
using MyFirstGame.Artifacts;

namespace MyFirstGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D textureCollisionMap;

        //Texture2D myTexture;
        //Texture2D textureSpeedPellet;

        List<Artifact> activeArtifacts;
        Artifact player;
        Artifact speedPellet;

        //Vector2 spritePosition = Vector2.Zero;
        //Vector2 positionSpeedPellet = Vector2.Zero;

        //Vector2 spriteSpeed = new Vector2(200f, -200f);
        //int speed = 2;
        //int speedMultiplier = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;

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


            textureCollisionMap = Content.Load<Texture2D>("LevelCollisionMap");

            activeArtifacts = new List<Artifact>();

            player = CreatePlayer();
            activeArtifacts.Add(player);

            speedPellet = CreateSpeedPellet();
            activeArtifacts.Add(speedPellet);
            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
             
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            bool catCanMove;
            Vector2 oldPosition = player.position;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                player.moveLeft(player.totalSpeed);
            }
                

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                player.moveRight(player.totalSpeed);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                player.moveUp(player.totalSpeed);
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                player.moveDown(player.totalSpeed);
            }
            
            //Check for Collisions

            //Field
            catCanMove = CatIsOnField(player.position, player.currentTexture, textureCollisionMap);
            
            if (!catCanMove)
               player.position = oldPosition;


            //Player with speed pellet
            if (player.CheckCollision(speedPellet))
            {
                activeArtifacts.Remove(speedPellet);
                speedPellet = CreateSpeedPellet();
                activeArtifacts.Add(speedPellet);
                player.baseSpeed += 1;
            }
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            
            foreach(Artifact a in activeArtifacts)
            {
                spriteBatch.Draw(a.currentTexture, a.position, Color.White);
            }
            spriteBatch.Draw(textureCollisionMap, Vector2.Zero, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
         
        void UpdateSprite(GameTime gameTime)
        {

        }

        Player CreatePlayer()
        {
            Player player = new Player(Vector2.Zero, 1);
            player.graphics = graphics;
            player.textures.Add("normal", Content.Load<Texture2D>("mytexture"));
            player.currentTexture = player.textures["normal"];
            player.setBoundaries();

            return player;
        }

        SpeedPellet CreateSpeedPellet()
        {
            speedPellet = new SpeedPellet();
            speedPellet.graphics = graphics;
            speedPellet.textures.Add("normal", Content.Load<Texture2D>("speedPellet"));
            speedPellet.currentTexture = speedPellet.textures["normal"];
            speedPellet.setBoundaries();

            do
            {
                speedPellet.SetStartPosition();
            } while (!SpeedPelletIsOnField(speedPellet.position, speedPellet.currentTexture, textureCollisionMap));
                
            
            return (SpeedPellet)speedPellet;
        }

        public static bool CatIsOnField(Vector2 positionCat, Texture2D TextureCat, Texture2D TextureField)
        {
            Rectangle RectangleCat = new Rectangle((int)positionCat.X, (int)positionCat.Y, TextureCat.Width, TextureCat.Height);


            Color[] TextureDataField = new Color[TextureField.Width * TextureField.Height];
            TextureField.GetData(TextureDataField);



            for (int i = RectangleCat.Top; i < RectangleCat.Bottom; i++)
                for (int j = RectangleCat.Left; j < RectangleCat.Right; j++)
                    if (TextureDataField[i * TextureField.Width + j] == Color.Black)
                        return false;



            return true;
        }

        public static bool SpeedPelletIsOnField(Vector2 positionCat, Texture2D TextureCat, Texture2D TextureField)
        {
            Rectangle RectanglePellet = new Rectangle((int)positionCat.X, (int)positionCat.Y, TextureCat.Width, TextureCat.Height);

            Color[] TextureDataField = new Color[TextureField.Width * TextureField.Height];
            TextureField.GetData(TextureDataField);

            for (int i = RectanglePellet.Top; i < RectanglePellet.Bottom; i+=2)
                for (int j = RectanglePellet.Left; j < RectanglePellet.Right; j+=2)
                    if (TextureDataField[i * TextureField.Width + j] == Color.Black)
                        return false;



            return true;
        }
    }
}
