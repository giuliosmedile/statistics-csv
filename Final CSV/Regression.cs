using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_CSV
{
    class Regression
    {
        List<double> xValues;
        List<double> yValues;

        int n;
        double meanX;
        double meanY;
        double varX;
        double covXY;
        public double m, q;        //parameters for linear regression
        public double a, b, c;     //parameters for quadratic regression

        public Regression(List<double> xValues, List<double> yValues)
        {
            this.xValues = xValues;
            this.yValues = yValues;
            n = this.xValues.Count();
            meanX = 0;
            meanY = 0;
            varX = 0;
            covXY = 0;
        }

        //this will return a line in the form y = mx + q
        private void calculateLinearRegression()
        {
            //running way to calculate variance, covariance and mean
            for (double i = 1; i <= n; i++)
            {
                double x = xValues[(int)i - 1];
                double y = yValues[(int)i - 1];

                double dx = x - meanX;
                double dy = y - meanY;

                varX += (((i - 1) / i) * dx * dx - varX) / i;
                covXY += (((i - 1) / i) * dx * dy - covXY) / i;
                meanX += dx / i;
                meanY += dy / i;
            }

            this.m = covXY / varX;
            this.q = meanY - (m * meanX);
        }

        public void findLinearRegressionIntersectionPoints(double minYWindow, double maxYWindow, out double minXRegression, out double maxXRegression)
        {
            this.calculateLinearRegression();
            minXRegression = (minYWindow - q) / m;
            maxXRegression = (maxYWindow - q) / m;
        }

        public void drawLinearRegression(Graphics g, Rectangle viewport, double minX, double minY, double rangeX, double rangeY)
        {
            this.calculateLinearRegression();

            int size = 25;          //number of points that approximate the parabula
            int start = viewport.X;
            int end = viewport.Right;
            int step = (viewport.Width) / size;

            List<PointF> points = new List<PointF>();

            //iterate through each step of the parabula
            for (int i = 0; i <= size; i++)
            {
                //the x world-value is obtained through reversing the world-to-viewport transformation equation
                double x = xWorld(start + i * step, viewport, minX, rangeX);
                double y = yViewport(linear(x), viewport, minY, rangeY);
                points.Add(new PointF(start + i * step, (float)y));
                //Debug.WriteLine("i: " + i + "; x: " + (start + i * step) + "; y: " + y);
            }

            for (int i = 1; i < size; i++)
            {
                if (viewport.Contains((int)points[i].X, (int)points[i].Y) && viewport.Contains((int)points[i - 1].X, (int)points[i - 1].Y))
                {
                    g.DrawLine(Pens.Green, points[i - 1], points[i]);
                }
            }
        }

        //this will return a parabula in the form y = ax^2 + bx + c
        private void calculateQuadraticRegression()
        {
            //various values for the initial matrix
            double sumx = 0;
            double sumx2 = 0;
            double sumx3 = 0;
            double sumx4 = 0;
            double sumy = 0;
            double sumxy = 0;
            double sumx2y = 0;

            //running way to calculate those values
            for (int i = 0; i < n; i++)
            {
                double x = xValues[i];
                double y = yValues[i];

                sumx += x;
                sumx2 += x * x;
                sumx3 += x * x * x;
                sumx4 += x * x * x * x;
                sumy += y;
                sumxy += x * y;
                sumx2y += x * x * y;
            }

            //the coefficients a, b and c are calculated through a linear equation
            //this is the matrix of the coefficients
            double[,] X = new double[,]
            {
                {sumx4, sumx3, sumx2 },
                {sumx3, sumx2, sumx },
                {sumx2, sumx, n }
            };
            double detX = calculateDeterminant(X);

            //the next three are the matrices needed to solve the equation through the Cramer method
            double [,] aX= new double[,]
            {
                {sumx2y, sumx3, sumx2 },
                {sumxy, sumx2, sumx },
                {sumy, sumx, n }
            };
            double detaX = calculateDeterminant(aX);

            double [,] bX = new double[,]
            {
                {sumx4, sumx2y, sumx2 },
                {sumx3, sumxy, sumx },
                {sumx2, sumy, n }
            };
            double detbX = calculateDeterminant(bX);

            double [,] cX = new double[,]
            {
                {sumx4, sumx3, sumx2y },
                {sumx3, sumx2, sumxy },
                {sumx2, sumx, sumy }
            };
            double detcX = calculateDeterminant(cX);

            //final step of the Cramer method
            a = detaX / detX;
            b = detbX / detX;
            c = detcX / detX;
        }

        //draw a the quadratic regression line given a graphics object and a viewport
        public void drawQuadraticRegression(Graphics g, Rectangle viewport, double minX, double minY, double rangeX, double rangeY)
        {
            this.calculateQuadraticRegression();

            int size = 105;          //number of points that approximate the parabula
            int start = viewport.X;
            int end = viewport.Right;
            int step = (viewport.Width) / size;

            List<PointF> points = new List<PointF>();

            //iterate through each step of the parabula
            for (int i = 0; i <= size; i++)
            {
                //the x world-value is obtained through reversing the world-to-viewport transformation equation
                double x = xWorld(start + i * step, viewport, minX, rangeX);
                double y = yViewport(quadratic(x), viewport, minY, rangeY);
                points.Add(new PointF(start + i * step, (float)y));
                //Debug.WriteLine("i: " + i + "; x: " + (start + i * step) + "; y: " + y);
            }

            for (int i = 1; i < size; i++)
            {
                if (viewport.Contains((int)points[i].X, (int)points[i].Y) && viewport.Contains((int)points[i - 1].X, (int)points[i - 1].Y))
                {
                    g.DrawLine(Pens.Black, points[i - 1], points[i]);
                } 
            }
        }

        //linear regression function
        private double linear(double x)
        {
            return m * x + q;
        }

        //quadratic regression function
        private double quadratic(double x)
        {
            return a * Math.Pow(x, 2D) + b * x + c;
        }

        public double xViewport(double xWorld, Rectangle viewport, double minX, double rangeX)
        {
            return viewport.Left + viewport.Width * (xWorld - minX) / rangeX;
        }

        public double xWorld(double xViewport, Rectangle viewport, double minX, double rangeX)
        {
            return (viewport.Left * rangeX - viewport.Width * minX - xViewport * rangeX) / -viewport.Width;
        }

        public double yViewport(double yWorld, Rectangle viewport, double minY, double rangeY)
        {
            return (viewport.Top + viewport.Height - viewport.Height * (yWorld - minY) / rangeY);
        }

        //Calculates the determinant of a 3x3 matrix
        public double calculateDeterminant(double[,] mat)
        {
            return
                mat[0, 0] * mat[1, 1] * mat[2, 2] +
                mat[0, 1] * mat[1, 2] * mat[2, 0] +
                mat[0, 2] * mat[1, 0] * mat[2, 1] -
                mat[2, 0] * mat[1, 1] * mat[0, 2] -
                mat[2, 1] * mat[1, 2] * mat[0, 0] -
                mat[2, 2] * mat[1, 0] * mat[0, 1];
        }
    }
}
