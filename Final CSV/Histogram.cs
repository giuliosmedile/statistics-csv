using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GiulioSmedile_CSV
{
    class Histogram
    {
        public List<Double> values = new List<double>();

        List<Interval> intervalList;
        double intervalSize;
        int intervalNumber;
        double startPoint;
        double minVal;
        double maxVal;
        public List<double> quartiles;

        public double findMaxValue()
        {
            double result = values[0];
            foreach (double v in values)
            {
                if (v > result) result = v;
            }
            return result;
        }

        public double findMinValue()
        {
            double result = values[0];
            foreach (double v in values)
            {
                if (v < result) result = v;
            }
            return result;
        }

        public int findMaxDistribution()
        {
            int result = 0;
            foreach (Interval interval in intervalList)
            {
                if (interval.count > result) result = interval.count;
            }
            return result;
        }

        public void calculateContinousDistribution()
        {
            intervalList = new List<Interval>();

            Interval int0 = new Interval();
            int0.lowerBound = startPoint;
            int0.upperBound = int0.lowerBound + intervalSize;
            intervalList.Add(int0);

            foreach (Double v in values)
            {
                bool valueAllocated = false;
                foreach (Interval interval in intervalList)
                {
                    if (v >= interval.lowerBound && v < interval.upperBound)
                    {
                        valueAllocated = true;
                        interval.count++;
                        break;
                    }
                }

                if (valueAllocated)
                {
                    continue;
                }
                else
                {
                    if (v <= intervalList[0].lowerBound)
                    {
                        while (true)
                        {
                            Interval newInterval = new Interval();
                            newInterval.upperBound = intervalList[0].lowerBound;
                            newInterval.lowerBound = newInterval.upperBound - intervalSize;
                            intervalList.Insert(0, newInterval);

                            if (v >= newInterval.lowerBound && v < newInterval.upperBound)
                            {
                                newInterval.count++;
                                break;
                            }
                        }
                    }
                    else if (v > intervalList[intervalList.Count - 1].upperBound)
                    {
                        while (true)
                        {
                            Interval newInterval = new Interval();
                            newInterval.lowerBound = intervalList[intervalList.Count - 1].upperBound;
                            newInterval.upperBound = newInterval.lowerBound + intervalSize;
                            intervalList.Add(newInterval);
                            if (v >= newInterval.lowerBound && v < newInterval.upperBound)
                            {
                                newInterval.count++;
                                break;
                            }
                        }
                    }
                }

            }
        }

        public Histogram(List<Double> values, int intervalNumber)
        {
            this.values = values;
            this.intervalNumber = intervalNumber;
            this.intervalSize = (findMaxValue() - findMinValue()) / intervalNumber;
            this.startPoint = (findMinValue() + findMaxValue()) / 2;
            this.quartiles = new List<double>();
        }
        public void drawXHistogram(Graphics g, Rectangle viewport)
        {
            calculateContinousDistribution();

            minVal = findMinValue();
            maxVal = findMaxValue();
            int maxDistr = findMaxDistribution();
            int histogramBarWidth = viewport.Width / intervalList.Count;
            using (Pen pen = new Pen(Color.White, 3))
            {
                int i = 0;
                foreach (Interval interval in intervalList)
                {
                    double height = Interpolate(interval.count, 0, maxDistr, 0, viewport.Height);
                    Rectangle bar = new Rectangle(viewport.X + i * histogramBarWidth + 1, viewport.Y, histogramBarWidth, (int)height);
                    using (Brush brush = new SolidBrush(Color.Green))
                    {
                        g.FillRectangle(brush, bar);
                        g.DrawRectangle(pen, bar.X, bar.Y, bar.Width, bar.Height);
                    }
                    i++;
                }
            }


            using (Pen pen = new Pen(Color.Blue, 0))
            {
                double avg = values.Average();
                double avgLocation = Interpolate(avg, minVal, maxVal, viewport.X, viewport.X + viewport.Width);
                g.DrawLine(pen, new Point((int)avgLocation, viewport.Y), new Point((int)avgLocation, viewport.Y + viewport.Height));
            }

            xQuartile(true, values, values.Count, g, viewport);

        }

        public void drawYHistogram(Graphics g, Rectangle viewport)
        {
            calculateContinousDistribution();

            minVal = findMinValue();
            maxVal = findMaxValue();
            int maxDistr = findMaxDistribution();
            int histogramBarHeight = viewport.Height / intervalList.Count;
            using (Pen pen = new Pen(Color.White, 3))
            {
                int i = 0;
                foreach (Interval interval in intervalList)
                {
                    double width = Interpolate(interval.count, 0, maxDistr, 0, viewport.Width);
                    Rectangle bar = new Rectangle(viewport.X, viewport.Y + viewport.Height - histogramBarHeight - i * histogramBarHeight, (int)width, histogramBarHeight);
                    using (Brush brush = new SolidBrush(Color.Green))
                    {
                        g.FillRectangle(brush, bar);
                        g.DrawRectangle(pen, bar.X, bar.Y, bar.Width, bar.Height);
                    }
                    i++;
                }
            }

            using (Pen pen = new Pen(Color.Blue, 0))
            {
                double avg = values.Average();
                double avgLocation = Interpolate(avg, minVal, maxVal, viewport.Y + viewport.Height, viewport.Y);
                g.DrawLine(pen, new Point(viewport.X, (int)avgLocation), new Point(viewport.X + viewport.Width, (int)avgLocation));
            }

            yQuartile(true, values, values.Count, g, viewport);
        }

        static double Interpolate(double Value, double OldMin, double OldMax, double NewMin, double NewMax)
        {
            //Linear interpolation
            return ((NewMax - NewMin) / (OldMax - OldMin)) * (Value - OldMin) + NewMin;
        }

        private void xQuartile(bool check, List<double> listX, int lenght, Graphics g, Rectangle viewport)
        {
            List<double> sortX = new List<double>();


            double median = 0.0;
            sortX = listX.OrderBy(i => i).ToList();
            int n = lenght;

            if (n % 2 == 0)
            {
                median = (double)((double)sortX[n / 2 - 1] + (double)sortX[n / 2]) / 2;
                if (check)
                {
                    xQuartile(false, sortX.GetRange(0, n / 2), n / 2, g, viewport);
                    xQuartile(false, sortX.GetRange(n / 2, n / 2), n / 2, g, viewport);
                    check = false;
                }
            }
            else
            {
                median = (double)(sortX[n / 2]);
                if (check)
                {
                    xQuartile(false, sortX.GetRange(0, n / 2), n / 2, g, viewport);
                    xQuartile(false, sortX.GetRange(n / 2 + 1, n / 2), n / 2, g, viewport);
                    check = false;
                }
            }
            quartiles.Add(median);

            int xPoint = (int)(viewport.Left + ((median - minVal) / (maxVal - minVal)) * viewport.Width);
            int yPoint = viewport.Top + viewport.Height;

            Point pX = new Point(xPoint, yPoint);
            Point pY = new Point(xPoint, viewport.Top);

            g.DrawLine(new Pen(Brushes.Red, 2), pX, pY);
        }

        private void yQuartile(bool check, List<double> listX, int lenght, Graphics g, Rectangle viewport)
        {
            List<double> sortX = new List<double>();


            double median = 0.0;
            sortX = listX.OrderBy(i => i).ToList();
            int n = lenght;

            if (n % 2 == 0)
            {
                median = (double)((double)sortX[n / 2 - 1] + (double)sortX[n / 2]) / 2;
                if (check)
                {
                    yQuartile(false, sortX.GetRange(0, n / 2), n / 2, g, viewport);
                    yQuartile(false, sortX.GetRange(n / 2, n / 2), n / 2, g, viewport);
                    check = false;
                }
            }
            else
            {
                median = (double)(sortX[n / 2]);
                if (check)
                {
                    yQuartile(false, sortX.GetRange(0, n / 2), n / 2, g, viewport);
                    yQuartile(false, sortX.GetRange(n / 2 + 1, n / 2), n / 2, g, viewport);
                    check = false;
                }
            }
            quartiles.Add(median);


            int yPoint = (int)(viewport.Top + viewport.Height - ((median - minVal) / (maxVal - minVal)) * viewport.Height);
            int xPoint = viewport.Left + viewport.Width;

            Point pX = new Point(viewport.Left, yPoint);
            Point pY = new Point(xPoint, yPoint);

            g.DrawLine(new Pen(Brushes.Red, 2), pX, pY);

        }

    }
}
