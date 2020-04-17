using RG_PSI_PZ2.Helpers;
using RG_PSI_PZ2.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace RG_PSI_PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridMap _map = new GridMap(100, 100);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadXml()
        {
            var doc = new XmlDocument();
            doc.Load("Geographic.xml");

            var nodes = doc.DocumentElement.SelectNodes(("/NetworkModel/Nodes/NodeEntity"));
            // TODO: Load nodes and map them to GridMap
            // TODO: Draw GridMap elements to DisplayGrid
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            _map.InitGrid(DisplayGrid);

            LoadXml();
        }
    }
}