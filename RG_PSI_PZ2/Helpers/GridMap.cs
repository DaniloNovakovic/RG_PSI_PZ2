using System;
using System.Collections.Generic;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMap
    {
        private readonly GridMapCell[,] _map;
        private readonly Dictionary<long, GridMapCell> _cellByIdCache;

        public GridMap(int numRows, int numCols)
        {
            _map = new GridMapCell[numRows, numCols];
            _cellByIdCache = new Dictionary<long, GridMapCell>();
        }

        public int NumCols => _map.GetLength(1);
        public int NumRows => _map.GetLength(0);

        public void Add(int x, int y, GridMapCell cell)
        {
            Clip(ref x, ref y);
            _map[x, y] = cell;
            _cellByIdCache.Add(cell.Id, cell);
        }

        public void Delete(int x, int y)
        {
            Clip(ref x, ref y);
            var cell = _map[x, y];
            if (cell != null)
            {
                _cellByIdCache.Remove(cell.Id);
                _map[x, y] = null;
            }
        }

        /// <summary>
        /// Calls `action(x, y, cell)` for every cell element
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<int, int, GridMapCell> action)
        {
            for (int x = 0; x < NumRows; ++x)
            {
                for (int y = 0; y < NumCols; ++y)
                {
                    var cell = _map[x, y];
                    if (cell != null)
                    {
                        action(x, y, cell);
                    }
                }
            }
        }

        public GridMapCell Get(int x, int y)
        {
            Clip(ref x, ref y);
            return _map[x, y];
        }

        public GridMapCell GetById(long id)
        {
            _cellByIdCache.TryGetValue(id, out var cell);
            return cell;
        }

        public bool IsTaken(int x, int y)
        {
            Clip(ref x, ref y);
            return _map[x, y] == null;
        }

        private void Clip(ref int x, ref int y)
        {
            x = Clip(x, min: 0, max: NumRows - 1);
            y = Clip(y, min: 0, max: NumCols - 1);
        }

        private int Clip(int value, int min, int max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }
            return value;
        }
    }
}