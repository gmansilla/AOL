using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Engine;
using Microsoft.Xna.Framework;

namespace TileMapEditor
{
    using Image = System.Drawing.Image;
    using System.IO;

    public partial class frmTileMapEditor : Form
    {
        string[] imageExtensions = new string[] { ".jpg", ".png", ".tga" };
        
        Camera camera;
        TileMap tileMap;
        SpriteBatch spriteBatch;
        Texture2D cursor;
        Texture2D emptyTile;

        Dictionary<string, TileLayer> tileLayerList = new Dictionary<string, TileLayer>();
        Dictionary<string, Texture2D> textureList = new Dictionary<string, Texture2D>();
        Dictionary<string, Image> previewList = new Dictionary<string, Image>();

        int cellX;
        int cellY;
        int currentLayerIndex = -1;
        int tempTextureIndex = -1;
        int currentTextureIndex = -1;

        TileLayer currentLayer
        { get { return tileMap.Layers[currentLayerIndex]; } }

        GraphicsDevice graphicsDevice
        { get { return tileDisplay1.GraphicsDevice; } }

        public frmTileMapEditor()
        {
            InitializeComponent();

            //tdTileMap.OnInitialize += new EventHandler(tdTileMap_OnInitialize);
            //tdTileMap.OnDraw += new EventHandler(tdTileMap_OnDraw);
            tileDisplay1.OnInitializeAdd(new EventHandler(tdTileMap_OnInitialize));
            tileDisplay1.OnDrawAdd(new EventHandler(tdTileMap_OnDraw));
            
            openFileDialog1.Filter = "Tile Map FIle|*.map";
            openFileDialog1.FileName = "*.map";
            saveFileDialog1.Filter = "Tile Map FIle|*.map";
            saveFileDialog1.FileName = "*.map";

            Mouse.WindowHandle = tileDisplay1.Handle;
            Application.Idle += delegate { tileDisplay1.Invalidate(); };
        }

        private void frmTileMapEditor_Load(object sender, EventArgs e)
        {
        }

        void tdTileMap_OnInitialize(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            camera = new Camera(tileDisplay1.Width, tileDisplay1.Height);

            //tileMap = new TileMap("..\\..\\..\\overworld.txt", graphicsDevice);
        }

        void tdTileMap_OnDraw(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                Logic();
                Render();
            }
        }

        private void Logic()
        {
            camera.Update(new Vector2(hsTileMap.Value * tileMap.TileWidth, vsTileMap.Value * tileMap.TileHeight), tileMap.PixelWidth, tileMap.PixelHeight);

            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;

            if (mx >= 0 && mx < tileDisplay1.Width && my >= 0 && my < tileDisplay1.Height)
            {
                cellX = hsTileMap.Value + (mx / tileMap.TileWidth);
                cellY = vsTileMap.Value + (my / tileMap.TileHeight);

                cellX = (int)MathHelper.Clamp(cellX, 0, tileMap.Width);
                cellY = (int)MathHelper.Clamp(cellY, 0, tileMap.Height);

                if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (rbDraw.Checked)
                    {
                        if(currentTextureIndex != -1)
                            currentLayer.SetCellIndex(cellX, cellY, currentTextureIndex);

                        if (lstTextures.SelectedIndex != currentTextureIndex)
                            currentTextureIndex = lstTextures.SelectedIndex;
                    }
                    else if (rbErase.Checked)
                    {
                        currentLayer.SetCellIndex(cellX, cellY, -1);
                    }
                }
            }
            else
            {
                cellY = -1;
                cellX = -1;
            }
        }

        private void Render()
        {
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

            if (currentLayerIndex != -1)
            {
                //Draw all layers up to the selected one
                for (int i = 0; i < currentLayerIndex + 1; i++)
                    tileMap.Layers[i].Draw(spriteBatch, tileMap.TileTextures);

                //Draw empty cells
                for (int y = 0; y < tileMap.Layers[currentLayerIndex].Height; y++)
                    for (int x = 0; x < tileMap.Layers[currentLayerIndex].Height; x++)
                        if (tileMap.GetCellIndex(currentLayerIndex, x, y) == -1)
                            spriteBatch.Draw(emptyTile,
                                             new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                           tileMap.TileWidth, tileMap.TileHeight),
                                             Color.White);


            }
            /*
            else
            {
                tileMap.Draw(spriteBatch);
                for (int y = 0; y < tileMap.Layers[tileMap.Layers.Count-1].Height; y++)
                    for (int x = 0; x < tileMap.Layers[tileMap.Layers.Count - 1].Height; x++)
                        if (tileMap.GetCellIndex(tileMap.Layers.Count - 1, x, y) == -1)
                            spriteBatch.Draw(emptyTile,
                                             new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                           tileMap.TileWidth, tileMap.TileHeight),
                                             Color.White);
                UpdateCurrentLayerIndex(tileMap.Layers.Count - 1);
            }
            */


            if(cellX != -1 && cellY != -1)
            {
                spriteBatch.Draw(emptyTile,
                                    new Rectangle(cellX * tileMap.TileWidth, cellY * tileMap.TileWidth,
                                                tileMap.TileWidth, tileMap.TileHeight),
                                    Color.Blue);
            }

            spriteBatch.End();
        }

        private void UpdateCurrentLayerIndex(int newIndex)
        {
            lstLayers.SelectedIndex = newIndex;
            currentLayerIndex = newIndex;
        }

        private void cmdFindContent_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtContentFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void newTIleMapToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fileStream = new FileStream(@"..\..\content\" + "tileCursor.png", FileMode.Open))
                    cursor = Texture2D.FromStream(graphicsDevice, fileStream);
                using (FileStream fileStream = new FileStream(@"..\..\content\" + "tileEmpty.png", FileMode.Open))
                    emptyTile = Texture2D.FromStream(graphicsDevice, fileStream);
                
                textureList = new Dictionary<string,Texture2D>();
                previewList = new Dictionary<string,Image>();
                lstTextures.Items.Clear();
                
                string filename = openFileDialog1.FileName;

                tileMap = new TileMap(filename, graphicsDevice);

                for (int i = 1; i <= tileMap.Layers.Count; i++)
                    lstLayers.Items.Add("layer" + i);

                UpdateCurrentLayerIndex(tileMap.Layers.Count - 1);

                foreach(string textureName in tileMap.TextureNames)
                {
                    string fullpath = txtContentFolder.Text + @"\" + textureName;
                    foreach (string ext in imageExtensions)
                    {
                        if(File.Exists(fullpath + ext))
                        {
                            fullpath += ext;
                            break;
                        }
                    }

                    Texture2D texture;
                    using (FileStream fileStream = new FileStream(fullpath, FileMode.Open))
                        texture = Texture2D.FromStream(graphicsDevice, fileStream);
                    Image image = Image.FromFile(fullpath);

                    textureList.Add(textureName, texture);
                    previewList.Add(textureName, image);
                    lstTextures.Items.Add(textureName);
                }

                if (tileMap.PixelWidth > tileDisplay1.Width)
                {
                    hsTileMap.Visible = true;
                    hsTileMap.Minimum = 0;
                    hsTileMap.Maximum = tileMap.Width;

                }
                if (tileMap.PixelHeight > tileDisplay1.Height)
                {
                    vsTileMap.Visible = true;
                    vsTileMap.Minimum = 0;
                    vsTileMap.Maximum = tileMap.Height;
                }
            } 
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tileMap != null)
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    tileMap.Save(saveFileDialog1.FileName);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lstTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTextures.SelectedItem != null)
            {
                picPreview.Image = previewList[lstTextures.SelectedItem.ToString()];
                currentTextureIndex = lstTextures.SelectedIndex;
                tempTextureIndex = currentTextureIndex;
            }
        }

        private void lstLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLayers.SelectedItem != null)
                UpdateCurrentLayerIndex(lstLayers.SelectedIndex);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTextureIndex = -1;
        }

        private void cmdAddLayer_Click(object sender, EventArgs e)
        {

        }

        private void cmdRemoveLayer_Click(object sender, EventArgs e)
        {

        }

    }
}
