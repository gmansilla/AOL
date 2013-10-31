namespace TileMapEditor
{
    partial class frmTileMapEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hsTileMap = new System.Windows.Forms.HScrollBar();
            this.vsTileMap = new System.Windows.Forms.VScrollBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTIleMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtContentFolder = new System.Windows.Forms.TextBox();
            this.cmdFindContent = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.rbDraw = new System.Windows.Forms.RadioButton();
            this.rbErase = new System.Windows.Forms.RadioButton();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.cmdAddLayer = new System.Windows.Forms.Button();
            this.cmdRemoveLayer = new System.Windows.Forms.Button();
            this.lstTextures = new System.Windows.Forms.ListBox();
            this.cmdAddTexture = new System.Windows.Forms.Button();
            this.cmdRemoveTexture = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.tileDisplay1 = new TileMapEditor.TileDisplay();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // hsTileMap
            // 
            this.hsTileMap.Location = new System.Drawing.Point(12, 605);
            this.hsTileMap.Name = "hsTileMap";
            this.hsTileMap.Size = new System.Drawing.Size(684, 17);
            this.hsTileMap.TabIndex = 1;
            this.hsTileMap.Visible = false;
            // 
            // vsTileMap
            // 
            this.vsTileMap.Location = new System.Drawing.Point(699, 24);
            this.vsTileMap.Name = "vsTileMap";
            this.vsTileMap.Size = new System.Drawing.Size(18, 578);
            this.vsTileMap.TabIndex = 2;
            this.vsTileMap.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(930, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTIleMapToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // newTIleMapToolStripMenuItem
            // 
            this.newTIleMapToolStripMenuItem.Name = "newTIleMapToolStripMenuItem";
            this.newTIleMapToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newTIleMapToolStripMenuItem.Text = "New TIle Map";
            this.newTIleMapToolStripMenuItem.Click += new System.EventHandler(this.newTIleMapToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtContentFolder
            // 
            this.txtContentFolder.Location = new System.Drawing.Point(720, 35);
            this.txtContentFolder.Name = "txtContentFolder";
            this.txtContentFolder.ReadOnly = true;
            this.txtContentFolder.Size = new System.Drawing.Size(171, 20);
            this.txtContentFolder.TabIndex = 4;
            this.txtContentFolder.Text = "..\\..\\..\\LadyJava\\LadyJavaContent";
            // 
            // cmdFindContent
            // 
            this.cmdFindContent.Location = new System.Drawing.Point(892, 35);
            this.cmdFindContent.Name = "cmdFindContent";
            this.cmdFindContent.Size = new System.Drawing.Size(28, 23);
            this.cmdFindContent.TabIndex = 5;
            this.cmdFindContent.Text = "...";
            this.cmdFindContent.UseVisualStyleBackColor = true;
            this.cmdFindContent.Click += new System.EventHandler(this.cmdFindContent_Click);
            // 
            // rbDraw
            // 
            this.rbDraw.AutoSize = true;
            this.rbDraw.Checked = true;
            this.rbDraw.Location = new System.Drawing.Point(720, 64);
            this.rbDraw.Name = "rbDraw";
            this.rbDraw.Size = new System.Drawing.Size(50, 17);
            this.rbDraw.TabIndex = 6;
            this.rbDraw.TabStop = true;
            this.rbDraw.Text = "Draw";
            this.rbDraw.UseVisualStyleBackColor = true;
            // 
            // rbErase
            // 
            this.rbErase.AutoSize = true;
            this.rbErase.Location = new System.Drawing.Point(776, 64);
            this.rbErase.Name = "rbErase";
            this.rbErase.Size = new System.Drawing.Size(52, 17);
            this.rbErase.TabIndex = 6;
            this.rbErase.Text = "Erase";
            this.rbErase.UseVisualStyleBackColor = true;
            // 
            // lstLayers
            // 
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.Location = new System.Drawing.Point(720, 87);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.Size = new System.Drawing.Size(200, 121);
            this.lstLayers.TabIndex = 7;
            this.lstLayers.SelectedIndexChanged += new System.EventHandler(this.lstLayers_SelectedIndexChanged);
            // 
            // cmdAddLayer
            // 
            this.cmdAddLayer.Location = new System.Drawing.Point(720, 214);
            this.cmdAddLayer.Name = "cmdAddLayer";
            this.cmdAddLayer.Size = new System.Drawing.Size(99, 23);
            this.cmdAddLayer.TabIndex = 8;
            this.cmdAddLayer.Text = "Add";
            this.cmdAddLayer.UseVisualStyleBackColor = true;
            this.cmdAddLayer.Click += new System.EventHandler(this.cmdAddLayer_Click);
            // 
            // cmdRemoveLayer
            // 
            this.cmdRemoveLayer.Location = new System.Drawing.Point(821, 214);
            this.cmdRemoveLayer.Name = "cmdRemoveLayer";
            this.cmdRemoveLayer.Size = new System.Drawing.Size(99, 23);
            this.cmdRemoveLayer.TabIndex = 9;
            this.cmdRemoveLayer.Text = "Remove";
            this.cmdRemoveLayer.UseVisualStyleBackColor = true;
            this.cmdRemoveLayer.Click += new System.EventHandler(this.cmdRemoveLayer_Click);
            // 
            // lstTextures
            // 
            this.lstTextures.FormattingEnabled = true;
            this.lstTextures.Location = new System.Drawing.Point(720, 243);
            this.lstTextures.Name = "lstTextures";
            this.lstTextures.Size = new System.Drawing.Size(200, 121);
            this.lstTextures.TabIndex = 7;
            this.lstTextures.SelectedIndexChanged += new System.EventHandler(this.lstTextures_SelectedIndexChanged);
            // 
            // cmdAddTexture
            // 
            this.cmdAddTexture.Location = new System.Drawing.Point(720, 370);
            this.cmdAddTexture.Name = "cmdAddTexture";
            this.cmdAddTexture.Size = new System.Drawing.Size(99, 23);
            this.cmdAddTexture.TabIndex = 8;
            this.cmdAddTexture.Text = "Add";
            this.cmdAddTexture.UseVisualStyleBackColor = true;
            // 
            // cmdRemoveTexture
            // 
            this.cmdRemoveTexture.Location = new System.Drawing.Point(821, 370);
            this.cmdRemoveTexture.Name = "cmdRemoveTexture";
            this.cmdRemoveTexture.Size = new System.Drawing.Size(99, 23);
            this.cmdRemoveTexture.TabIndex = 9;
            this.cmdRemoveTexture.Text = "Remove";
            this.cmdRemoveTexture.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picPreview.Location = new System.Drawing.Point(720, 399);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(200, 200);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 10;
            this.picPreview.TabStop = false;
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Location = new System.Drawing.Point(13, 24);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(683, 575);
            this.tileDisplay1.TabIndex = 11;
            this.tileDisplay1.Text = "tileDisplay2";
            // 
            // frmTileMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 634);
            this.Controls.Add(this.tileDisplay1);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.cmdRemoveTexture);
            this.Controls.Add(this.cmdRemoveLayer);
            this.Controls.Add(this.cmdAddTexture);
            this.Controls.Add(this.cmdAddLayer);
            this.Controls.Add(this.lstTextures);
            this.Controls.Add(this.lstLayers);
            this.Controls.Add(this.rbErase);
            this.Controls.Add(this.rbDraw);
            this.Controls.Add(this.cmdFindContent);
            this.Controls.Add(this.txtContentFolder);
            this.Controls.Add(this.vsTileMap);
            this.Controls.Add(this.hsTileMap);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmTileMapEditor";
            this.Text = "Tile Map Editor";
            this.Load += new System.EventHandler(this.frmTileMapEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hsTileMap;
        private System.Windows.Forms.VScrollBar vsTileMap;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTIleMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtContentFolder;
        private System.Windows.Forms.Button cmdFindContent;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RadioButton rbDraw;
        private System.Windows.Forms.RadioButton rbErase;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.Button cmdAddLayer;
        private System.Windows.Forms.Button cmdRemoveLayer;
        private System.Windows.Forms.ListBox lstTextures;
        private System.Windows.Forms.Button cmdAddTexture;
        private System.Windows.Forms.Button cmdRemoveTexture;
        private System.Windows.Forms.PictureBox picPreview;
        private TileDisplay tileDisplay1;
    }
}