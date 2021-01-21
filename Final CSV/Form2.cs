using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_CSV
{
    public partial class GraphVisualizer : Form
    {
        public Bitmap b;
        public Graphics g;
        public Font smallFont = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Pixel);

        List<Object> itemsList = new List<object>();
        Type myType;
        Dictionary<String, Type> variables;
        List<DataPoint> dataset;
        List<double> xValues;
        List<double> yValues;

        double minXWindow;
        double maxXWindow;
        double minYWindow;
        double maxYWindow;
        double rangeX;
        double rangeY;

        bool draggingStarted;
        bool resizingStared;

        Rectangle scatterPlotViewport = new Rectangle(100, 25, 525, 325);
        Rectangle histogramXViewport;
        Rectangle histogramYViewPort;
        Rectangle contingencyTableViewPort;

        Histogram histogramX;
        Histogram histogramY;
        ContingencyTable contingencyTable;
        LinearRegression lr;


        private Rectangle viewportAtMouseDown;
        private Point mouseLocationAtMouseDown;

        private double minXWindowAtMouseDown;
        private double minYWindowAtMouseDown;
        private double maxXWindowAtMouseDown;
        private double maxYWindowAtMouseDown;
        private double rangeXAtMouseDown;
        private double rangeYAtMouseDown;

        public GraphVisualizer(List<Object> itemsList, Dictionary<String, Type> variables)
        {
            this.itemsList = itemsList;
            this.variables = variables;
            this.myType = itemsList[0].GetType();
            InitializeComponent();
            extractVariables();
        }

        private void extractVariables()
        {
            foreach (KeyValuePair<String, Type> kvp in variables)
            {
                if (kvp.Value.Equals(typeof(Int32)) || kvp.Value.Equals(typeof(Int64)) || kvp.Value.Equals(typeof(Double))) {
                    xComboBox.Items.Add(kvp.Key);
                    yComboBox.Items.Add(kvp.Key);
                }
            }
            //foreach (FieldInfo fi in itemsList[0].GetType().GetFields())
            //{
            //    if (fi.FieldType.Equals(typeof(Int32)) || fi.FieldType.Equals(typeof(Int64)) || fi.FieldType.Equals(typeof(Double)))
            //    {
            //        xComboBox.Items.Add(fi.Name);
            //        yComboBox.Items.Add(fi.Name);
            //    }
            //}

            xComboBox.SelectedItem = xComboBox.Items[0];
            yComboBox.SelectedItem = yComboBox.Items[1];

        }

        private void initGraphics()
        {
            this.b = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            this.g = Graphics.FromImage(b);
            this.g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            this.histogramXViewport = new Rectangle(scatterPlotViewport.X, scatterPlotViewport.Y + scatterPlotViewport.Height + 10, scatterPlotViewport.Width, scatterPlotViewport.Height / 4);
            this.histogramYViewPort = new Rectangle(scatterPlotViewport.X - scatterPlotViewport.Width / 4 - 10, scatterPlotViewport.Y, scatterPlotViewport.Width / 4, scatterPlotViewport.Height);
            this.contingencyTableViewPort = new Rectangle(10, pictureBox1.Height - 350, pictureBox1.Width - 75, 245);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xValues = new List<double>();
            yValues = new List<double>();

            try
            {
                for (int i = 0; i < itemsList.Count; i++)
                {
                    var tempX = myType.GetField(xComboBox.SelectedItem.ToString()).GetValue(itemsList[i]);
                    var tempY = myType.GetField(yComboBox.SelectedItem.ToString()).GetValue(itemsList[i]);
                    double x = Convert.ToDouble(tempX);
                    double y = Convert.ToDouble(tempY);

                    xValues.Add(x);
                    yValues.Add(y);
                }

                this.initGraphics();

                dataset = new List<DataPoint>();

                for (int i = 0; i < xValues.Count; i++)
                {
                    dataset.Add(new DataPoint(xValues[i], yValues[i]));
                }

                minXWindow = findMinValue(xValues);
                maxXWindow = findMaxValue(xValues);
                minYWindow = findMinValue(yValues);
                maxYWindow = findMaxValue(yValues);
                rangeX = maxXWindow - minXWindow;
                rangeY = maxYWindow - minYWindow;

                histogramX = new Histogram(xValues, 15);
                histogramY = new Histogram(yValues, 15);
                contingencyTable = new ContingencyTable(xValues, yValues);
                lr = new LinearRegression(xValues, yValues);
                this.drawScene();
            } catch (Exception ex)
            {
                Debug.WriteLine("Couldn't locate one of the variable types. Whoops!");
            }

            
        }

        public void drawScene()
        {
            g.Clear(Color.White);
            histogramX.drawXHistogram(g, histogramXViewport);
            histogramY.drawYHistogram(g, histogramYViewPort);
            contingencyTable.drawTable(g, contingencyTableViewPort, this.radioAbsolute.Checked);
            g.DrawRectangle(Pens.Red, scatterPlotViewport);
            g.DrawRectangle(Pens.Blue, histogramXViewport);
            g.DrawRectangle(Pens.Blue, histogramYViewPort);
            g.DrawRectangle(Pens.Green, contingencyTableViewPort);

            //Scatterplot
            foreach (DataPoint d in dataset)
            {
                int xDevice = this.xViewport(d.x, scatterPlotViewport, minXWindow, rangeX);
                int yDevice = this.yViewport(d.y, scatterPlotViewport, minYWindow, rangeY);

                //Scatterplot
                if (this.scatterPlotViewport.Contains(xDevice, yDevice))
                {
                    g.FillEllipse(Brushes.Black, new Rectangle(new Point(xDevice, yDevice), new Size(6, 6)));
                }
            }

            //Quartiles
            int xMean = this.xViewport(xValues.Average(), scatterPlotViewport, minXWindow, rangeX);
            int yMean = this.yViewport(yValues.Average(), scatterPlotViewport, minYWindow, rangeY);
            List<double> xQuartiles = this.histogramX.quartiles;
            List<double> yQuartiles = this.histogramY.quartiles;

            foreach (double q in xQuartiles)
            {
                int v = this.xViewport(q, scatterPlotViewport, minXWindow, rangeX);
                g.DrawLine(new Pen(Brushes.Red, 2), v, scatterPlotViewport.Y + scatterPlotViewport.Height, v, scatterPlotViewport.Y);
            }

            foreach (double q in yQuartiles)
            {
                int v = this.yViewport(q, scatterPlotViewport, minYWindow, rangeY);
                g.DrawLine(new Pen(Brushes.Red, 2), scatterPlotViewport.X + scatterPlotViewport.Width, v, scatterPlotViewport.X, v);
            }

            if (this.scatterPlotViewport.Contains(xMean, yMean))
            {
                g.DrawLine(Pens.Blue, xMean, scatterPlotViewport.Y + scatterPlotViewport.Height, xMean, scatterPlotViewport.Y);
                g.DrawLine(Pens.Blue, scatterPlotViewport.X + scatterPlotViewport.Width, yMean, scatterPlotViewport.X, yMean);
            }

            //Linear Regression
            double minXRegression, maxXRegression = 0;
            lr.findIntersectionPoints(minYWindow, maxYWindow, out minXRegression, out maxXRegression);
            double minXRegressionViewport = this.xViewport(minXRegression, scatterPlotViewport, minXWindow, rangeX);
            double maxXRegressionViewport = this.xViewport(maxXRegression, scatterPlotViewport, minXWindow, rangeX);
            double minY = this.yViewport(minYWindow, scatterPlotViewport, minYWindow, rangeY);
            double maxY = this.yViewport(maxYWindow, scatterPlotViewport, minYWindow, rangeY);
            g.DrawLine(Pens.Green, (float)minXRegressionViewport, (float)minY, (float)maxXRegressionViewport, (float)maxY);

            this.pictureBox1.Image = b;

        }

        public int xViewport(double xWorld, Rectangle viewport, double minX, double rangeX)
        {
            return (int)(viewport.Left + viewport.Width * (xWorld - minX) / rangeX);
        }

        public int yViewport(double yWorld, Rectangle viewport, double minY, double rangeY)
        {
            return (int)(viewport.Top + viewport.Height - viewport.Height * (yWorld - minY) / rangeY);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox1.Focus();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.scatterPlotViewport.Contains(e.X, e.Y))
            {
                this.viewportAtMouseDown = this.scatterPlotViewport;
                this.mouseLocationAtMouseDown = new Point(e.X, e.Y);

                this.minXWindowAtMouseDown = minXWindow;
                this.minYWindowAtMouseDown = minYWindow;
                this.maxXWindowAtMouseDown = maxXWindow;
                this.maxYWindowAtMouseDown = maxYWindow;
                this.rangeXAtMouseDown = rangeX;
                this.rangeYAtMouseDown = rangeY;

                if (e.Button == MouseButtons.Left)
                {
                    draggingStarted = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    resizingStared = true;
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingStarted)
            {
                int deltaX = e.X - mouseLocationAtMouseDown.X;
                int deltaY = e.Y - mouseLocationAtMouseDown.Y;

                double realWorldDeltaX = this.rangeXAtMouseDown * deltaX / this.scatterPlotViewport.Width;
                this.minXWindow = this.minXWindowAtMouseDown - realWorldDeltaX;
                this.maxXWindow = this.maxXWindowAtMouseDown - realWorldDeltaX;

                double realWorldDeltaY = this.rangeYAtMouseDown * deltaY / this.scatterPlotViewport.Height;
                this.minYWindow = this.minYWindowAtMouseDown + realWorldDeltaY;
                this.maxYWindow = this.maxYWindowAtMouseDown + realWorldDeltaY;

                this.drawScene();
            }
            else if (resizingStared)
            {
                int deltaX = e.X - mouseLocationAtMouseDown.X;
                int deltaY = e.Y - mouseLocationAtMouseDown.Y;

                double realWorldDeltaX = this.rangeXAtMouseDown * deltaX / this.scatterPlotViewport.Width;
                this.maxXWindow = this.maxXWindowAtMouseDown - realWorldDeltaX;
                this.rangeX = this.rangeXAtMouseDown - realWorldDeltaX;

                double realWorldDeltaY = this.rangeYAtMouseDown * deltaY / this.scatterPlotViewport.Height;
                this.maxYWindow = this.maxYWindowAtMouseDown + realWorldDeltaY;
                this.rangeY = this.rangeYAtMouseDown + realWorldDeltaY;


                this.drawScene();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draggingStarted = false;
            resizingStared = false;
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            int changeX = this.scatterPlotViewport.Width / 20;
            int changeY = this.scatterPlotViewport.Height * changeX / this.scatterPlotViewport.Width;

            int sign = Math.Sign(e.Delta);


            double realWorldChangeX = this.rangeXAtMouseDown * changeX / this.scatterPlotViewport.Width;
            double realWorldChangeY = this.rangeYAtMouseDown * changeY / this.scatterPlotViewport.Height;

            this.minXWindow -= sign * realWorldChangeX;
            this.rangeX += sign * 2 * realWorldChangeX;

            this.minYWindow -= sign * realWorldChangeY;
            this.rangeY += sign * 2 * realWorldChangeY;


            this.drawScene();
        }

        private double findMaxValue(List<double> values)
        {
            double result = values[0];
            foreach (double v in values)
            {
                if (v > result) result = v;
            }
            return result;
        }

        private double findMinValue(List<double> values)
        {
            double result = values[0];
            foreach (double v in values)
            {
                if (v < result) result = v;
            }
            return result;
        }

    }

    class DataPoint
    {
        public double x;
        public double y;

        public DataPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
