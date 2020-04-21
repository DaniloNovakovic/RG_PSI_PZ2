using RG_PSI_PZ2.Model;
using System.Collections.Generic;
using System.Diagnostics;

namespace RG_PSI_PZ2.Helpers
{
    public class BFS
    {
        // Direction vectors for north, south, east, and west
        private readonly static List<int> dr = new List<int> { -1, +1, 0, 0 };

        private readonly static List<int> dc = new List<int> { 0, 0, +1, -1 };

        public static List<GridPoint> GetShortestPath(GridMapCell start, GridMapCell end, GridMap map)
        {
            var shortestPath = new List<GridPoint>();
            var q = new Queue<GridPoint>();
            var prev = new GridPoint[map.NumRows, map.NumCols];

            q.Enqueue(start);
            prev[start.Row, start.Column] = start;

            while (q.Count > 0)
            {
                var point = q.Dequeue();

                if (point.Row == end.Row && point.Column == end.Column)
                {
                    break;
                }

                for (int i = 0; i < 4; ++i)
                {
                    int rr = point.Row + dr[i];
                    int cc = point.Column + dc[i];

                    // Skip invalid cells. Assume R and C for the number of roms and columns
                    if (rr < 0 || cc < 0) continue;
                    if (rr >= map.NumRows || cc >= map.NumCols) continue;

                    // Skip visited locations or blocked cells
                    if (prev[rr, cc] != null) continue;
                    if ((rr != end.Row || cc != end.Column) && map.IsTaken(rr, cc)) continue;

                    // (rr, cc) is a neighbouring cell of (r,c)
                    q.Enqueue(new GridPoint(rr, cc));
                    prev[rr, cc] = point;
                }
            }

            shortestPath.Add(end);
            var currPrev = prev[end.Row, end.Column];
            while (currPrev != null && currPrev != start)
            {
                shortestPath.Add(currPrev);
                currPrev = prev[currPrev.Row, currPrev.Column];
            }
            shortestPath.Add(currPrev);

            return shortestPath;
        }
    }
}