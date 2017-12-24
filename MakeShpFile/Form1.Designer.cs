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
            this.PreviousVersionMode = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MYTOCControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AxmapCtrl)).BeginInit();
            this.SuspendLayout();
            // 
            // ReadExl
            // 
            this.ReadExl.Location = new System.Drawing.Point(596, 28);
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
            this.BtnOriginArrayRead.Location = new System.Drawing.Point(729, 28);
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
            this.panel1.Controls.Add(this.PreviousVersionMode);
            this.panel1.Controls.Add(this.WalkingModeRbtn);
            this.panel1.Controls.Add(this.RidingModeRbtn);
            this.panel1.Controls.Add(this.PublicTranspRBtn);
            this.panel1.Controls.Add(this.CarModeRBtn);
            this.panel1.Location = new System.Drawing.Point(28, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 211);
            this.panel1.TabIndex = 11;
            // 
            // WalkingModeRbtn
            // 
            this.WalkingModeRbtn.AutoSize = true;
            this.WalkingModeRbtn.Location = new System.Drawing.Point(31, 145);
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
            this.RidingModeRbtn.Location = new System.Drawing.Point(31, 104);
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
            this.PublicTranspRBtn.Location = new System.Drawing.Point(31, 59);
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
            this.panel2.Location = new System.Drawing.Point(28, 326);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(223, 116);
            this.panel2.TabIndex = 12;
            // 
            // PolygonRBtn
            // 
            this.PolygonRBtn.AutoSize = true;
            this.PolygonRBtn.Location = new System.Drawing.Point(33, 81);
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
            this.PointRBtn.Location = new System.Drawing.Point(33, 48);
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
            this.PolylineRBtn.Location = new System.Drawing.Point(33, 13);
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
            this.panel3.Location = new System.Drawing.Point(285, 87);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(661, 445);
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
            // PreviousVersionMode
            // 
            this.PreviousVersionMode.AutoSize = true;
            this.PreviousVersionMode.Location = new System.Drawing.Point(31, 178);
            this.PreviousVersionMode.Name = "PreviousVersionMode";
            this.PreviousVersionMode.Size = new System.Drawing.Size(125, 16);
            this.PreviousVersionMode.TabIndex = 2;
            this.PreviousVersionMode.TabStop = true;
            this.PreviousVersionMode.Text = "Previous version ";
            this.PreviousVersionMode.UseVisualStyleBackColor = true;
            this.PreviousVersionMode.CheckedChanged += new System.EventHandler(this.WalkingModeRbtn_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 531);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BtnOriginArrayRead);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.ReadExl);
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
            this.ResumeLayout(false);

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
        private System.Windows.Forms.RadioButton PreviousVersionMode;
    }
}

