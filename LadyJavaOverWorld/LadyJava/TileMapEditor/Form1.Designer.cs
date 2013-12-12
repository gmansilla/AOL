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
            this.newTileMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.cmdAddLayer = new System.Windows.Forms.Button();
            this.cmdRemoveLayer = new System.Windows.Forms.Button();
            this.cmdAddTexture = new System.Windows.Forms.Button();
            this.cmdRemoveTexture = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.chkFill = new System.Windows.Forms.CheckBox();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.lstTextures = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtResizeW = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtResizeH = new System.Windows.Forms.TextBox();
            this.cmdApplyResize = new System.Windows.Forms.Button();
            this.tileDisplay1 = new TileMapEditor.TileDisplay();
            this.tileDisplay2 = new TileMapEditor.TileDisplay();
            this.lstEntrances = new System.Windows.Forms.ListBox();
            this.cmdAddEntrance = new System.Windows.Forms.Button();
            this.cmdRemoveEntrance = new System.Windows.Forms.Button();
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
            this.menuStrip1.Size = new System.Drawing.Size(1181, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTileMapToolStripMenuItem,
            this.openToolStripMenuItem,
            this.resizeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // newTileMapToolStripMenuItem
            // 
            this.newTileMapToolStripMenuItem.Name = "newTileMapToolStripMenuItem";
            this.newTileMapToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.newTileMapToolStripMenuItem.Text = "New Tile Map";
            this.newTileMapToolStripMenuItem.Click += new System.EventHandler(this.newTileMapToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.resizeToolStripMenuItem.Text = "Resize";
            this.resizeToolStripMenuItem.Click += new System.EventHandler(this.resizeToolStripMenuItem_Click);
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
            // cmdAddLayer
            // 
            this.cmdAddLayer.Location = new System.Drawing.Point(720, 202);
            this.cmdAddLayer.Name = "cmdAddLayer";
            this.cmdAddLayer.Size = new System.Drawing.Size(99, 23);
            this.cmdAddLayer.TabIndex = 8;
            this.cmdAddLayer.Text = "Add";
            this.cmdAddLayer.UseVisualStyleBackColor = true;
            this.cmdAddLayer.Click += new System.EventHandler(this.cmdAddLayer_Click);
            // 
            // cmdRemoveLayer
            // 
            this.cmdRemoveLayer.Location = new System.Drawing.Point(821, 202);
            this.cmdRemoveLayer.Name = "cmdRemoveLayer";
            this.cmdRemoveLayer.Size = new System.Drawing.Size(99, 23);
            this.cmdRemoveLayer.TabIndex = 9;
            this.cmdRemoveLayer.Text = "Remove";
            this.cmdRemoveLayer.UseVisualStyleBackColor = true;
            this.cmdRemoveLayer.Click += new System.EventHandler(this.cmdRemoveLayer_Click);
            // 
            // cmdAddTexture
            // 
            this.cmdAddTexture.Location = new System.Drawing.Point(720, 599);
            this.cmdAddTexture.Name = "cmdAddTexture";
            this.cmdAddTexture.Size = new System.Drawing.Size(99, 23);
            this.cmdAddTexture.TabIndex = 8;
            this.cmdAddTexture.Text = "Add";
            this.cmdAddTexture.UseVisualStyleBackColor = true;
            this.cmdAddTexture.Click += new System.EventHandler(this.cmdAddTexture_Click);
            // 
            // cmdRemoveTexture
            // 
            this.cmdRemoveTexture.Location = new System.Drawing.Point(821, 599);
            this.cmdRemoveTexture.Name = "cmdRemoveTexture";
            this.cmdRemoveTexture.Size = new System.Drawing.Size(99, 23);
            this.cmdRemoveTexture.TabIndex = 9;
            this.cmdRemoveTexture.Text = "Remove";
            this.cmdRemoveTexture.UseVisualStyleBackColor = true;
            this.cmdRemoveTexture.Click += new System.EventHandler(this.cmdRemoveTexture_Click);
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picPreview.Location = new System.Drawing.Point(934, 375);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(235, 235);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 10;
            this.picPreview.TabStop = false;
            // 
            // chkFill
            // 
            this.chkFill.AutoSize = true;
            this.chkFill.Location = new System.Drawing.Point(834, 65);
            this.chkFill.Name = "chkFill";
            this.chkFill.Size = new System.Drawing.Size(38, 17);
            this.chkFill.TabIndex = 12;
            this.chkFill.Text = "Fill";
            this.chkFill.UseVisualStyleBackColor = true;
            // 
            // lstLayers
            // 
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.Location = new System.Drawing.Point(720, 88);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.Size = new System.Drawing.Size(200, 108);
            this.lstLayers.TabIndex = 7;
            this.lstLayers.SelectedIndexChanged += new System.EventHandler(this.lstLayers_SelectedIndexChanged);
            // 
            // lstTextures
            // 
            this.lstTextures.FormattingEnabled = true;
            this.lstTextures.Location = new System.Drawing.Point(720, 375);
            this.lstTextures.Name = "lstTextures";
            this.lstTextures.Size = new System.Drawing.Size(200, 186);
            this.lstTextures.TabIndex = 7;
            this.lstTextures.SelectedIndexChanged += new System.EventHandler(this.lstTextures_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(720, 573);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "W:";
            // 
            // txtResizeW
            // 
            this.txtResizeW.Location = new System.Drawing.Point(747, 573);
            this.txtResizeW.Name = "txtResizeW";
            this.txtResizeW.Size = new System.Drawing.Size(35, 20);
            this.txtResizeW.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(785, 573);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "H:";
            // 
            // txtResizeH
            // 
            this.txtResizeH.Location = new System.Drawing.Point(809, 573);
            this.txtResizeH.Name = "txtResizeH";
            this.txtResizeH.Size = new System.Drawing.Size(35, 20);
            this.txtResizeH.TabIndex = 15;
            // 
            // cmdApplyResize
            // 
            this.cmdApplyResize.Location = new System.Drawing.Point(851, 573);
            this.cmdApplyResize.Name = "cmdApplyResize";
            this.cmdApplyResize.Size = new System.Drawing.Size(69, 23);
            this.cmdApplyResize.TabIndex = 16;
            this.cmdApplyResize.Text = "Apply";
            this.cmdApplyResize.UseVisualStyleBackColor = true;
            this.cmdApplyResize.Click += new System.EventHandler(this.cmdApplyResize_Click);
            // 
            // tileDisplay1
            // 
            this.tileDisplay1.Location = new System.Drawing.Point(12, 27);
            this.tileDisplay1.Name = "tileDisplay1";
            this.tileDisplay1.Size = new System.Drawing.Size(684, 575);
            this.tileDisplay1.TabIndex = 13;
            this.tileDisplay1.Text = "tileDisplay1";
            // 
            // tileDisplay2
            // 
            this.tileDisplay2.Location = new System.Drawing.Point(934, 87);
            this.tileDisplay2.Name = "tileDisplay2";
            this.tileDisplay2.Size = new System.Drawing.Size(235, 235);
            this.tileDisplay2.TabIndex = 13;
            this.tileDisplay2.Text = "tileDisplay2";
            // 
            // lstEntrances
            // 
            this.lstEntrances.FormattingEnabled = true;
            this.lstEntrances.Location = new System.Drawing.Point(720, 231);
            this.lstEntrances.Name = "lstEntrances";
            this.lstEntrances.Size = new System.Drawing.Size(200, 108);
            this.lstEntrances.TabIndex = 7;
            this.lstEntrances.SelectedIndexChanged += new System.EventHandler(this.lstLayers_SelectedIndexChanged);
            // 
            // cmdAddEntrance
            // 
            this.cmdAddEntrance.Location = new System.Drawing.Point(721, 346);
            this.cmdAddEntrance.Name = "cmdAddEntrance";
            this.cmdAddEntrance.Size = new System.Drawing.Size(75, 23);
            this.cmdAddEntrance.TabIndex = 17;
            this.cmdAddEntrance.Text = "Add";
            this.cmdAddEntrance.UseVisualStyleBackColor = true;
            this.cmdAddEntrance.Click += new System.EventHandler(this.cmdAddEntrance_Click_1);
            // 
            // cmdRemoveEntrance
            // 
            this.cmdRemoveEntrance.Location = new System.Drawing.Point(834, 346);
            this.cmdRemoveEntrance.Name = "cmdRemoveEntrance";
            this.cmdRemoveEntrance.Size = new System.Drawing.Size(75, 23);
            this.cmdRemoveEntrance.TabIndex = 18;
            this.cmdRemoveEntrance.Text = "Remove";
            this.cmdRemoveEntrance.UseVisualStyleBackColor = true;
            this.cmdRemoveEntrance.Click += new System.EventHandler(this.cmdRemoveEntrance_Click_1);
            // 
            // frmTileMapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 634);
            this.Controls.Add(this.cmdRemoveEntrance);
            this.Controls.Add(this.cmdAddEntrance);
            this.Controls.Add(this.cmdApplyResize);
            this.Controls.Add(this.txtResizeH);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtResizeW);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tileDisplay1);
            this.Controls.Add(this.tileDisplay2);
            this.Controls.Add(this.chkFill);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.cmdRemoveTexture);
            this.Controls.Add(this.cmdRemoveLayer);
            this.Controls.Add(this.cmdAddTexture);
            this.Controls.Add(this.cmdAddLayer);
            this.Controls.Add(this.lstTextures);
            this.Controls.Add(this.lstEntrances);
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
        private System.Windows.Forms.ToolStripMenuItem newTileMapToolStripMenuItem;
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
        private System.Windows.Forms.Button cmdAddLayer;
        private System.Windows.Forms.Button cmdRemoveLayer;
        private System.Windows.Forms.Button cmdAddTexture;
        private System.Windows.Forms.Button cmdRemoveTexture;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.CheckBox chkFill;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.ListBox lstTextures;
        private TileDisplay tileDisplay2;
        private TileDisplay tileDisplay1;
        private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtResizeW;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtResizeH;
        private System.Windows.Forms.Button cmdApplyResize;
        private System.Windows.Forms.ListBox lstEntrances;
        private System.Windows.Forms.Button cmdAddEntrance;
        private System.Windows.Forms.Button cmdRemoveEntrance;
    }
}