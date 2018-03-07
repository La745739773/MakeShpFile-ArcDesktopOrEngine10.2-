namespace MakeShpFile
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ReadExl = new System.Windows.Forms.Button();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.BtnOriginArrayRead = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DefaultRbtn = new System.Windows.Forms.RadioButton();
            this.WalkingModeRbtn = new System.Windows.Forms.RadioButton();
            this.RidingModeRbtn = new System.Windows.Forms.RadioButton();
            this.PublicTranspRBtn = new System.Windows.Forms.RadioButton();
            this.CarModeRBtn = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PolygonRBtn = new System.Windows.Forms.RadioButton();
            this.PointRBtn = new System.Windows.Forms.RadioButton();
            this.PolylineRBtn = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.MYTOCControl = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.AxmapCtrl = new ESRI.ArcGIS.Controls.AxMapControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ComboBox_GeoSystem = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ComboBox_Prjsystem = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FILE_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Quit_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取DB文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReadDb_menu = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MYTOCControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxmapCtrl)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReadExl
            // 
            this.ReadExl.Location = new System.Drawing.Point(596, 38);
            this.ReadExl.Name = "ReadExl";
            this.ReadExl.Size = new System.Drawing.Size(106, 39);
            this.ReadExl.TabIndex = 2;
            this.ReadExl.Text = "读取Csv文件";
            this.ReadExl.UseVisualStyleBackColor = true;
            this.ReadExl.Click += new System.EventHandler(this.ReadExl_Click);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(610, 446);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 4;
            // 
            // BtnOriginArrayRead
            // 
            this.BtnOriginArrayRead.Location = new System.Drawing.Point(731, 38);
            this.BtnOriginArrayRead.Name = "BtnOriginArrayRead";
            this.BtnOriginArrayRead.Size = new System.Drawing.Size(106, 39);
            this.BtnOriginArrayRead.TabIndex = 5;
            this.BtnOriginArrayRead.Text = "读取Csv组合文件";
            this.BtnOriginArrayRead.UseVisualStyleBackColor = true;
            this.BtnOriginArrayRead.Click += new System.EventHandler(this.BtnOriginArrayRead_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.DefaultRbtn);
            this.panel1.Controls.Add(this.WalkingModeRbtn);
            this.panel1.Controls.Add(this.RidingModeRbtn);
            this.panel1.Controls.Add(this.PublicTranspRBtn);
            this.panel1.Controls.Add(this.CarModeRBtn);
            this.panel1.Location = new System.Drawing.Point(12, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(209, 211);
            this.panel1.TabIndex = 11;
            // 
            // DefaultRbtn
            // 
            this.DefaultRbtn.AutoSize = true;
            this.DefaultRbtn.Location = new System.Drawing.Point(31, 172);
            this.DefaultRbtn.Name = "DefaultRbtn";
            this.DefaultRbtn.Size = new System.Drawing.Size(65, 16);
            this.DefaultRbtn.TabIndex = 2;
            this.DefaultRbtn.TabStop = true;
            this.DefaultRbtn.Text = "Default";
            this.DefaultRbtn.UseVisualStyleBackColor = true;
            this.DefaultRbtn.CheckedChanged += new System.EventHandler(this.DefaultRbtn_CheckedChanged);
            // 
            // WalkingModeRbtn
            // 
            this.WalkingModeRbtn.AutoSize = true;
            this.WalkingModeRbtn.Location = new System.Drawing.Point(31, 134);
            this.WalkingModeRbtn.Name = "WalkingModeRbtn";
            this.WalkingModeRbtn.Size = new System.Drawing.Size(95, 16);
            this.WalkingModeRbtn.TabIndex = 2;
            this.WalkingModeRbtn.TabStop = true;
            this.WalkingModeRbtn.Text = "Walking Mode";
            this.WalkingModeRbtn.UseVisualStyleBackColor = true;
            this.WalkingModeRbtn.CheckedChanged += new System.EventHandler(this.WalkingModeRbtn_CheckedChanged);
            // 
            // RidingModeRbtn
            // 
            this.RidingModeRbtn.AutoSize = true;
            this.RidingModeRbtn.Location = new System.Drawing.Point(31, 96);
            this.RidingModeRbtn.Name = "RidingModeRbtn";
            this.RidingModeRbtn.Size = new System.Drawing.Size(89, 16);
            this.RidingModeRbtn.TabIndex = 2;
            this.RidingModeRbtn.TabStop = true;
            this.RidingModeRbtn.Text = "Riding Mode";
            this.RidingModeRbtn.UseVisualStyleBackColor = true;
            this.RidingModeRbtn.CheckedChanged += new System.EventHandler(this.RidingModeRbtn_CheckedChanged);
            // 
            // PublicTranspRBtn
            // 
            this.PublicTranspRBtn.AutoSize = true;
            this.PublicTranspRBtn.Location = new System.Drawing.Point(31, 58);
            this.PublicTranspRBtn.Name = "PublicTranspRBtn";
            this.PublicTranspRBtn.Size = new System.Drawing.Size(149, 16);
            this.PublicTranspRBtn.TabIndex = 2;
            this.PublicTranspRBtn.TabStop = true;
            this.PublicTranspRBtn.Text = "public transport Mode";
            this.PublicTranspRBtn.UseVisualStyleBackColor = true;
            this.PublicTranspRBtn.CheckedChanged += new System.EventHandler(this.PublicTranspRBtn_CheckedChanged);
            // 
            // CarModeRBtn
            // 
            this.CarModeRBtn.AutoSize = true;
            this.CarModeRBtn.Location = new System.Drawing.Point(31, 20);
            this.CarModeRBtn.Name = "CarModeRBtn";
            this.CarModeRBtn.Size = new System.Drawing.Size(71, 16);
            this.CarModeRBtn.TabIndex = 1;
            this.CarModeRBtn.TabStop = true;
            this.CarModeRBtn.Text = "Car Mode";
            this.CarModeRBtn.UseVisualStyleBackColor = true;
            this.CarModeRBtn.CheckedChanged += new System.EventHandler(this.CarModeRBtn_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.PolygonRBtn);
            this.panel2.Controls.Add(this.PointRBtn);
            this.panel2.Controls.Add(this.PolylineRBtn);
            this.panel2.Location = new System.Drawing.Point(12, 362);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(209, 157);
            this.panel2.TabIndex = 12;
            // 
            // PolygonRBtn
            // 
            this.PolygonRBtn.AutoSize = true;
            this.PolygonRBtn.Location = new System.Drawing.Point(33, 108);
            this.PolygonRBtn.Name = "PolygonRBtn";
            this.PolygonRBtn.Size = new System.Drawing.Size(119, 16);
            this.PolygonRBtn.TabIndex = 0;
            this.PolygonRBtn.TabStop = true;
            this.PolygonRBtn.Text = "Polygon Features";
            this.PolygonRBtn.UseVisualStyleBackColor = true;
            this.PolygonRBtn.CheckedChanged += new System.EventHandler(this.PolygonRBtn_CheckedChanged);
            // 
            // PointRBtn
            // 
            this.PointRBtn.AutoSize = true;
            this.PointRBtn.Location = new System.Drawing.Point(33, 67);
            this.PointRBtn.Name = "PointRBtn";
            this.PointRBtn.Size = new System.Drawing.Size(107, 16);
            this.PointRBtn.TabIndex = 0;
            this.PointRBtn.TabStop = true;
            this.PointRBtn.Text = "Point Features";
            this.PointRBtn.UseVisualStyleBackColor = true;
            this.PointRBtn.CheckedChanged += new System.EventHandler(this.PointRBtn_CheckedChanged);
            // 
            // PolylineRBtn
            // 
            this.PolylineRBtn.AutoSize = true;
            this.PolylineRBtn.Location = new System.Drawing.Point(33, 24);
            this.PolylineRBtn.Name = "PolylineRBtn";
            this.PolylineRBtn.Size = new System.Drawing.Size(125, 16);
            this.PolylineRBtn.TabIndex = 0;
            this.PolylineRBtn.TabStop = true;
            this.PolylineRBtn.Text = "Polyline Features";
            this.PolylineRBtn.UseVisualStyleBackColor = true;
            this.PolylineRBtn.CheckedChanged += new System.EventHandler(this.PolylineRBtn_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.MYTOCControl);
            this.panel3.Controls.Add(this.AxmapCtrl);
            this.panel3.Location = new System.Drawing.Point(486, 90);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(626, 445);
            this.panel3.TabIndex = 13;
            // 
            // MYTOCControl
            // 
            this.MYTOCControl.Location = new System.Drawing.Point(3, 3);
            this.MYTOCControl.Name = "MYTOCControl";
            this.MYTOCControl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("MYTOCControl.OcxState")));
            this.MYTOCControl.Size = new System.Drawing.Size(155, 435);
            this.MYTOCControl.TabIndex = 1;
            // 
            // AxmapCtrl
            // 
            this.AxmapCtrl.Location = new System.Drawing.Point(164, 3);
            this.AxmapCtrl.Name = "AxmapCtrl";
            this.AxmapCtrl.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("AxmapCtrl.OcxState")));
            this.AxmapCtrl.Size = new System.Drawing.Size(494, 435);
            this.AxmapCtrl.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.ComboBox_GeoSystem);
            this.panel4.Location = new System.Drawing.Point(227, 90);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(253, 209);
            this.panel4.TabIndex = 14;
            // 
            // ComboBox_GeoSystem
            // 
            this.ComboBox_GeoSystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_GeoSystem.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ComboBox_GeoSystem.FormattingEnabled = true;
            this.ComboBox_GeoSystem.Location = new System.Drawing.Point(3, 4);
            this.ComboBox_GeoSystem.Name = "ComboBox_GeoSystem";
            this.ComboBox_GeoSystem.Size = new System.Drawing.Size(243, 24);
            this.ComboBox_GeoSystem.TabIndex = 0;
            this.ComboBox_GeoSystem.SelectedIndexChanged += new System.EventHandler(this.ComboBox_GeoSystem_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(41, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 23);
            this.label1.TabIndex = 15;
            this.label1.Text = "抓取数据来源";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(41, 321);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "恢复要素类型";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(280, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 23);
            this.label3.TabIndex = 15;
            this.label3.Text = "地理坐标系";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(280, 321);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 23);
            this.label4.TabIndex = 15;
            this.label4.Text = "投影坐标系";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.ComboBox_Prjsystem);
            this.panel5.Location = new System.Drawing.Point(227, 362);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(253, 157);
            this.panel5.TabIndex = 14;
            // 
            // ComboBox_Prjsystem
            // 
            this.ComboBox_Prjsystem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_Prjsystem.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ComboBox_Prjsystem.FormattingEnabled = true;
            this.ComboBox_Prjsystem.Location = new System.Drawing.Point(3, 3);
            this.ComboBox_Prjsystem.Name = "ComboBox_Prjsystem";
            this.ComboBox_Prjsystem.Size = new System.Drawing.Size(248, 24);
            this.ComboBox_Prjsystem.TabIndex = 0;
            this.ComboBox_Prjsystem.SelectedIndexChanged += new System.EventHandler(this.ComboBox_Prjsystem_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FILE_ToolStripMenuItem,
            this.读取DB文件ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1114, 28);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "Main_Menu";
            // 
            // FILE_ToolStripMenuItem
            // 
            this.FILE_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Quit_ToolStripMenuItem});
            this.FILE_ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FILE_ToolStripMenuItem.Name = "FILE_ToolStripMenuItem";
            this.FILE_ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.FILE_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.FILE_ToolStripMenuItem.Size = new System.Drawing.Size(49, 24);
            this.FILE_ToolStripMenuItem.Text = "文件";
            // 
            // Quit_ToolStripMenuItem
            // 
            this.Quit_ToolStripMenuItem.Name = "Quit_ToolStripMenuItem";
            this.Quit_ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.Quit_ToolStripMenuItem.Size = new System.Drawing.Size(159, 24);
            this.Quit_ToolStripMenuItem.Text = "退出";
            this.Quit_ToolStripMenuItem.Click += new System.EventHandler(this.Quit_ToolStripMenuItem_Click);
            // 
            // 读取DB文件ToolStripMenuItem
            // 
            this.读取DB文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReadDb_menu});
            this.读取DB文件ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.读取DB文件ToolStripMenuItem.Name = "读取DB文件ToolStripMenuItem";
            this.读取DB文件ToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.读取DB文件ToolStripMenuItem.Text = "DB文件处理";
            // 
            // ReadDb_menu
            // 
            this.ReadDb_menu.Name = "ReadDb_menu";
            this.ReadDb_menu.Size = new System.Drawing.Size(187, 24);
            this.ReadDb_menu.Text = "读取Rout.db文件";
            this.ReadDb_menu.Click += new System.EventHandler(this.ReadDb_menu_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 531);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BtnOriginArrayRead);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.ReadExl);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MYTOCControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxmapCtrl)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadExl;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Button BtnOriginArrayRead;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton WalkingModeRbtn;
        private System.Windows.Forms.RadioButton RidingModeRbtn;
        private System.Windows.Forms.RadioButton PublicTranspRBtn;
        private System.Windows.Forms.RadioButton CarModeRBtn;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton PolygonRBtn;
        private System.Windows.Forms.RadioButton PointRBtn;
        private System.Windows.Forms.RadioButton PolylineRBtn;
        private System.Windows.Forms.Panel panel3;
        private ESRI.ArcGIS.Controls.AxMapControl AxmapCtrl;
        private ESRI.ArcGIS.Controls.AxTOCControl MYTOCControl;
        private System.Windows.Forms.RadioButton DefaultRbtn;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox ComboBox_GeoSystem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox ComboBox_Prjsystem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FILE_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Quit_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读取DB文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReadDb_menu;
    }
}

