using RG_PSI_PZ2.Helpers;
using RG_PSI_PZ2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Linq;
using System.Windows.Shapes;
using System.Windows.Media;

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
            DrawToGrid(nodeEntities);

            Debug.WriteLine($"NodeEntities Loaded: {nodeEntities.Count}");

            // TODO: Draw GridMap elements to DisplayGrid
        }

        private void DrawToGrid(List<NodeEntity> nodeEntities)
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

                var uiElement = new Ellipse { Fill = Brushes.Purple };

                Grid.SetColumn(uiElement, gridY);
                Grid.SetRow(uiElement, gridX);

                DisplayGrid.Children.Add(uiElement);

                _map.Add(gridX, gridY, new GridMapCell { Value = item, UIElement = uiElement });
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            InitGrid();
            LoadXml();
        }
    }
}