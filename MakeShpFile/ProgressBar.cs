﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MakeShpFile
{
    public partial class ProgressBar : Form
    {
        public ProgressBar(int _Minimum, int _Maximum)//带参数，表示进度条的范围的最小值和最大值
        {
            InitializeComponent();
            ProgressCtrl.Maximum = _Maximum;//设置范围最大值
            ProgressCtrl.Value = ProgressCtrl.Minimum;//设置范围最小值
        }
        public void setPos(int value)//设置进度条当前进度值
        {
            if (value < ProgressCtrl.Maximum)//如果值有效
            {
                ProgressCtrl.Value = value;//设置进度值
                label1.Text = (value * 100 / ProgressCtrl.Maximum).ToString() + "%";//显示百分比
            }
            Application.DoEvents();//重点，必须加上，否则父子窗体都假死
        }
        private void ProgressBar_Load(object sender, EventArgs e)
        {
            this.Owner.Enabled = false;//设置父窗体不可用
        }

        private void ProgressBar_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Owner.Enabled = true;//回复父窗体为可用
        }
    }
}
