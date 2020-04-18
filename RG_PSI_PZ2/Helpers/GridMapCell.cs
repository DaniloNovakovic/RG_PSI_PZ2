using RG_PSI_PZ2.Model;
using System.Windows;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMapCell
    {
        public GridMapCell()
        {
        }

        public GridMapCell(PowerEntity entity, FrameworkElement element)
        {
            Value = entity;
            UIElement = element;
        }

        public FrameworkElement UIElement { get; set; }
        public PowerEntity Value { get; set; }
    }
}