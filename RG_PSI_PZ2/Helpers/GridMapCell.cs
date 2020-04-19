﻿using RG_PSI_PZ2.Model;
using System.Windows;
using System.Windows.Controls;

namespace RG_PSI_PZ2.Helpers
{
    public class GridMapCell
    {
        public GridMapCell(PowerEntity value, FrameworkElement uiElement)
        {
            Value = value;
            UIElement = uiElement;
        }

        public FrameworkElement UIElement { get; set; }
        public PowerEntity Value { get; set; }

        public long Id { get => Value.Id; }
        public int Row { get => Grid.GetRow(UIElement); }
        public int Column { get => Grid.GetColumn(UIElement); }
    }
}