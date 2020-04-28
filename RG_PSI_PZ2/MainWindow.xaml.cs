using RG_PSI_PZ2.Helpers;
using RG_PSI_PZ2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PSI_PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GridMap _map = new GridMap(200, 250);
        private bool _zoomed;
        private readonly double _zoomFactor = 2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadXml()
        {
            var loader = new GeographicXmlLoader();

            // Draw Nodes
            var substationEntities = loader.GetSubstationEntities();
            AddToGridMap(substationEntities, CreateSubstationEntityUIElement);

            var nodeEntities = loader.GetNodeEntities();
            AddToGridMap(nodeEntities, CreateNodeEntityUIElement);

            var switchEntities = loader.GetSwitchEntities();
            AddToGridMap(switchEntities, CreateSwitchEntityUIElement);

            _map.Enlarge();

            // Draw Lines
            var lineEntities = loader.GetLineEntities();
            Debug.WriteLine($"Lines: {lineEntities.Count()}");

            ConnectNodes(lineEntities);
        }

        private void ConnectNodes(IEnumerable<LineEntity> lineEntities)
        {
            foreach (var line in lineEntities)
            {
                var start = _map.GetById(line.FirstEnd);
                var end = _map.GetById(line.SecondEnd);

                if (start == null || end == null || AreConnected(start, end))
                {
                    continue;
                }

                // Prev line is obstacle
                line.Vertices = BFS.GetShortestPath(start, end, _map, isObstacle: IsNotNull);
                if (line.Vertices.Count < 2)
                {
                    // Prev line is not obstacle
                    line.Vertices = BFS.GetShortestPath(start, end, _map, isObstacle: IsPowerEntity);
                }

                start.Lines.Add(line);
                _map.Connect(line.Vertices);
            }
        }

        private bool AreConnected(GridMapCell leftCell, GridMapCell rightCell)
        {
            return leftCell.Lines.Any(line => DoesConnect(line, leftCell, rightCell))
                || rightCell.Lines.Any(line => DoesConnect(line, leftCell, rightCell));
        }

        private bool DoesConnect(LineEntity line, GridMapCell leftCell, GridMapCell rightCell)
        {
            return (line.FirstEnd == leftCell.Id || line.FirstEnd == rightCell.Id)
                && (line.SecondEnd == leftCell.Id || line.SecondEnd == rightCell.Id);
        }

        private bool IsNotNull(int rr, int cc)
        {
            return _map.Get(rr, cc) != null;
        }

        private bool IsPowerEntity(int rr, int cc)
        {
            return IsPowerEntity(_map.Get(rr, cc));
        }

        private bool IsPowerEntity(GridMapCell cell)
        {
            return cell?.Id != null;
        }

        private void AddToGridMap(IEnumerable<PowerEntity> nodeEntities, Func<PowerEntity, Shape> createUIElement)
        {
            var xCoords = nodeEntities.Select(e => e.X).ToList();
            var yCoords = nodeEntities.Select(e => e.Y).ToList();

            double minX = xCoords.Min();
            double maxX = xCoords.Max();
            double minY = yCoords.Min();
            double maxY = yCoords.Max();

            foreach (var item in nodeEntities)
            {
                int row = (int)Math.Round(CoordinateConversion.Scale(item.X, minX, maxX, 0, _map.NumRows));
                int col = (int)Math.Round(CoordinateConversion.Scale(item.Y, minY, maxY, 0, _map.NumCols));

                var uiElement = createUIElement(item);

                if (!_map.TryAddToClosestAvailable(row, col, new GridMapCell(item, uiElement)))
                {
                    Debug.WriteLine($"Failed to add cell closest available to ({row}, {col})");
                }
            }
        }

        private Shape CreateNodeEntityUIElement(PowerEntity entity)
        {
            return CreateEllipse(Brushes.Purple, entity);
        }

        private Shape CreateSubstationEntityUIElement(PowerEntity entity)
        {
            return CreateEllipse(Brushes.OrangeRed, entity);
        }

        private Shape CreateSwitchEntityUIElement(PowerEntity entity)
        {
            return CreateEllipse(Brushes.DarkGreen, entity);
        }

        private Ellipse CreateEllipse(Brush Fill, object ToolTip)
        {
            var e = new Ellipse { Fill = Fill, ToolTip = ToolTip, Stroke = Brushes.Black, StrokeThickness = 0.1 };
            e.MouseLeftButtonDown += OnEllipseMouseClick;
            return e;
        }

        private void OnEllipseMouseClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_zoomed)
            {
                _zoom.Value -= _zoomFactor;
            }
            else
            {
                _zoom.Value += _zoomFactor;
            }
            _zoomed = !_zoomed;

            var ellipse = (Ellipse)sender;
            ellipse.BringIntoView();
        }

        private void DrawMapToCanvas()
        {
            var painter = new CanvasPainter(_canvas, onLineClick: HighlightLineEntities);
            painter.PaintToCanvas(_map);
        }

        private void HighlightLineEntities(List<LineEntity> entities)
        {
            var ids = new List<long>();
            foreach (var entity in entities)
            {
                ids.Add(entity.FirstEnd);
                ids.Add(entity.SecondEnd);
            }
            var cells = ids.Distinct().Select(id => _map.GetById(id)).Where(cell => cell != null).ToList();
            HighlightCells(cells);
        }

        private static readonly List<GridMapCell> prevHighlighted = new List<GridMapCell>();

        private bool ContainsSameElements(List<GridMapCell> first, List<GridMapCell> second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            first.Sort();
            second.Sort();

            for (int i = 0; i < first.Count; ++i)
            {
                var f = first[i];
                var s = second[i];
                if (f.Row != s.Row || f.Column != s.Column)
                {
                    return false;
                }
            }
            return true;
        }

        private void HighlightCells(List<GridMapCell> cellsToHighlight)
        {
            UnhiglightCells(prevHighlighted);

            if (ContainsSameElements(prevHighlighted, cellsToHighlight))
            {
                prevHighlighted.Clear();
                return;
            }

            prevHighlighted.Clear();

            foreach (var cell in cellsToHighlight)
            {
                var shape = cell.UIElement;
                shape.Fill = cell.HighlightedColor;
            }

            prevHighlighted.AddRange(cellsToHighlight);
        }

        private void UnhiglightCells(IEnumerable<GridMapCell> cellsToUnhiglight)
        {
            foreach (var cell in cellsToUnhiglight)
            {
                var shape = cell.UIElement;
                shape.Fill = cell.Color;
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadXml();
            DrawMapToCanvas();
        }
    }
}