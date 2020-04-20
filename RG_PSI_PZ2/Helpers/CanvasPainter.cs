using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RG_PSI_PZ2.Helpers
{
    public class CanvasPainter
    {
        public int ElementWidth { get; set; }
        private readonly Canvas _canvas;

        public CanvasPainter(Canvas canvas, int elementWidth = 2)
        {
            ElementWidth = elementWidth;
            _canvas = canvas;
        }

        public void PaintToCanvas(GridMap map)
        {
            _canvas.Width = MapColumnToCanvasLeft(map.NumCols + 1);
            _canvas.Height = MapRowToCanvasTop(map.NumRows + 1);

            map.ForEach((row, col, cell) =>
            {
                var el = cell.UIElement;

                el.Height = ElementWidth;
                el.Width = ElementWidth;

                Canvas.SetTop(el, MapRowToCanvasTop(row));
                Canvas.SetLeft(el, MapColumnToCanvasLeft(col));

                _canvas.Children.Add(el);
            });
        }

        private double MapRowToCanvasTop(int row)
        {
            return (row * 2 + 1) * ElementWidth;
        }

        private double MapColumnToCanvasLeft(int column)
        {
            return (column * 2 + 1) * ElementWidth;
        }
    }
}