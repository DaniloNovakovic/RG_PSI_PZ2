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

        public override string ToString()
        {
            return $"{Row},{Column}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public bool Equals(GridPoint obj)
        {
            return Row == obj.Row && Column == obj.Column;
        }

        public override bool Equals(object obj)
        {
            return ToString().Equals(obj);
        }
    }
}