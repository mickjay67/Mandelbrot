using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        double zoom = 1;
        double centerX = 0;
        double centerY = 0;
        int maxIter = 10000;
        Bitmap bmp = new Bitmap(750, 500);
        public Form1()
        {
            InitializeComponent();
        }

        private void Draw_Click(object sender, EventArgs e)
        {            
            int xRatio = 3;
            int yRatio = 2;
            double xmax = xRatio * zoom + centerX;
            double ymax = yRatio * zoom + centerY;
            double xmin = -1 * xmax + centerX;
            double ymin = -1 * ymax + centerY;
            double xRange = xmax * 2;
            double yRange = ymax * 2;
            pictureBox1.Image = Mandelbrot(xmax, xmin, ymax, ymin);            
        }

        public Bitmap Mandelbrot(double xMax, double xMin, double yMax, double yMin) {                     
            double x1;
            double y1;
            double nxtX;
            double xZoom = ((xMax - xMin) / Convert.ToDouble(pictureBox1.Width));
            double yZoom = ((yMax - yMin) / Convert.ToDouble(pictureBox1.Height));
            double baseX = xMin;
            for (int x = 0; x < pictureBox1.Width; x++) {
                double baseY = yMin;
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    x1 = baseX;
                    y1 = baseY;
                    int ct = 0;
                    while (Math.Sqrt((x1 * x1) + (y1 * y1)) < 2 && ct < maxIter)
                    {
                        nxtX = (x1 * x1) - (y1 * y1) + baseX;
                        y1 = 2 * x1 * y1 + baseY;
                        x1 =nxtX; 
                        ct++;
                    }
                    if (ct >= maxIter)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                    else {
                        int red;
                        int green;
                        int blue;
                        if (ct % maxIter > 0 && ct % maxIter < maxIter/3) {
                            red = (int)(((ct + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y)) / 2) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y)) / 3) % (255 /** (double)maxIter*/)));
                        }
                        else if (ct % maxIter > 0 && ct % maxIter < 2*maxIter / 3) {
                            red = (int)(((ct + ((x * x) + (y * y))/5) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y))/2) % (255 /** (double)maxIter*/)));
                        }
                        else {
                            red = (int)(((ct + ((x * x) + (y * y))/3) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y)) / 6) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                        }
                                          
                        //int red = (int)(((ct + ((x*x)+(y*y))) % (255 /** (double)maxIter*/)));
                        //int blue = (int)((((ct) + ((x * x) + (y * y))/2) % (255 /** (double)maxIter*/)));
                        //int green = (int)((((ct) + ((x * x) + (y * y))/3)  % (255 /** (double)maxIter*/)));
                        if (red == 0) { red = 50; }
                        else if (blue == 0) { blue = 50; }
                        else if (green == 0) { green = 50; }
                        bmp.SetPixel(x, y, Color.FromArgb(red,green,blue));
                    }
                    baseY += yZoom;                
                }
                baseX += xZoom;
            }
            return bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

          
        }
        //click the image to zoom in

        //right click to zoom back one layer

        //ctrl click to draw julia

        //arrow keys to pan around
    }
}
