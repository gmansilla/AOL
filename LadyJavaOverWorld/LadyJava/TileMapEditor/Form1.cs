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

        string[] defaultCollisionItems = { "collisionBlock", "startPosition" };
        int collisionBlockIndex = 0;
        int startPositionIndex = 1;

        int fillCount = 0;

        int storedTextureIndex = -1;

        string[] imageExtensions = new string[] { ".jpg", ".png", ".tga" };
        
        Camera camera;
        Camera previewCamera;
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

        int currentEntranceIndex
        { get { return lstEntrances.SelectedIndex; }
          set { lstEntrances.SelectedIndex = value; } }

        TileLayer currentLayer
        { get { return tileMap.Layers[lstLayers.SelectedIndex]; } }

        GraphicsDevice graphicsDevice
        { get { return tileDisplay1.GraphicsDevice; } }

        public frmTileMapEditor()
        {
            InitializeComponent();
            this.CenterToScreen();

            tileDisplay2.OnInitializeAdd(new EventHandler(tileDisplay2_OnInitialize));
            tileDisplay2.OnDrawAdd(new EventHandler(tileDisplay2_OnDraw));

            tileDisplay1.OnInitializeAdd(new EventHandler(tileDisplay1_OnInitialize));
            tileDisplay1.OnDrawAdd(new EventHandler(tileDisplay1_OnDraw));

            saveFileDialog1.Filter = "Tile Map FIle|*.map";
            saveFileDialog1.FileName = "*.map";

            Mouse.WindowHandle = tileDisplay1.Handle;
            Application.Idle += delegate { tileDisplay1.Invalidate(); tileDisplay2.Invalidate(); };
        }

        private void frmTileMapEditor_Load(object sender, EventArgs e)
        {
        }

        void tileDisplay1_OnInitialize(object sender, EventArgs e)
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

        void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                Logic();
                Render();

            }
        }

        void tileDisplay2_OnInitialize(object sender, EventArgs e)
        {
            //spriteBatch = new SpriteBatch(graphicsDevice);
            previewCamera = new Camera(tileDisplay1.Width, tileDisplay1.Height);
        }

        void tileDisplay2_OnDraw(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                LogicPreview();
                RenderPreview();

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
            camera.Update(new Vector2(hsTileMap.Value * tileMap.TileWidth, vsTileMap.Value * tileMap.TileHeight));

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
                            if (currentLayerIndex == tileMap.Layers.Count)
                            {
                                if (lstEntrances.SelectedItem != null)
                                {
                                    if (lstEntrances.SelectedItem.ToString() == defaultCollisionItems[collisionBlockIndex])
                                        tileMap.CollisionLayer.SetCellIndex(cellX, cellY, CollisionLayer.CollisionCell);
                                    else if (lstEntrances.SelectedItem.ToString() == defaultCollisionItems[startPositionIndex])
                                        tileMap.CollisionLayer.SetCellIndex(cellX, cellY, CollisionLayer.StartingCell);
                                    else if (currentEntranceIndex != -1)
                                        tileMap.CollisionLayer.AddEntrance(lstEntrances.SelectedItem.ToString(), cellX, cellY, currentEntranceIndex - defaultCollisionItems.Length);
                                }
                            }
                            else if (!chkFill.Checked)
                                tileMap.SetCellIndex(currentLayerIndex, cellX, cellY, currentTextureIndex);
                            else
                            {
                                FillCell(currentLayerIndex, cellX, cellY, currentTextureIndex);
                            }
                    }
                    else if (rbErase.Checked)
                    {
                        if (currentLayerIndex != -1 && currentTextureIndex != -1)
                            if (currentLayerIndex == tileMap.Layers.Count)
                            {
                                if (lstEntrances.SelectedItem != null)
                                {
                                    if (lstEntrances.SelectedItem.ToString() == defaultCollisionItems[collisionBlockIndex])
                                        tileMap.CollisionLayer.SetCellIndex(cellX, cellY, CollisionLayer.NothingCell);
                                    else if (lstEntrances.SelectedItem.ToString() == defaultCollisionItems[startPositionIndex])
                                        tileMap.CollisionLayer.SetCellIndex(cellX, cellY, CollisionLayer.NothingCell);
                                    else if (currentEntranceIndex != -1)
                                        tileMap.CollisionLayer.RemoveEntrance(cellX, cellY);

                                }
                            }
                            else if (!chkFill.Checked)
                                tileMap.SetCellIndex(currentLayerIndex, cellX, cellY, TileLayer.TransparentCell);
                            else
                            {
                                FillCell(currentLayerIndex, cellX, cellY, TileLayer.TransparentCell);
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

        private void LogicPreview()
        {
            float scale = tileDisplay2.Width / (float)tileMap.PixelWidth;
            previewCamera.Update(Vector2.Zero, scale);
        }

        private void Render()
        {
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.TransformMatrix);

            if (currentLayerIndex != -1)
            {
                int drawTo = currentLayerIndex;
                if (currentLayerIndex == lstLayers.Items.Count - 1)
                    drawTo = currentLayerIndex - 1;

                //Draw all layers up to the selected one
                for (int i = 0; i <= drawTo; i++)
                    tileMap.Layers[i].Draw(spriteBatch, tileMap.Tiles);

                //Draw empty cells
                for (int y = 0; y < tileMap.Layers[drawTo].Height; y++)
                    for (int x = 0; x < tileMap.Layers[drawTo].Width; x++)
                        if (tileMap.GetCellIndex(drawTo, x, y) == TileLayer.TransparentCell)
                            spriteBatch.Draw(emptyTile,
                                             new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                           tileMap.TileWidth, 
                                                           tileMap.TileHeight),
                                             Color.White);

                if(drawTo != currentLayerIndex)
                    for (int y = 0; y < tileMap.CollisionLayer.Height; y++)
                        for (int x = 0; x < tileMap.CollisionLayer.Width; x++)
                            if (tileMap.CollisionLayer.GetCellIndex(x, y) == CollisionLayer.CollisionCell)
                                spriteBatch.Draw(emptyTile,
                                                 new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                               tileMap.TileWidth,
                                                               tileMap.TileHeight),
                                                 Color.Red);
                            else if (tileMap.CollisionLayer.GetCellIndex(x, y) == CollisionLayer.StartingCell)
                                spriteBatch.Draw(emptyTile,
                                                 new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                               tileMap.TileWidth,
                                                               tileMap.TileHeight),
                                                 Color.Gold);
                            else if (tileMap.CollisionLayer.GetCellIndex(x, y) >= 0)
                                spriteBatch.Draw(emptyTile,
                                                 new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                               tileMap.TileWidth,
                                                               tileMap.TileHeight),
                                                 Color.Green);

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

        private void RenderPreview()
        {
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, previewCamera.TransformMatrix);

            if (currentLayerIndex != -1)
            {
                //Draw all layers up to the selected one
                for (int i = 0; i < tileMap.Layers.Count; i++)
                    tileMap.Layers[i].Draw(spriteBatch, tileMap.Tiles);

                for (int y = 0; y < tileMap.CollisionLayer.Height; y++)
                    for (int x = 0; x < tileMap.CollisionLayer.Width; x++)
                        if (tileMap.CollisionLayer.GetCellIndex(x, y) == CollisionLayer.CollisionCell)
                            spriteBatch.Draw(emptyTile,
                                             new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                           tileMap.TileWidth,
                                                           tileMap.TileHeight),
                                             Color.Red);
                        else if (tileMap.CollisionLayer.GetCellIndex(x, y) == CollisionLayer.StartingCell)
                            spriteBatch.Draw(emptyTile,
                                             new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                           tileMap.TileWidth,
                                                           tileMap.TileHeight),
                                             Color.Gold);
                        else if (tileMap.CollisionLayer.GetCellIndex(x, y) >= 0)
                            spriteBatch.Draw(emptyTile,
                                             new Rectangle(x * tileMap.TileWidth, y * tileMap.TileHeight,
                                                           tileMap.TileWidth,
                                                           tileMap.TileHeight),
                                             Color.Green);
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

                lstEntrances.Items.Clear();
                foreach(string item in defaultCollisionItems)
                    lstEntrances.Items.Add(item);

                lstTextures.Items.Clear();
                previewList = new Dictionary<string, Image>();

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
            openFileDialog1.Filter = "Tile Map File|*.map";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                previewList = new Dictionary<string,Image>();
                lstTextures.Items.Clear();
                lstEntrances.Items.Clear();
                foreach(string item in defaultCollisionItems)
                    lstEntrances.Items.Add(item);
        
                string filename = openFileDialog1.FileName;

                tileMap = new TileMap(filename, graphicsDevice);

                lstLayersUpdate(tileMap.Layers.Count);

                currentLayerIndex = tileMap.Layers.Count - 1;
                //lstLayers.SelectedIndex = tileMap.Layers.Count - 1;

                foreach (string entrance in tileMap.CollisionLayer.Entrances)
                    lstEntrances.Items.Add(entrance);

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
                if(lstTextures.Items.Count > 1)
                    currentTextureIndex = 0;
                
                UpdateScrollbars();
            }

            if (storedTextureIndex != -1)
                currentTextureIndex = storedTextureIndex;
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
            vsTileMap.Maximum = tileMap.Height;

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
            lstLayers.Items.Add("collisionLayer");
            currentLayerIndex = lstLayers.Items.Count - 2;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                saveFileDialog1.InitialDirectory = txtContentFolder.Text;
                saveFileDialog1.Filter = "Tile Map File|*.map";
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    tileMap.Save(saveFileDialog1.FileName);
                }

                if (storedTextureIndex != -1)
                    currentTextureIndex = storedTextureIndex;
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
                txtResizeW.Text = tileMap.Tiles[currentTextureIndex].Width.ToString();
                txtResizeH.Text = tileMap.Tiles[currentTextureIndex].Height.ToString();
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
            storedTextureIndex = currentTextureIndex;
            currentTextureIndex = -1;
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

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                frmNewTileMap form = new frmNewTileMap();

                form.Text = "Resize Tile Map";
                form.txtWidth.Text = tileMap.Width.ToString();
                form.txtHeight.Text = tileMap.Height.ToString();
                form.txtTileWidth.Text = tileMap.TileWidth.ToString();
                form.txtTileHeight.Text = tileMap.TileHeight.ToString();

                form.ShowDialog();
                if (form.OKPressed)
                {
                    int width = int.Parse(form.txtWidth.Text);
                    int height = int.Parse(form.txtHeight.Text);
                    int tileWidth = int.Parse(form.txtTileWidth.Text);
                    int tileHeight = int.Parse(form.txtTileHeight.Text);

                    tileMap.Resize(width, height, tileWidth, tileHeight);
                    //currentLayerIndex = 0;
                    //lstLayersUpdate(tileMap.Layers.Count);
                    //currentLayerIndex = tileMap.Layers.Count - 1;
                    //lstLayers.SelectedIndex = tileMap.Layers.Count - 1;
                    //currentTextureIndex = -1;

                    UpdateScrollbars();
                }

                if (storedTextureIndex != -1)
                    currentTextureIndex = storedTextureIndex;
            }
        }

        private void cmdApplyResize_Click(object sender, EventArgs e)
        {
            tileMap.Tiles[currentTextureIndex].Resize(int.Parse(txtResizeW.Text), int.Parse(txtResizeH.Text));
        }

        private void cmdAddEntrance_Click_1(object sender, EventArgs e)
        {
            if (tileMap != null)
            {
                openFileDialog1.InitialDirectory = txtContentFolder.Text;
                openFileDialog1.Filter = "TileMap Files|*.map";
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog() == DialogResult.OK && tileMap != null)
                {
                    string tileMapPath = openFileDialog1.FileName;
                    string rootFolder = txtContentFolder.Text.Substring(txtContentFolder.Text.LastIndexOf("\\"));
                    string tileMapName = tileMapPath.Substring(tileMapPath.LastIndexOf(rootFolder) + rootFolder.Length);
                    tileMapName = tileMapName.Substring(tileMapName.IndexOf("\\") + 1);
                    lstEntrances.Items.Add(tileMapName);

                    //textureName = textureName.Substring(0, textureName.IndexOf("."));
                }
            }
        }

        private void cmdRemoveEntrance_Click_1(object sender, EventArgs e)
        {
            if (currentLayerIndex != -1 &&
                currentEntranceIndex != -1 &&
                lstEntrances.Items[currentEntranceIndex].ToString() != defaultCollisionItems[collisionBlockIndex] &&
                lstEntrances.Items[currentEntranceIndex].ToString() != defaultCollisionItems[startPositionIndex])
            {
                tileMap.CollisionLayer.RemoveEntrance(lstEntrances.SelectedItem.ToString());
                int newIndex = currentEntranceIndex - 1;
                lstEntrances.Items.RemoveAt(currentEntranceIndex);
                currentEntranceIndex = newIndex;
            }
        }

       
    }
}
