using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PSI_PZ2.Helpers
{
    public class CanvasPainter
    {
        public int ElementWidth { get; set; }
        public Brush GridLineStroke { get; set; } = Brushes.Black;
        public double GridLineStrokeThickness { get; set; } = 0.1;

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

            DrawLines();

            map.ForEach(cell =>
            {
                var el = cell.UIElement;

                el.Height = ElementWidth;
                el.Width = ElementWidth;

                Canvas.SetTop(el, MapRowToCanvasTop(cell.Row) - (ElementWidth / 2));
                Canvas.SetLeft(el, MapColumnToCanvasLeft(cell.Column) - (ElementWidth / 2));

                _canvas.Children.Add(el);
            });
        }

        private void DrawLines()
        {
            // Draw Horizontal lines
            for (int i = 0; i < _canvas.Height; i += ElementWidth)
            {
                _canvas.Children.Add(new Line
                {
                    X1 = 0,
                    Y1 = i,
                    X2 = _canvas.Width,
                    Y2 = i,
                    Stroke = GridLineStroke,
                    StrokeThickness = GridLineStrokeThickness
                });
            }

            // Draw Vertical lines
            for (int i = 0; i < _canvas.Width; i += ElementWidth)
            {
                _canvas.Children.Add(new Line
                {
                    X1 = i,
                    Y1 = 0,
                    X2 = i,
                    Y2 = _canvas.Height,
                    Stroke = GridLineStroke,
                    StrokeThickness = GridLineStrokeThickness
                });
            }
        }

        private double MapRowToCanvasTop(int row)
        {
            return row * ElementWidth;
        }

        private double MapColumnToCanvasLeft(int column)
        {
            return column * ElementWidth;
        }
    }
}