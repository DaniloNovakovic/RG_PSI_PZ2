using RG_PSI_PZ2.Helpers;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace RG_PSI_PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GridMap _map = new GridMap(100, 100);

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

            Debug.WriteLine($"NodeEntities Loaded: {nodeEntities.Count}");

            // TODO: Draw GridMap elements to DisplayGrid
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            InitGrid();
            LoadXml();
        }
    }
}