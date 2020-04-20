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
            _canvas.Width = map.NumCols * ElementWidth;
            _canvas.Height = map.NumRows * ElementWidth;

            map.ForEach((row, col, cell) =>
            {
                var el = cell.UIElement;
                el.Height = ElementWidth;
                el.Width = ElementWidth;
                Canvas.SetTop(el, row * ElementWidth);
                Canvas.SetLeft(el, col * ElementWidth);
                _canvas.Children.Add(el);
            });
        }
    }
}