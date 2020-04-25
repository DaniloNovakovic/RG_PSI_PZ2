using RG_PSI_PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMapCell : GridPoint, IComparable
    {
        /// <summary>
        /// Note: Possible `NullRefferenceException` on `Id` if `Value` prop is `null`.
        /// </summary>
        public GridMapCell()
        {
        }

        public GridMapCell(PowerEntity value, Shape uiElement)
        {
            Value = value;
            UIElement = uiElement;
            Color = uiElement.Fill;
        }

        public GridMapCell(int row, int col, PowerEntity value, Shape uiElement) : this(value, uiElement)
        {
            Row = row;
            Column = col;
        }

        public IEntity Value { get; set; }

        public long? Id { get => Value?.Id; }

        public List<LineEntity> Lines { get; set; } = new List<LineEntity>();

        public List<GridPoint> ConnectedTo { get; set; } = new List<GridPoint>();

        public Brush Color { get; set; } = Brushes.Black;

        public Brush HighlightedColor { get; set; } = Brushes.Pink;

        public Shape UIElement { get; set; }

        public int CompareTo(object obj)
        {
            return ToString().CompareTo(obj?.ToString() ?? "");
        }

        public override string ToString()
        {
            return $"{Id}, {Row}, {Column}";
        }

        public bool Equals(GridMapCell obj)
        {
            return Id == obj.Id && Row == obj.Row && Column == obj.Column;
        }
    }
}