using System.Windows;
using System.Windows.Controls;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMap
    {
        private readonly GridMapCell[,] _map;

        public GridMap(int numRows, int numCols)
        {
            _map = new GridMapCell[numRows, numCols];
        }

        public int NumCols => _map.GetLength(1);
        public int NumRows => _map.GetLength(0);

        public void InitGrid(Grid grid)
        {
            for (int i = 0; i < NumRows; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(grid.Width / NumCols) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(grid.Height / NumRows) });
            }
        }
    }
}