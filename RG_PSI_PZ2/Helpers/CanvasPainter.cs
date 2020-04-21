using RG_PSI_PZ2.Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PSI_PZ2.Helpers
{
    public class CanvasPainter
    {
        public int ElementWidth { get; set; }
        private int ElementHeight { get => ElementWidth; }

        public Brush GridLineStroke { get; set; } = Brushes.Black;
        public double GridLineStrokeThickness { get; set; } = 0.1;

        public Brush LineEntityStroke { get; set; } = Brushes.Red;
        public double LineEntityStrokeThickness { get; set; } = 0.1;

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

                el.Height = ElementHeight;
                el.Width = ElementWidth;

                Canvas.SetTop(el, MapRowToCanvasTop(cell.Row) - (ElementHeight / 2));
                Canvas.SetLeft(el, MapColumnToCanvasLeft(cell.Column) - (ElementWidth / 2));

                _canvas.Children.Add(el);

                cell.Lines.ForEach(DrawLineEntity);
            });
        }

        private void DrawLineEntity(LineEntity line)
        {
            for (int i = 0; i < line.Vertices.Count - 1; ++i)
            {
                var first = line.Vertices[i];
                var second = line.Vertices[i + 1];

                _canvas.Children.Add(new Line
                {
                    X1 = MapRowToCanvasTop((int)first.Row),
                    Y1 = MapColumnToCanvasLeft((int)first.Column),
                    X2 = MapRowToCanvasTop((int)second.Row),
                    Y2 = MapColumnToCanvasLeft((int)second.Column),
                    Stroke = LineEntityStroke,
                    StrokeThickness = LineEntityStrokeThickness
                });
            }
        }

        private void DrawLines()
        {
            // Draw Horizontal lines
            for (int i = 0; i < _canvas.Height; i += ElementHeight)
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
            return row * ElementHeight;
        }

        private double MapColumnToCanvasLeft(int column)
        {
            return column * ElementWidth;
        }
    }
}