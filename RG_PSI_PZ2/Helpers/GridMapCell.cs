using RG_PSI_PZ2.Model;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMapCell
    {
        public GridMapCell(PowerEntity value = null)
        {
            Value = value;
        }

        public PowerEntity Value { get; set; }
    }
}