using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_CSV
{
    class LinearRegression
    {
        List<double> xValues;
        List<double> yValues;

        int n;
        double meanX;
        double meanY;
        double varX;
        double covXY;
        double m, q;


        public LinearRegression(List<double> xValues, List<double> yValues)
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
            for (double i = 1; i<=n; i++)
            {
                double x = xValues[(int)i-1];
                double y = yValues[(int)i-1];

                double dx = x - meanX;
                double dy = y - meanY;

                varX += (((i - 1) / i) * dx * dx - varX) / i;
                covXY += (((i - 1) / i) * dx * dy - covXY) / i;
                meanX += dx / i;
                meanY += dy / i;
            }

            this.m = covXY / varX;
            this.q = meanY - (m * meanX);
            Debug.WriteLine("m: " + m + "; q: " + q);
        }

        public void findIntersectionPoints(double minYWindow, double maxYWindow, out double minXRegression, out double maxXRegression)
        {
            this.calculateLinearRegression();
            minXRegression = (minYWindow - q) / m;
            maxXRegression = (maxYWindow - q) / m;
        }
        

    }
}
