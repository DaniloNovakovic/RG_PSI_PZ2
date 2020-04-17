namespace RG_PSI_PZ2.Model
{
    public class Point
    {
        public Point(double X = 0, double Y = 0)
        {
            this.X = X;
            this.Y = Y;
        }

        public double X { get; set; }

        public double Y { get; set; }
    }
}