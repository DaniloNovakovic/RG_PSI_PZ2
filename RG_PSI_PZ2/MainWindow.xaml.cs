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

                if (start == null || end == null)
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

        private bool IsNotNull(int rr, int cc)
        {
            return _map.Get(rr, cc) != null;
        }

        private bool IsPowerEntity(int rr, int cc)
        {
            return _map.Get(rr, cc)?.Id != null;
        }

        private void AddToGridMap(IEnumerable<PowerEntity> nodeEntities, Func<PowerEntity, FrameworkElement> createUIElement)
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

        private FrameworkElement CreateNodeEntityUIElement(PowerEntity entity)
        {
            return new Ellipse { Fill = Brushes.Purple, ToolTip = entity };
        }

        private FrameworkElement CreateSubstationEntityUIElement(PowerEntity entity)
        {
            return new Ellipse { Fill = Brushes.OrangeRed, ToolTip = entity };
        }

        private FrameworkElement CreateSwitchEntityUIElement(PowerEntity entity)
        {
            return new Ellipse { Fill = Brushes.DarkGreen, ToolTip = entity };
        }

        private void DrawMapToCanvas()
        {
            var painter = new CanvasPainter(DisplayCanvas);
            painter.PaintToCanvas(_map);
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            LoadXml();
            DrawMapToCanvas();
        }
    }
}