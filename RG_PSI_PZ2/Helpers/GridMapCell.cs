using RG_PSI_PZ2.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMapCell : GridPoint
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

        public IEntity Value { get; set; }

        public long Id { get => Value.Id; }

        public List<LineEntity> Lines { get; set; } = new List<LineEntity>();

        public FrameworkElement UIElement { get; set; }
    }
}