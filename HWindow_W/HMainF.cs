using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace HWindow_W
{
    public partial class HMainF : Form
    {
        string FilePath = System.Environment.CurrentDirectory + @"\ImageSource\plastics\plastics_11.png";
        HImage image; int IWidth, IHeight;
        public HMainF()
        {
            InitializeComponent();
            image = new HImage(FilePath);
            image.GetImageSize(out IWidth, out IHeight);
        }

        private void btn_LoadImage_Click(object sender, EventArgs e)
        {

            hWindowControl1.HalconWindow.SetPart(0, 0, IWidth, IHeight);
            hWindowControl1.SetFullImagePart(image);
            hWindowControl1.HalconWindow.DispImage(image);
        }

        private void btn_SaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "BMP图像|*.bmp|所有文件|*.*";
            if (dia.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dia.FileName))
                    return;
                HOperatorSet.WriteImage(image, "bmp", 0, dia.FileName);
            }
        }


        private void hWindowControl1_HMouseWheel(object sender, HMouseEventArgs e)
        {
            double scale;
            double PointX, PointY;
            double Row1, Row2, Col1, Col2;
            double PercentW, PercentH;
            int Swidth, Sheight;
            if (e.Delta > 0)
                scale = 0.9;
            else
                scale = 1 / 0.9;
            //缩放中心
            PointX = e.X;
            PointY = e.Y;
            Row1 = hWindowControl1.ImagePart.Y;
            Col1 = hWindowControl1.ImagePart.X;
            Row2 = hWindowControl1.ImagePart.Bottom;
            Col2 = hWindowControl1.ImagePart.Right;

            hWindowControl1.HalconWindow.ClearWindow();
            //缩放中心的比例
            PercentW = (PointX - Col1) / (Col2 - Col1);
            PercentH = (PointY - Row1) / (Row2 - Row1);
            //缩放后的尺寸
            Swidth = (int)Math.Round(scale * (Col2 - Col1));
            Sheight = (int)Math.Round(scale * (Row2 - Row1));

            System.Drawing.Rectangle rect = hWindowControl1.ImagePart;
            rect.X = (int)Math.Round(PointX - PercentW * Swidth);
            rect.Y = (int)Math.Round(PointY - PercentH * Sheight);
            rect.Width = (Swidth > 0) ? Swidth : 1;
            rect.Height = (Sheight > 0) ? Sheight : 1;

            hWindowControl1.ImagePart = rect;
            hWindowControl1.HalconWindow.DispImage(image);

        }
    }
}
