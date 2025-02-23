﻿using RG_PSI_PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PSI_PZ2.Helpers
{
    public class CanvasPainter
    {
        public int ElementWidth { get; set; }
        private int ElementHeight { get => ElementWidth; }

        public Brush GridLineStroke { get; set; } = Brushes.Gray;
        public double GridLineStrokeThickness { get; set; } = 0.1;

        public Brush LineEntityStroke { get; set; } = Brushes.Red;
        public double LineEntityStrokeThickness { get; set; } = 0.3;

        public Brush LineCrossFill { get; set; } = Brushes.DarkGray;

        private readonly Canvas _canvas;
        private readonly Action<List<LineEntity>> _onLineClick;

        public CanvasPainter(Canvas canvas, int elementWidth = 2, Action<List<LineEntity>> onLineClick = null)
        {
            ElementWidth = elementWidth;
            _canvas = canvas;
            _onLineClick = onLineClick;
        }

        public void PaintToCanvas(GridMap map)
        {
            _canvas.Width = MapColumnToCanvasLeft(map.NumCols + 1);
            _canvas.Height = MapRowToCanvasTop(map.NumRows + 1);

            DrawGridLines();

            var nodes = new List<FrameworkElement>();
            var lineParts = new List<LinePart>();

            map.ForEach(cell =>
            {
                var el = cell.UIElement;

                if (el == null)
                {
                    if (cell.ConnectedTo.Count <= 2)
                    {
                        return;
                    }
                    el = new Ellipse { Fill = LineCrossFill, Stroke = Brushes.Black, StrokeThickness = 0.1, Height = ElementHeight / 2, Width = ElementWidth / 2 };
                }
                else
                {
                    el.Height = ElementHeight;
                    el.Width = ElementWidth;
                }

                Canvas.SetTop(el, MapRowToCanvasTop(cell.Row) - (el.Height / 2));
                Canvas.SetLeft(el, MapColumnToCanvasLeft(cell.Column) - (el.Width / 2));

                nodes.Add(el);

                cell.Lines.ForEach(line => lineParts.AddRange(CreateLineParts(line)));
            });

            var filteredLines = RemoveDuplicates(lineParts);
            filteredLines.ForEach(line => _canvas.Children.Add(line));
            nodes.ForEach(node => _canvas.Children.Add(node));
        }

        private List<Line> RemoveDuplicates(List<LinePart> parts)
        {
            var comparer = new LineEqualityComparer();
            var lineEntitiesByLine = new Dictionary<Line, List<LineEntity>>(comparer);
            foreach (var part in parts)
            {
                if (!lineEntitiesByLine.TryGetValue(part.UIElement, out var entities))
                {
                    entities = new List<LineEntity>();
                    lineEntitiesByLine.Add(part.UIElement, entities);
                }

                entities.Add(part.Entity);
            }

            foreach (var pair in lineEntitiesByLine)
            {
                AttachHandlersToLine(line: pair.Key, entities: pair.Value);
            }
            return lineEntitiesByLine.Keys.ToList();
        }

        private List<LinePart> CreateLineParts(LineEntity entity)
        {
            var lineParts = new List<LinePart>();
            for (int i = 0; i < entity.Vertices.Count - 1; ++i)
            {
                var first = entity.Vertices[i];
                var second = entity.Vertices[i + 1];

                var uiLine = new Line
                {
                    X1 = MapColumnToCanvasLeft(first.Column),
                    Y1 = MapRowToCanvasTop(first.Row),
                    X2 = MapColumnToCanvasLeft(second.Column),
                    Y2 = MapRowToCanvasTop(second.Row),
                    Stroke = LineEntityStroke,
                    StrokeThickness = LineEntityStrokeThickness,
                    ToolTip = entity
                };
                lineParts.Add(new LinePart { Entity = entity, UIElement = uiLine });
            }
            return lineParts;
        }

        private void AttachHandlersToLine(Line line, List<LineEntity> entities)
        {
            line.MouseRightButtonUp += (s, e) => _onLineClick?.Invoke(entities);
        }

        private void DrawGridLines()
        {
            // Draw Horizontal lines
            for (int i = 0; i < _canvas.Height; i += ElementHeight)
            {
                _canvas.Children.Add(new Line
                {
                    X1 = 0,
                    Y1 = i,
                    X2 = _canvas.Width,
                    Y2 = i,
                    Stroke = GridLineStroke,
                    StrokeThickness = GridLineStrokeThickness
                });
            }

            // Draw Vertical lines
            for (int i = 0; i < _canvas.Width; i += ElementWidth)
            {
                _canvas.Children.Add(new Line
                {
                    X1 = i,
                    Y1 = 0,
                    X2 = i,
                    Y2 = _canvas.Height,
                    Stroke = GridLineStroke,
                    StrokeThickness = GridLineStrokeThickness
                });
            }
        }

        private double MapRowToCanvasTop(int row)
        {
            return row * ElementHeight;
        }

        private double MapColumnToCanvasLeft(int column)
        {
            return column * ElementWidth;
        }
    }
}