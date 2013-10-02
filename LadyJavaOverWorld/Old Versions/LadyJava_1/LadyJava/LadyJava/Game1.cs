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
        int[,] tileMap = new int[,]
        {
            {1,2,1,3,1,2,0,0,0,0,0,0,0,0,0,0,0,0,00,00,00,00,00,09,05,05,08,01,03,01},//1       0 = Grass
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,00,00,00,00,00,09,05,05,08,00,00,03},//        1 = Tree #1
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,00,00,00,00,17,13,05,05,12,16,00,01},//        2 = Tree #2
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,00,00,00,17,13,05,05,05,05,08,00,02},//        3 = Tree #3
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,00,00,00,09,05,05,05,05,05,08,00,01},//        4 = Road
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,00,00,00,09,05,05,05,05,05,08,00,03},//5       5 = Water
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,00,00,00,15,11,05,05,05,10,14,00,02},//       6 = Sand Down
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,00,00,00,00,15,07,07,07,14,00,00,01},//        7 = Sand Up
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,3},//        8 = Sand Left
            {2,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,2},//        9 = Sand Right
            {3,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,1},//10      10 = Sand Up Left
            {2,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,2},//        11 = Sand Up Right
            {1,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,3},//        12 = Sand DL
            {3,0,0,0,0,0,0,0,0,0,0,4,4,4,0,0,4,4,4,0,0,0,0,0,0,0,0,0,0,2},//        13 = Sand DR
            {2,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,1},//        14 = SSquare TL
            {3,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,2},//15      15 = SSquare TR
            {2,0,0,0,0,0,0,0,0,0,0,4,4,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0,2},//        16 = SSquare BL
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,1},//        17 = SSquare BR
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,3},//
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,2},//
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,1},//20
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,3},//
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,2},//
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,1},//
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,2},//25
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,1},//
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,2},//
            {3,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,3},//
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0,0,0,0,0,0,0,0,0,0,1},//
            {1,3,1,3,2,1,3,1,3,2,3,1,2,3,4,4,2,1,2,3,1,2,3,1,3,2,1,2,1,3},//30
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            ladyJ.Update(gameTime, lady);

            #region Moving Camera
            KeyboardState keyState = Keyboard.GetState();
            Vector2 motion = Vector2.Zero;

            if (keyState.IsKeyDown(Keys.Up)) { cameraPosition.Y -= 5; } //UP
            if (keyState.IsKeyDown(Keys.W)) { cameraPosition.Y -= 5; }  //UP
            if (keyState.IsKeyDown(Keys.Down)) { cameraPosition.Y += 5; }  //Down
            if (keyState.IsKeyDown(Keys.S)) { cameraPosition.Y += 5; }  //Down
            if (keyState.IsKeyDown(Keys.Left)) { cameraPosition.X -= 5; } //Left
            if (keyState.IsKeyDown(Keys.A)) { cameraPosition.X -= 5; } //Left
            if (keyState.IsKeyDown(Keys.Right)) { cameraPosition.X += 5; } //Right
            if (keyState.IsKeyDown(Keys.D)) { cameraPosition.X += 5; } //Right

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                cameraPosition += motion * cameraSpeed;
            }

            if (cameraPosition.X < 0) { cameraPosition.X = 0; } //Keeps camera from moving off the
            if (cameraPosition.Y < 0) { cameraPosition.Y = 0; } //top left corner

            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;

            int tileMapWidth = tileMap.GetLength(1) * tileWidth;
            int tileMapHeight = tileMap.GetLength(0) * tileHeight;

            if (cameraPosition.X > tileMapWidth - screenWidth)
            { cameraPosition.X = tileMapWidth - screenWidth; }
            if (cameraPosition.Y > tileMapHeight - screenHeight)
            { cameraPosition.Y = tileMapHeight - screenHeight; }
            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            #region DrawTile
            int tileMapWidth = tileMap.GetLength(1);
            int tileMapHeight = tileMap.GetLength(0);

            for (int x = 0; x < tileMapWidth; x++)
            {
                for (int y = 0; y < tileMapHeight; y++)
                {
                    int textureIndex = tileMap[y, x];
                    Texture2D texture = tileTexture[textureIndex];
                    spriteBatch.Draw(texture, new Rectangle(
                        x * tileWidth - (int)cameraPosition.X,
                        y * tileHeight - (int)cameraPosition.Y, //casting as int (rounding)
                        tileWidth, tileHeight), Color.White);
                }
            }
            #endregion
            ladyJ.Draw(lady, spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
