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
        double Gzoom = 1;
        double centerX = 0;
        double centerY = 0;
        int clicks = 0;
        int maxIter = 10000;
        Bitmap bmp = new Bitmap(750, 500);
        public Form1()
        {
            InitializeComponent();
        }

        private void Draw_Click(object sender, EventArgs e)
        {
            double locZoom = 0;
            int xRatio = 3;
            int yRatio = 2;
            double xmax = xRatio * (Gzoom-locZoom) + centerX;
            double ymax = yRatio * (Gzoom-locZoom) + centerY;
            double xmin = -1 * xmax + centerX;
            double ymin = -1 * ymax + centerY;
            double xRange = xmax * 2;
            double yRange = ymax * 2; 
            pictureBox1.Image = Mandelbrot(xmax, xmin, ymax, ymin);
            pictureBox3.Image = Julia(xmax, xmin, ymax, ymin);
            clicks = 0;
            textBox1.Clear();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //click the image to zoom in by a factor of .2 for the first 4 zooms, after that you have a factor .95 as the maximum
            MouseEventArgs mouse = (MouseEventArgs)e;
            int mpX = mouse.X;
            int mpY = mouse.Y;
            double mX = mpX/pictureBox1.Width *  
            clicks++;
            double locZoom = 1;
            if (clicks < 5)
            {
                locZoom = clicks * 0.2;
            }
            else { locZoom = 0.95; }
            int xRatio = 3;
            int yRatio = 2;
            double xmax = xRatio * (Gzoom - locZoom) + centerX;
            double ymax = yRatio * (Gzoom - locZoom) + centerY;
            centerX = mpX/(double)pictureBox1.Width * xmax;
            centerY = mpY / (double)pictureBox1.Height * ymax;
            xmax = xRatio * (Gzoom - locZoom) + centerX;
            ymax = yRatio * (Gzoom - locZoom) + centerY;
            Console.WriteLine(centerX+ " " + xmax + " " + ymax+" " +mpX+" " +pictureBox1.Width + " " + (mpX / pictureBox1.Width));
            double xmin = -1 * xmax + centerX;
            double ymin = -1 * ymax + centerY;
            double xRange = xmax * 2;
            double yRange = ymax * 2;
            pictureBox1.Image = Mandelbrot(xmax, xmin, ymax, ymin);
            pictureBox3.Image = Julia(xmax, xmin, ymax, ymin);
            textBox1.Clear();
            if (clicks <= 5)
            {
                textBox1.AppendText(clicks.ToString());
            }
            else { textBox1.AppendText("MAX"); }

        }


        //right click to zoom back one layer

        //ctrl click to draw julia

        //arrow keys to pan around


        public Bitmap Mandelbrot(double xMax, double xMin, double yMax, double yMin)
        {
            double x1;
            double y1;
            double nxtX;
            double xZoom = ((xMax - xMin) / Convert.ToDouble(pictureBox1.Width));
            double yZoom = ((yMax - yMin) / Convert.ToDouble(pictureBox1.Height));
            double baseX = xMin;
            for (int x = 0; x < pictureBox1.Width; x++)
            {
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
                        x1 = nxtX;
                        ct++;
                    }
                    if (ct >= maxIter)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        int red;
                        int green;
                        int blue;
                        if (ct % maxIter > 0 && ct % maxIter < maxIter / 3)
                        {
                            red = (int)(((ct + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y)) / 2) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y)) / 3) % (255 /** (double)maxIter*/)));
                        }
                        else if (ct % maxIter > 0 && ct % maxIter < 2 * maxIter / 3)
                        {
                            red = (int)(((ct + ((x * x) + (y * y)) / 5) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y)) / 2) % (255 /** (double)maxIter*/)));
                        }
                        else
                        {
                            red = (int)(((ct + ((x * x) + (y * y)) / 3) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y)) / 6) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                        }

                        //int red = (int)(((ct + ((x*x)+(y*y))) % (255 /** (double)maxIter*/)));
                        //int blue = (int)((((ct) + ((x * x) + (y * y))/2) % (255 /** (double)maxIter*/)));
                        //int green = (int)((((ct) + ((x * x) + (y * y))/3)  % (255 /** (double)maxIter*/)));
                        if (red == 0) { red = 50; }
                        else if (blue == 0) { blue = 50; }
                        else if (green == 0) { green = 50; }
                        bmp.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                    baseY += yZoom;
                }
                baseX += xZoom;
            }
            return bmp;
        }


        public Bitmap Julia(double xMax, double xMin, double yMax, double yMin) {
            Console.WriteLine(centerX + " " + xMax + " " + yMax);
            Bitmap Jbmp = new Bitmap(375,250);
            double x1;
            double y1;
            double cx = xMax - xMin;
            double cy = yMax - yMin;
            
            double nxtX;
            double xZoom = ((xMax - xMin) / Convert.ToDouble(pictureBox3.Width));
            double yZoom = ((yMax - yMin) / Convert.ToDouble(pictureBox3.Height));
            double baseX = xMin;
            for (int x = 0; x < pictureBox3.Width; x++)
            {
                double baseY = yMin;
                for (int y = 0; y < pictureBox3.Height; y++)
                {
                    x1 = baseX;
                    y1 = baseY;
                    int ct = 0;
                    while (Math.Sqrt((x1 * x1) + (y1 * y1)) < 2 && ct < maxIter)
                    {
                        nxtX = (x1 * x1) - (y1 * y1) + centerX;
                        y1 = 2 * x1 * y1 + centerY;
                        x1 = nxtX;
                        ct++;
                        Console.WriteLine(centerX + " " + nxtX + " " + x1 + " " + y1);
                    }
                    if (ct >= maxIter)
                    {
                        Jbmp.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        int red;
                        int green;
                        int blue;
                        if (ct % maxIter > 0 && ct % maxIter < maxIter / 3)
                        {
                            red = (int)(((ct + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y)) / 2) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y)) / 3) % (255 /** (double)maxIter*/)));
                        }
                        else if (ct % maxIter > 0 && ct % maxIter < 2 * maxIter / 3)
                        {
                            red = (int)(((ct + ((x * x) + (y * y)) / 5) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y)) / 2) % (255 /** (double)maxIter*/)));
                        }
                        else
                        {
                            red = (int)(((ct + ((x * x) + (y * y)) / 3) % (255 /** (double)maxIter*/)));
                            blue = (int)((((ct) + ((x * x) + (y * y)) / 6) % (255 /** (double)maxIter*/)));
                            green = (int)((((ct) + ((x * x) + (y * y))) % (255 /** (double)maxIter*/)));
                        }

                        //int red = (int)(((ct + ((x*x)+(y*y))) % (255 /** (double)maxIter*/)));
                        //int blue = (int)((((ct) + ((x * x) + (y * y))/2) % (255 /** (double)maxIter*/)));
                        //int green = (int)((((ct) + ((x * x) + (y * y))/3)  % (255 /** (double)maxIter*/)));
                        if (red == 0) { red = 50; }
                        else if (blue == 0) { blue = 50; }
                        else if (green == 0) { green = 50; }
                        Jbmp.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                    baseY += yZoom;
                }
                baseX += xZoom;
            }
            return Jbmp;

        }
    }
}
