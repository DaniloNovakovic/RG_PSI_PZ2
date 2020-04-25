using RG_PSI_PZ2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMap
    {
        private GridMapCell[,] _map;
        private readonly Dictionary<long, GridMapCell> _cellByIdCache;

        public GridMap(int numRows, int numCols)
        {
            _map = new GridMapCell[numRows, numCols];
            _cellByIdCache = new Dictionary<long, GridMapCell>();
        }

        public int NumCols => _map.GetLength(1);
        public int NumRows => _map.GetLength(0);

        public void AddOrUpdate(int x, int y, GridMapCell cell)
        {
            Clip(ref x, ref y);
            _map[x, y] = cell;
            cell.Row = x;
            cell.Column = y;
            if (cell.Id != null)
            {
                _cellByIdCache.Add(cell.Id.Value, cell);
            }
        }

        public void Connect(GridPoint source, GridPoint dest)
        {
            var leftCell = _map[source.Row, source.Column] ?? new GridMapCell { Row = source.Row, Column = source.Column };
            var rightCell = _map[dest.Row, dest.Column] ?? new GridMapCell { Row = dest.Row, Column = dest.Column };

            if (!leftCell.ConnectedTo.Any(p => AreSamePosition(p, dest)))
            {
                leftCell.ConnectedTo.Add(dest);
            }

            if (!rightCell.ConnectedTo.Any(p => AreSamePosition(p, source)))
            {
                rightCell.ConnectedTo.Add(source);
            }

            _map[source.Row, source.Column] = leftCell;
            _map[dest.Row, dest.Column] = rightCell;
        }

        private static bool AreSamePosition(GridPoint left, GridPoint right)
        {
            return left.Row == right.Row && left.Column == right.Column;
        }

        public void Connect(IList<GridPoint> points)
        {
            for (int i = 0; i < points.Count - 1; ++i)
            {
                var source = points[i];
                var dest = points[i + 1];

                Connect(source, dest);
            }
        }

        /// <summary>
        /// Adds extra empty cell around each element in matrix, therefore enlargening the matrix
        /// </summary>
        public void Enlarge()
        {
            int nRows = EnlargeRow(NumRows);
            int nCols = EnlargeColumn(NumCols);
            var newMap = new GridMapCell[nRows, nCols];
            ForEach(cell =>
            {
                cell.Row = EnlargeRow(cell.Row);
                cell.Column = EnlargeColumn(cell.Column);
                newMap[cell.Row, cell.Column] = cell;
            });
            _map = newMap;
        }

        private int EnlargeRow(int row)
        {
            return (row * 2) + 1;
        }

        private int EnlargeColumn(int col)
        {
            return EnlargeRow(col);
        }

        public bool TryAddToClosestAvailable(int x, int y, GridMapCell cell)
        {
            int iterationLimit = Math.Max(NumCols, NumRows);

            for (int depth = 1; depth < iterationLimit; ++depth)
            {
                int bottomRow = x + depth;
                int topRow = x - depth;
                int rightCol = y + depth;
                int leftCol = y - depth;

                for (int i = -depth; i <= depth; ++i)
                {
                    int col = y + i;
                    if (HorizontalTryAdd(col, topRow, bottomRow, cell))
                    {
                        return true;
                    }

                    int row = x + i;
                    if (VerticalTryAdd(row, leftCol, rightCol, cell))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool VerticalTryAdd(int row, int leftCol, int rightCol, GridMapCell cell)
        {
            if (row <= 0 || row >= NumRows)
            {
                return false;
            }

            if (leftCol >= 0 && !IsTaken(row, leftCol))
            {
                AddOrUpdate(row, leftCol, cell);
                return true;
            }
            if (rightCol < NumCols && !IsTaken(row, rightCol))
            {
                AddOrUpdate(row, rightCol, cell);
                return true;
            }
            return false;
        }

        private bool HorizontalTryAdd(int col, int topRow, int bottomRow, GridMapCell cell)
        {
            if (col <= 0 || col >= NumCols)
            {
                return false;
            }

            if (bottomRow < NumRows && !IsTaken(bottomRow, col))
            {
                AddOrUpdate(bottomRow, col, cell);
                return true;
            }
            if (topRow >= 0 && !IsTaken(topRow, col))
            {
                AddOrUpdate(topRow, col, cell);
                return true;
            }
            return false;
        }

        public void Delete(int x, int y)
        {
            Clip(ref x, ref y);
            var cell = _map[x, y];
            if (cell?.Id != null)
            {
                _cellByIdCache.Remove(cell.Id.Value);
                _map[x, y] = null;
            }
        }

        /// <summary>
        /// Calls `action(x, y, cell)` for every cell element.
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<GridMapCell> action)
        {
            for (int x = 0; x < NumRows; ++x)
            {
                for (int y = 0; y < NumCols; ++y)
                {
                    var cell = _map[x, y];
                    if (cell != null)
                    {
                        action(cell);
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
            return _map[x, y] != null;
        }

        public bool IsPowerEntity(int x, int y)
        {
            Clip(ref x, ref y);
            return _map[x, y]?.Id != null;
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