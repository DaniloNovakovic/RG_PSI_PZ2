using RG_PSI_PZ2.Helpers;
using RG_PSI_PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PSI_PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GridMap _map = new GridMap(200, 200);

        public MainWindow()
        {
            InitializeComponent();
        }

        public void InitGrid()
        {
            for (int i = 0; i < _map.NumRows; i++)
            {
                DisplayGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Width / _map.NumCols) });
                DisplayGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(Height / _map.NumRows) });
            }
        }

        private void LoadXml()
        {
            var loader = new GeographicXmlLoader();

            var nodeEntities = loader.GetNodeEntities();
            AddToGridMap(nodeEntities, CreateNodeEntityUIElement);

            var substationEntities = loader.GetSubstationEntities();
            AddToGridMap(substationEntities, CreateSubstationEntityUIElement);

            var switchEntities = loader.GetSwitchEntities();
            AddToGridMap(switchEntities, CreateSwitchEntityUIElement);

            DrawGridMapToDisplayGrid();
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
                int gridX = (int)Math.Round(CoordinateConversion.Scale(item.X, minX, maxX, 0, _map.NumRows));
                int gridY = (int)Math.Round(CoordinateConversion.Scale(item.Y, minY, maxY, 0, _map.NumCols));

                var uiElement = createUIElement(item);

                Grid.SetColumn(uiElement, gridY);
                Grid.SetRow(uiElement, gridX);

                _map.Add(gridX, gridY, new GridMapCell(item, uiElement));
            }
        }

        private FrameworkElement CreateNodeEntityUIElement(PowerEntity entity)
        {
            return new Ellipse { Fill = Brushes.Purple, ToolTip = entity };
        }

        private FrameworkElement CreateSubstationEntityUIElement(PowerEntity entity)
        {
            return new Rectangle { Fill = Brushes.OrangeRed, ToolTip = entity };
        }

        private FrameworkElement CreateSwitchEntityUIElement(PowerEntity entity)
        {
            return new Ellipse { Fill = Brushes.GreenYellow, ToolTip = entity };
        }

        private void DrawGridMapToDisplayGrid()
        {
            _map.ForEach((_, __, cell) => DisplayGrid.Children.Add(cell.UIElement));
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            InitGrid();
            LoadXml();
        }
    }
}