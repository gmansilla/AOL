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

namespace LadyJava
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LadyJava ladyJ;

#region Background and Camera
        List<Texture2D> tileTexture = new List<Texture2D>();
        int[][,] tileMap = new int[][,]
        { new int[,] { {00,01}, {00,02}, {00,01}, {00,03}, {00,01}, {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,09}, {-1,05}, {-1,05}, {08,03}, {00,01}, {00,03}, {00,01} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,09}, {-1,05}, {-1,05}, {-1,08}, {-1,00}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,17}, {-1,13}, {-1,05}, {-1,05}, {-1,12}, {-1,16}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,01}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,17}, {-1,13}, {-1,05}, {-1,05}, {-1,05}, {-1,05}, {-1,08}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,09}, {-1,05}, {-1,05}, {-1,05}, {-1,05}, {-1,05}, {-1,08}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,09}, {-1,05}, {-1,05}, {-1,05}, {-1,05}, {-1,05}, {-1,08}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,15}, {-1,11}, {-1,05}, {-1,05}, {-1,05}, {-1,10}, {-1,14}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,15}, {-1,07}, {-1,07}, {-1,07}, {-1,14}, {-1,00}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,01}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,01}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,01}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,01}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,02}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,01} },//1       0 = Grass
          new int[,] { {00,01}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,02} },//1       0 = Grass
          new int[,] { {00,03}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,04}, {-1,04}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {-1,00}, {00,03} },//1       0 = Grass
          new int[,] { {00,01}, {00,03}, {00,01}, {00,03}, {00,02}, {00,01}, {00,03}, {00,01}, {00,03}, {00,02}, {00,03}, {00,01}, {00,02}, {00,03}, {-1,04}, {-1,04}, {00,02}, {00,01}, {00,02}, {00,03}, {00,01}, {00,02}, {00,03}, {00,01}, {00,03}, {00,02}, {00,01}, {00,02}, {00,01}, {00,03} }//1       0 = Grass
        };
        int tileWidth = 64;
        int tileHeight = 64;
        Vector2 cameraPosition = Vector2.Zero; //Hold X & Y Value
        float cameraSpeed = 5; // Used to stop double speed when down and left are pressed.
#endregion
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        Texture2D[] image = new Texture2D[1];
        Sprite lady;
        int STILL = 0 , DOWN = 1, LEFT=2, RIGHT=3, UP=4;
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            #region Add Tiles
            Texture2D texture;
            texture = Content.Load<Texture2D>("Background\\Grass"); //0 = Grass
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Tree1"); //1 = Tree 1
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Tree2"); //2 = Tree 2
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Tree3"); //3 = Tree 3
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Road"); //4 = Road
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Water"); //5 = Water
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandDown"); //6 = Sand D
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandUp"); //7 = Sand U
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandLeft"); //8 = Sand L
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandRight"); //9 = Sand R
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandUpLeft"); //10 = Sand UL
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandUpRight"); //11 = Sand UR
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandDownLeft"); //12 = Sand DL
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandDownRight"); //13 = Sand DR
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandSquareTL"); //14 = SSquare TL
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandSquareTR"); //15 = SSquare TR
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandSquareBL"); //16 = SSquare BL
            tileTexture.Add(texture);
            texture = Content.Load<Texture2D>("Background\\Sand\\SandSquareBR"); //17 = SSquare BR
            tileTexture.Add(texture);
            #endregion

            image[0] = Content.Load<Texture2D>("Sprites\\LadyJavaBigOverWorld");

            int[,] animations = { { STILL, 1, 0 }, { DOWN, 4, 100 }, { LEFT, 4, 100 }, { RIGHT, 4, 100 }, { UP, 4, 100 } };
            lady = new Sprite(image, new Vector2(0, 0), 32, 46, animations, 1.0f);
            //LadyJava.sides(Content.Load<Texture2D>(@"Sprites\LadyJavaBigOverWorld"), 4, 4);
            ladyJ = new LadyJava();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();


            #region Moving Camera
            KeyboardState keyState = Keyboard.GetState();
            Vector2 motion = Vector2.Zero;
            float camMovement = 2.4f;

            if (keyState.IsKeyDown(Keys.Up)) { cameraPosition.Y -= camMovement; } //UP
            if (keyState.IsKeyDown(Keys.W)) { cameraPosition.Y -= camMovement; }  //UP
            if (keyState.IsKeyDown(Keys.Down)) { cameraPosition.Y += camMovement; }  //Down
            if (keyState.IsKeyDown(Keys.S)) { cameraPosition.Y += camMovement; }  //Down
            if (keyState.IsKeyDown(Keys.Left)) { cameraPosition.X -= camMovement; } //Left
            if (keyState.IsKeyDown(Keys.A)) { cameraPosition.X -= camMovement; } //Left
            if (keyState.IsKeyDown(Keys.Right)) { cameraPosition.X += camMovement; } //Right
            if (keyState.IsKeyDown(Keys.D)) { cameraPosition.X += camMovement; } //Right

            /*if (motion != Vector2.Zero)
            {
                motion.Normalize();
                cameraPosition += motion * cameraSpeed;
            }
            */
            if (cameraPosition.X < 0) { cameraPosition.X = 0; } //Keeps camera from moving off the
            if (cameraPosition.Y < 0) { cameraPosition.Y = 0; } //top left corner

            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            int tileMapWidth = tileMap[0].GetLength(0) * tileWidth;
            int tileMapHeight = tileMap.GetLength(0) * tileHeight;

            if (cameraPosition.X > tileMapWidth - screenWidth)
            { 
                cameraPosition.X = tileMapWidth - screenWidth; 
                
            }
            if (cameraPosition.Y > tileMapHeight - screenHeight)
            { cameraPosition.Y = tileMapHeight - screenHeight; }
            #endregion

            // TODO: Add your update logic here
            ladyJ.Update(gameTime, lady, cameraPosition);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            #region DrawTile
            int tileMapWidth = tileMap[0].GetLength(0);
            int tileMapHeight = tileMap.GetLength(0);

            for (int y = 0; y < tileMapHeight; y++)
            {
                for (int x = 0; x < tileMapWidth; x++)
                {
                    for (int z = 0; z < tileMap[y].GetLength(1); z++)
                    {
                        int textureIndex = tileMap[y][x, z];
                        if (textureIndex != -1)
                        {
                            Texture2D texture = tileTexture[textureIndex];
                            spriteBatch.Draw(texture, new Rectangle(
                            x * tileWidth - (int)cameraPosition.X,
                            y * tileHeight - (int)cameraPosition.Y, //casting as int (rounding)
                            tileWidth, tileHeight), Color.White);
                        }
                    }
                }
            }
            #endregion
            ladyJ.Draw(lady, spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
