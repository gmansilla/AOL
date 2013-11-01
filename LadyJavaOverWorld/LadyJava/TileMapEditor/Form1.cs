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
        Point invalidPoint = new Point(-1, -1);
        const int MaxFillCount = 100;

        int fillCount = 0;

        string[] imageExtensions = new string[] { ".jpg", ".png", ".tga" };
        
        Camera camera;
        TileMap tileMap;
        SpriteBatch spriteBatch;
        Texture2D cursor;
        Texture2D emptyTile;

        Dictionary<string, TileLayer> tileLayerList = new Dictionary<string, TileLayer>();
        //Dictionary<string, Texture2D> textureList = new Dictionary<string, Texture2D>();
        Dictionary<string, Image> previewList = new Dictionary<string, Image>();

        int cellX;
        int cellY;
        //int tempTextureIndex = -1;

        int currentLayerIndex
        { get { return lstLayers.SelectedIndex; }
          set { lstLayers.SelectedIndex = value; } }

        int currentTextureIndex
        { get { return lstTextures.SelectedIndex; }
          set { lstTextures.SelectedIndex = value; } }

        TileLayer currentLayer
        { get { return tileMap.Layers[lstLayers.SelectedIndex]; } }

        GraphicsDevice graphicsDevice
        { get { return tileDisplay1.GraphicsDevice; } }

        public frmTileMapEditor()
        {
            InitializeComponent();
            this.CenterToScreen();

            tileDisplay1.OnInitializeAdd(new EventHandler(tdTileMap_OnInitialize));
            tileDisplay1.OnDrawAdd(new EventHandler(tdTileMap_OnDraw));
            
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

            using (FileStream fileStream = new FileStream(@"..\..\content\" + "tileCursor.png", FileMode.Open))
            {
                cursor = Texture2D.FromStream(graphicsDevice, fileStream);
                fileStream.Close();
            }
            using (FileStream fileStream = new FileStream(@"..\..\content\" + "tileEmpty.png", FileMode.Open))
            {
                emptyTile = Texture2D.FromStream(graphicsDevice, fileStream);
                fileStream.Close();
            }
        }

        void tdTileMap_OnDraw(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                Logic();
                Render();

            }
        }

        public void FillCell(int layerIndex, int x, int y, int newIndex)
        {
            Point currentLocation = new Point(x, y);
            int totalLoops = (int)Math.Round((float)(tileMap.Width * tileMap.Height / MaxFillCount), MidpointRounding.AwayFromZero);

            //for(int i = 0; i < totalLoops; i++)
            while (currentLocation != invalidPoint)
            {
                fillCount = 0;
                //if(currentLocation != invalidPoint)
                currentLocation = FillCellPartial(layerIndex, currentLocation.X, currentLocation.Y, newIndex);
        }

        }

        public Point FillCellPartial(int layerIndex, int x, int y, int newIndex)
        {
            fillCount++;
            int oldIndex = tileMap.Layers[layerIndex].GetCellIndex(x, y);
            tileMap.Layers[layerIndex].SetCellIndex(x, y, newIndex);

            Point currentLocation = invalidPoint;

            if (oldIndex != newIndex)
            {
                if (x > 0 && tileMap.Layers[layerIndex].GetCellIndex(x - 1, y) == oldIndex)
                    if (fillCount <= MaxFillCount)
                        currentLocation = FillCellPartial(layerIndex, x - 1, y, newIndex);
                    else
                        currentLocation = new Point(x - 1, y);
                if (x < tileMap.Width - 1 && tileMap.Layers[layerIndex].GetCellIndex(x + 1, y) == oldIndex)
                    if (fillCount <= MaxFillCount)
                        currentLocation = FillCellPartial(layerIndex, x + 1, y, newIndex);
                    else
                        currentLocation = new Point(x + 1, y);
                if (y > 0 && tileMap.Layers[layerIndex].GetCellIndex(x, y - 1) == oldIndex)
                    if (fillCount <= MaxFillCount)
                        currentLocation = FillCellPartial(layerIndex, x, y - 1, newIndex);
                    else
                        currentLocation = new Point(x, y - 1);
                if (y < tileMap.Height - 1 && tileMap.Layers[layerIndex].GetCellIndex(x, y + 1) == oldIndex)
                    if (fillCount <= MaxFillCount)
                        currentLocation = FillCellPartial(layerIndex, x, y + 1, newIndex);
                    else
                        currentLocation = new Point(x, y + 1);
            }

            return currentLocation;
        }

        private void Logic()
        {
            camera.Update(new Vector2(hsTileMap.Value * tileMap.TileWidth, vsTileMap.Value * tileMap.TileHeight), tileMap.PixelWidth, tileMap.PixelHeight);

            int mx = Mouse.GetState().X;
            int my = Mouse.GetState().Y;

            if (mx >= 0 && mx < tileDisplay1.Width && my >= 0 && my < tileDisplay1.Height)
            {
                cellX = hsTileMap.Value + (mx / (tileMap.TileWidth));
                cellY = vsTileMap.Value + (my / (tileMap.TileHeight));

                cellX = (int)MathHelper.Clamp(cellX, 0, tileMap.Width - 1);
                cellY = (int)MathHelper.Clamp(cellY, 0, tileMap.Height - 1);

                if (Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                {
                    if (rbDraw.Checked)
                    {
                        if (currentTextureIndex != -1 && currentLayerIndex != -1)
                            if (!chkFill.Checked)
                                tileMap.SetCellIndex(currentLayerIndex, cellX, cellY, currentTextureIndex);
                            else
                            {
                                FillCell(currentLayerIndex, cellX, cellY, currentTextureIndex);
                            }
                    }
                    else if (rbErase.Checked)
                    {
                        if (currentLayerIndex != -1)
                            if (!chkFill.Checked)
                                tileMap.SetCellIndex(currentLayerIndex, cellX, cellY, -1);
                            else
                            {
                                FillCell(currentLayerIndex, cellX, cellY, -1);
                            }
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

            if(cellX != -1 && cellY != -1)
            {
                spriteBatch.Draw(emptyTile,
                                    new Rectangle(cellX * tileMap.TileWidth, cellY * tileMap.TileWidth,
                                                tileMap.TileWidth, tileMap.TileHeight),
                                    Color.Blue);
            }

            spriteBatch.End();
        }

        private void cmdFindContent_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                txtContentFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void newTileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewTileMap form = new frmNewTileMap();
            
            form.ShowDialog();
            if (form.OKPressed)
            {
                int width = int.Parse(form.txtWidth.Text);
                int height = int.Parse(form.txtHeight.Text);
                int tileWidth = int.Parse(form.txtTileWidth.Text);
                int tileHeight = int.Parse(form.txtTileHeight.Text);

                tileMap = new TileMap(width, height, tileWidth, tileHeight);
                //currentLayerIndex = 0;
                lstLayersUpdate(tileMap.Layers.Count);
                currentLayerIndex = tileMap.Layers.Count - 1;
                //lstLayers.SelectedIndex = tileMap.Layers.Count - 1;
                currentTextureIndex = -1;

                UpdateScrollbars();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = txtContentFolder.Text;
            openFileDialog1.Filter = "Tile Map FIle|*.map";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                previewList = new Dictionary<string,Image>();
                lstTextures.Items.Clear();
                
                string filename = openFileDialog1.FileName;

                tileMap = new TileMap(filename, graphicsDevice);

                lstLayersUpdate(tileMap.Layers.Count);

                currentLayerIndex = tileMap.Layers.Count - 1;
                //lstLayers.SelectedIndex = tileMap.Layers.Count - 1;

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
                    using (FileStream fileStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read))
                    {
                        texture = Texture2D.FromStream(graphicsDevice, fileStream);
                        fileStream.Close();
                    }
                    Image image = Image.FromFile(fullpath);

                    //textureList.Add(textureName, texture);
                    previewList.Add(textureName, image);
                    lstTextures.Items.Add(textureName);
                }

                UpdateScrollbars();
            } 
        }

        private void UpdateScrollbars()
        {
            hsTileMap.Visible = false;
            hsTileMap.Value = 0;
            hsTileMap.Minimum = 0;
            hsTileMap.Maximum = tileMap.Width - 1;

            vsTileMap.Visible = false;
            vsTileMap.Value = 0;
            vsTileMap.Minimum = 0;
            vsTileMap.Maximum = tileMap.Height - 1;

            if (tileMap.PixelWidth > tileDisplay1.Width)
            {
                hsTileMap.Visible = true;

            }
            if (tileMap.PixelHeight > tileDisplay1.Height)
            {
                vsTileMap.Visible = true;
            }
        }

        private void lstLayersUpdate(int amount)
        {
            lstLayers.Items.Clear();
            lstLayersADD(amount);
        }

        private void lstLayersADD(int amount)
        {
            for (int i = 1; i <= amount; i++)
                lstLayers.Items.Add("layer" + i);
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
                //currentTextureIndex = lstTextures.SelectedIndex;
                //tempTextureIndex = currentTextureIndex;
            }
        }

        private void lstLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (lstLayers.SelectedItem != null)
            //    currentLayerIndex = newIndex;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //currentTextureIndex = -1;
        }

        private void cmdAddLayer_Click(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                tileMap.AddLayer();
                lstLayersUpdate(tileMap.Layers.Count);
                //currentLayerIndex = lstLayers.Items.Count - 1;
            }
        }

        private void cmdRemoveLayer_Click(object sender, EventArgs e)
        {
            if (currentLayerIndex != -1)
            {
                int tempIndex = currentLayerIndex - 1;

                tileMap.RemoveLayer(currentLayerIndex);
                lstLayers.Items.RemoveAt(currentLayerIndex);
                currentLayerIndex = tempIndex;

                UpdateScrollbars();
            }
        }

        private void cmdAddTexture_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = txtContentFolder.Text;
            openFileDialog1.Filter = "Texture Files|*.jpg; *.png; *.tga";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK && tileMap != null)
            {
                string texturePath = openFileDialog1.FileName;
                string rootFolder = txtContentFolder.Text.Substring(txtContentFolder.Text.LastIndexOf("\\"));
                string textureName = texturePath.Substring(texturePath.LastIndexOf(rootFolder) + rootFolder.Length);
                textureName = textureName.Substring(textureName.IndexOf("\\") + 1);
                textureName = textureName.Substring(0, textureName.IndexOf("."));
                if(!previewList.ContainsKey(textureName))
                {
                    Texture2D newTexture = tileMap.AddTexture(texturePath, textureName, graphicsDevice);
                    lstTextures.Items.Add(textureName);

                    Image image = Image.FromFile(texturePath);
                    previewList.Add(textureName, image);

                    if(currentTextureIndex == -1)
                        currentTextureIndex = lstTextures.Items.Count - 1;
                        //lstTextures.SelectedIndex = lstTextures.Items.Count - 1;
                }
                else
                    MessageBox.Show("Texture has already been added to the list.");
            }
        }

        private void cmdRemoveTexture_Click(object sender, EventArgs e)
        {
            if (currentTextureIndex != -1)
            {
                int tempIndex = currentTextureIndex - 1;

                tileMap.RemoveTexture(currentTextureIndex);
                lstTextures.Items.RemoveAt(currentTextureIndex);
                currentTextureIndex = tempIndex;
            }
        }

    }
}
