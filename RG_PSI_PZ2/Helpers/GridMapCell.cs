using RG_PSI_PZ2.Model;
using System.Windows;
using System.Windows.Controls;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMapCell
    {
        /// <summary>
        /// Note: Possible `NullRefferenceException` on `Id` if `Value` prop is `null`.
        /// </summary>
        public GridMapCell()
        {
        }

        public GridMapCell(PowerEntity value, FrameworkElement uiElement)
        {
            Value = value;
            UIElement = uiElement;
        }

        public GridMapCell(int row, int col, PowerEntity value, FrameworkElement uiElement)
        {
            Row = row;
            Column = col;
            Value = value;
            UIElement = uiElement;
        }

        public FrameworkElement UIElement { get; set; }
        public PowerEntity Value { get; set; }

        public long Id { get => Value.Id; }

        public int Row { set; get; } = -1;

        public int Column { set; get; } = -1;
    }
}