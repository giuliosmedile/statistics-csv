using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiulioSmedile_CSV
{
    class ContingencyTable
    {
        List<double> xValues;
        List<double> yValues;
        List<Interval> xIntervalList;
        List<Interval> yIntervalList;
        Dictionary<Tuple<Interval, Interval>, int> contingencyMatrix;

        Histogram xHist;
        Histogram yHist;
        Rectangle xHistViewport;
        Rectangle yHistViewport;

        double xIntervalSize;
        double xStartPoint;
        double yIntervalSize;
        double yStartPoint;

        int colWidth;
        int rowHeight;

        public ContingencyTable(List<double> xValues, List<double> yValues)
        {
            this.xValues = xValues;
            this.yValues = yValues;

            this.xIntervalSize = (findMaxValue(xValues) - findMinValue(xValues)) / 10;
            this.xStartPoint = (findMinValue(xValues) + findMaxValue(xValues)) / 2;

            this.yIntervalSize = (findMaxValue(yValues) - findMinValue(yValues)) / 10;
            this.yStartPoint = (findMinValue(yValues) + findMaxValue(yValues)) / 2;

            xIntervalList = calculateContinousDistribution(xValues, xStartPoint, xIntervalSize);
            yIntervalList = calculateContinousDistribution(yValues, yStartPoint, yIntervalSize);

            contingencyMatrix = new Dictionary<Tuple<Interval, Interval>, int>();
            foreach (Interval x in xIntervalList)
            {
                foreach (Interval y in yIntervalList)
                {
                    Tuple<Interval, Interval> t = Tuple.Create(x, y);
                    contingencyMatrix.Add(t, 0);
                }
            }

            foreach (Tuple<Interval, Interval> t in contingencyMatrix.Keys.ToList())
            {
                for (int i = 0; i < xValues.Count; i++)
                {
                    Interval x = t.Item1;
                    Interval y = t.Item2;

                    if (xValues[i] >= x.lowerBound && xValues[i] < x.upperBound && yValues[i] >= y.lowerBound && yValues[i] < y.upperBound)
                    {
                        contingencyMatrix[t]++;
                    }
                }
            }

            xHist = new Histogram(xValues, xIntervalList.Count - 1);
            yHist = new Histogram(yValues, yIntervalList.Count - 1);
        }

        public double findMaxValue(List<double> values)
        {
            double result = values[0];
            foreach (double v in values)
            {
                if (v > result) result = v;
            }
            return result;
        }

        public double findMinValue(List<double> values)
        {
            double result = values[0];
            foreach (double v in values)
            {
                if (v < result) result = v;
            }
            return result;
        }

        public int findMaxDistribution(List<Interval> intervalList)
        {
            int result = 0;
            foreach (Interval interval in intervalList)
            {
                if (interval.count > result) result = interval.count;
            }
            return result;
        }

        public List<Interval> calculateContinousDistribution(List<double> values, double startPoint, double intervalSize)
        {
            List<Interval> intervalList = new List<Interval>();

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
            return intervalList;
        }

        public void drawTable(Graphics g, Rectangle viewport, bool absoluteChecked)
        {

            Font headerFont = new Font(FontFamily.GenericMonospace, 10f, FontStyle.Bold);

            this.colWidth = viewport.Width / (yIntervalList.Count + 1);
            this.rowHeight = viewport.Height / (xIntervalList.Count + 1);

            g.FillRectangle(Brushes.White, viewport);
            for (int col = viewport.Left; col <= viewport.Width; col += colWidth)
            {
                g.DrawLine(Pens.Black, new Point(col, viewport.Top), new Point(col, viewport.Bottom));
            }
            for (int row = viewport.Top; row <= viewport.Bottom; row += rowHeight)
            {
                g.DrawLine(Pens.Black, new Point(viewport.Left, row), new Point(viewport.Right, row));
            }

            //write the first column name
            int count = 1;
            foreach (Interval i in xIntervalList)
            {
                Point p = new Point(viewport.Left + 2, count * rowHeight + viewport.Y);
                String str = "[" + i.lowerBound.ToString("#.##") + "; " + i.upperBound.ToString("#.##") + ")";
                g.DrawString(str, headerFont, Brushes.Black, p);
                count++;
            }

            //write the first row name
            count = 1;
            foreach (Interval i in yIntervalList)
            {
                Point p = new Point(count * colWidth + viewport.X, viewport.Top + 2);
                String str = "[" + i.lowerBound.ToString("#.##") + "; " + i.upperBound.ToString("#.##") + ")";
                g.DrawString(str, headerFont, Brushes.Black, p);
                count++;
            }

            for (int i = 0; i < xIntervalList.Count; i++)
            {
                for (int j = 0; j < yIntervalList.Count; j++)
                {
                    int v = GetValueFromTuple(xIntervalList[i], yIntervalList[j]);
                    Point p = new Point((j + 1) * colWidth + viewport.X, (i + 1) * rowHeight + viewport.Y);
                    if (absoluteChecked)
                    {
                        g.DrawString(v.ToString(), headerFont, Brushes.Black, p);
                    }
                    else
                    {
                        double tmp = (double)v / (double)(xValues.Count + yValues.Count) * 100;
                        string s = tmp == 0 ? "-" : tmp.ToString("0.##") + "%";
                        g.DrawString(s, headerFont, Brushes.Black, p);
                    }
                }
            }

            //finally draw histograms
            xHistViewport = new Rectangle(viewport.X + (int)(0.0833333 * viewport.Width), viewport.Bottom + 10, viewport.Width - (int)(0.0833333 * viewport.Width), 50);
            yHistViewport = new Rectangle(viewport.Right + 10, viewport.Y + (int)(0.0833333 * viewport.Height), 50, viewport.Height - +(int)(0.0833333 * viewport.Height));

            xHist.drawXHistogram(g, xHistViewport);
            yHist.drawYHistogram(g, yHistViewport);
            g.DrawRectangle(Pens.Purple, xHistViewport);
            g.DrawRectangle(Pens.Purple, yHistViewport);
        }

        private int GetValueFromTuple(Interval x, Interval y)
        {
            foreach (Tuple<Interval, Interval> t in contingencyMatrix.Keys.ToList())
            {
                if (x.Equals(t.Item1) && y.Equals(t.Item2)) return contingencyMatrix[t];
            }

            return -1;
        }
    }
}
