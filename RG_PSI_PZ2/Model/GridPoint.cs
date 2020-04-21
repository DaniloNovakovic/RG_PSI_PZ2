namespace RG_PSI_PZ2.Model
{
    public class GridPoint
    {
        public GridPoint(int row = -1, int column = -1)
        {
            this.Row = row;
            this.Column = column;
        }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}