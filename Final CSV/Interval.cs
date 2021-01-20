namespace Final_CSV
{
    class Interval
    {
        public double lowerBound;
        public double upperBound;
        public int count;

        public override bool Equals(object obj)
        {
            Interval that = (Interval)obj;
            return this.lowerBound == that.lowerBound && this.upperBound == that.upperBound;
        }
    }
}
