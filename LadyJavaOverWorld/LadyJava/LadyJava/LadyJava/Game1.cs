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
using System.IO;

namespace LadyJava
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LadyJava ladyJ;
        Npc npcAmy;
        Rectangle rectAmy;
        Camera camera;
        

#region Background and Camera
        List<Texture2D> tileTexture = new List<Texture2D>();
        int[][,] tileMap;// = new int[30][,];
        
        int tileWidth = 64;
        int tileHeight = 64;
        //Vector2 cameraPosition = Vector2.Zero; //Hold X & Y Value
        //float cameraSpeed = 5; // Used to stop double speed when down and left are pressed.
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
        Texture2D[] npcImage = new Texture2D[1];



        
        Sprite amy;
        int STILL = 0 , DOWN = 1, LEFT=2, RIGHT=3, UP=4;
        protected override void LoadContent()
        {
            camera = new Camera(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
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
            LoadTileMap("..\\..\\..\\..\\..\\overworld.txt");

            int[,] animations = { { STILL, 1, 0 }, { DOWN, 4, 100 }, { LEFT, 4, 100 }, { RIGHT, 4, 100 }, { UP, 4, 100 } };
            Sprite lady = new Sprite(image, new Vector2(0, 0), 32, 46, animations, 1.0f);
            //LadyJava.sides(Content.Load<Texture2D>(@"Sprites\LadyJavaBigOverWorld"), 4, 4);
            ladyJ = new LadyJava(lady);

            //create a npc
           
            
            
            npcImage[0] = Content.Load<Texture2D>("Sprites\\NPI\\Sprites\\Amy");
            amy = new Sprite(npcImage, new Vector2(200, 200), 38, 73, 1.0f);
            npcAmy = new Npc(amy);
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
            int tileMapWidth = tileMap[0].GetLength(0) * tileWidth;
            int tileMapHeight = tileMap.GetLength(0) * tileHeight;
            #endregion

            // TODO: Add your update logic here
            ladyJ.Update(gameTime);
            camera.Update(gameTime, ladyJ.Position, ladyJ.Origin, tileMapWidth, tileMapHeight);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //spriteBatch.Begin();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

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
                            x * tileWidth,
                            y * tileHeight, //casting as int (rounding)
                            tileWidth, tileHeight), Color.White);
                        }
                    }
                }
            }
            #endregion
            ladyJ.Draw(spriteBatch);
            //npcAmy.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadTileMap(String fileLocation)
        {
            int tileMapWidth = 0; 
            int tileMapHeigth = 0;
            int layersCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fileLocation))
                {
                    string lines = sr.ReadToEnd();
                    string[] line = lines.Split(new Char[] { '\n' });

                    for (int y = 0; y < line.Length; y++)
                    {



                        if (y == 0)
                        {
                            string[] dimensions = line[y].Trim().Split(new Char[] { 'x' });

                            tileMapWidth = int.Parse(dimensions[0]);
                            tileMapHeigth = int.Parse(dimensions[1]);
                            layersCount = int.Parse(dimensions[2]);
                            tileMap = new int[tileMapHeigth][,];
                        }
                        else
                        {
                            tileMap[y - 1] = new int[tileMapWidth, layersCount];
                            string[] tiles = line[y].Trim().Split(new Char[] { '|' });

                            for (int i = 0; i < tiles.Length; i++)
                            {
                                string[] layers = tiles[i].Split(new Char[] { ',' });

                                for (int j = 0; j < layers.Length; j++)
                                {
                                    tileMap[y - 1][i, j] = int.Parse(layers[j]);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }

}
