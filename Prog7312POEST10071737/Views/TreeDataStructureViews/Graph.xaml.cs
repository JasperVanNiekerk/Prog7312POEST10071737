using Prog7312POEST10071737.Models;
using Prog7312POEST10071737.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Prog7312POEST10071737.Views.TreeDataStructureViews
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : UserControl
    {
        /// <summary>
        /// The graph object.
        /// </summary>
        private Services.Graph _graph;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Dictionary to store the UI elements of the nodes.
        /// </summary>
        private Dictionary<string, UIElement> _nodeElements;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The last mouse position.
        /// </summary>
        private Point _lastMousePosition;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Flag indicating if dragging is in progress.
        /// </summary>
        private bool _isDragging;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The zoom level of the graph.
        /// </summary>
        private double _zoom = 1.0;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// The singleton instance of the UserSingleton class.
        /// </summary>
        private readonly UserSingleton _userSingleton;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes a new instance of the Graph class.
        /// </summary>
        public Graph()
        {
            InitializeComponent();
            _userSingleton = UserSingleton.Instance;
            _nodeElements = new Dictionary<string, UIElement>();
            _graph = new Services.Graph();

            InitializeGraph();
            SetupEventHandlers();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Initializes the graph by creating category nodes and issue report nodes.
        /// </summary>
        private void InitializeGraph()
        {
            // Create category nodes
            var categories = Category.GetAllCategories();
            foreach (var category in categories)
            {
                _graph.AddNode(
                    $"cat_{category.CategoryName}",
                    category.CategoryName,
                    NodeType.Category,
                    category
                );
            }

            // Create issue report nodes and edges
            foreach (var report in _userSingleton.IssueReports)
            {
                _graph.AddNode(
                    $"report_{report.Id}",
                    report.name,
                    NodeType.IssueReport,
                    report
                );

                // Connect report to its category
                _graph.AddEdge($"cat_{report.Category}", $"report_{report.Id}");
            }

            DrawGraph();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Draws the graph on the canvas.
        /// </summary>
        private void DrawGraph()
        {
            GraphCanvas.Children.Clear();
            _nodeElements.Clear();

            var nodes = _graph.GetNodes().ToList();
            var radius = 30.0;

            // Calculate the size needed for the graph
            double graphWidth = 400;
            double graphHeight = 400;

            // Calculate the center point of the canvas with offsets
            var xOffset = 250; // Adjust this value to move right
            var yOffset = 250; // Adjust this value to move down

            var centerX = (GraphCanvas.ActualWidth / 2) + xOffset;
            var centerY = (GraphCanvas.ActualHeight / 2) + yOffset;

            // If canvas size is not yet set, use default values
            if (centerX == 0) centerX = (475 / 2) + xOffset;
            if (centerY == 0) centerY = (395 / 2) + yOffset;

            // Position nodes in a circle
            for (int i = 0; i < nodes.Count; i++)
            {
                var angle = 2 * Math.PI * i / nodes.Count;
                var x = centerX + (graphWidth / 2) * Math.Cos(angle);
                var y = centerY + (graphHeight / 2) * Math.Sin(angle);

                var node = CreateNodeElement(nodes[i], x, y, radius);
                _nodeElements[nodes[i].Id] = node;
                GraphCanvas.Children.Add(node);
            }

            // Draw edges
            foreach (var edge in _graph.GetEdges())
            {
                if (_nodeElements.TryGetValue(edge.Source, out var sourceElement) &&
                    _nodeElements.TryGetValue(edge.Target, out var targetElement))
                {
                    var line = new Line
                    {
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1
                    };

                    var sourcePos = GetElementCenter((FrameworkElement)sourceElement);
                    var targetPos = GetElementCenter((FrameworkElement)targetElement);

                    line.X1 = sourcePos.X;
                    line.Y1 = sourcePos.Y;
                    line.X2 = targetPos.X;
                    line.Y2 = targetPos.Y;

                    GraphCanvas.Children.Insert(0, line);
                }
            }
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Creates a UI element for a graph node.
        /// </summary>
        /// <param name="node">The graph node.</param>
        /// <param name="x">The x-coordinate of the node.</param>
        /// <param name="y">The y-coordinate of the node.</param>
        /// <param name="radius">The radius of the node.</param>
        /// <returns>The UI element representing the node.</returns>
        private UIElement CreateNodeElement(GraphNode node, double x, double y, double radius)
        {
            var ellipse = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = node.Type == NodeType.Category ? Brushes.LightBlue : Brushes.LightGreen,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            var text = new TextBlock
            {
                Text = node.Label,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Width = radius * 2
            };

            var panel = new StackPanel();
            panel.Children.Add(ellipse);
            panel.Children.Add(text);

            // Move the text below the ellipse
            text.Margin = new Thickness(0, 5, 0, 0);

            Canvas.SetLeft(panel, x - radius);
            Canvas.SetTop(panel, y - radius);

            panel.MouseLeftButtonDown += (s, e) => OnNodeClick(node);
            panel.Tag = node;

            return panel;
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Gets the center point of a UI element.
        /// </summary>
        /// <param name="element">The UI element.</param>
        /// <returns>The center point of the element.</returns>
        private Point GetElementCenter(FrameworkElement element)
        {
            if (element is StackPanel panel && panel.Children[0] is Ellipse ellipse)
            {
                // Calculate center based on the Ellipse position and size
                var x = Canvas.GetLeft(panel) + ellipse.Width / 2;
                var y = Canvas.GetTop(panel) + ellipse.Height / 2;

                // Adjust y to account for the StackPanel layout
                y -= (panel.ActualHeight - ellipse.Height) / 2;

                return new Point(x, y);
            }

            // Fallback for non-StackPanel elements
            return new Point(
                Canvas.GetLeft(element) + element.ActualWidth / 2,
                Canvas.GetTop(element) + element.ActualHeight / 2
            );
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Handles the click event on a graph node.
        /// </summary>
        /// <param name="node">The clicked graph node.</param>
        private void OnNodeClick(GraphNode node)
        {
            // Clear previous highlighting
            foreach (var element in _nodeElements.Values)
            {
                if (element is StackPanel panel && panel.Children[0] is Ellipse ellipse)
                {
                    ellipse.Fill = node.Type == NodeType.Category ? Brushes.LightBlue : Brushes.LightGreen;
                }
            }

            // Highlight selected node and its neighbors
            if (_nodeElements.TryGetValue(node.Id, out var selectedElement))
            {
                if (selectedElement is StackPanel selectedPanel &&
                    selectedPanel.Children[0] is Ellipse selectedEllipse)
                {
                    selectedEllipse.Fill = Brushes.Yellow;
                }

                foreach (var neighbor in node.Neighbors)
                {
                    if (_nodeElements.TryGetValue(neighbor.Id, out var neighborElement))
                    {
                        if (neighborElement is StackPanel neighborPanel &&
                            neighborPanel.Children[0] is Ellipse neighborEllipse)
                        {
                            neighborEllipse.Fill = Brushes.Orange;
                        }
                    }
                }
            }

            // Update details panel
            UpdateDetailsPanel(node);
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Updates the details panel with information about the selected node.
        /// </summary>
        /// <param name="node">The selected graph node.</param>
        private void UpdateDetailsPanel(GraphNode node)
        {
            var details = new StringBuilder();
            details.AppendLine($"Type: {node.Type}");
            details.AppendLine($"Label: {node.Label}");

            if (node.Type == NodeType.IssueReport && node.Data is IssueReport report)
            {
                details.AppendLine($"Location: {report.Location}");
                details.AppendLine($"Status: {report.Status}");
                details.AppendLine($"Description: {report.Description}");
                details.AppendLine($"Subscribers: {report.SubscribedUsers?.Count ?? 0}");
            }

            NodeDetailsText.Text = details.ToString();
        }
        //___________________________________________________________________________________________________________

        /// <summary>
        /// Sets up event handlers for the graph canvas.
        /// </summary>
        private void SetupEventHandlers()
        {
            GraphCanvas.MouseWheel += (s, e) =>
            {
                var delta = e.Delta > 0 ? 0.1 : -0.1;
                _zoom = Math.Max(0.1, Math.Min(2.0, _zoom + delta));
                GraphScale.ScaleX = GraphScale.ScaleY = _zoom;
            };

            GraphCanvas.MouseLeftButtonDown += (s, e) =>
            {
                _isDragging = true;
                _lastMousePosition = e.GetPosition(GraphCanvas);
                GraphCanvas.CaptureMouse();
            };

            GraphCanvas.MouseLeftButtonUp += (s, e) =>
            {
                _isDragging = false;
                GraphCanvas.ReleaseMouseCapture();
            };

            GraphCanvas.MouseMove += (s, e) =>
            {
                if (_isDragging)
                {
                    var currentPos = e.GetPosition(GraphCanvas);
                    var delta = currentPos - _lastMousePosition;
                    GraphTranslate.X += delta.X;
                    GraphTranslate.Y += delta.Y;
                    _lastMousePosition = currentPos;
                }
            };

            _userSingleton.IssueReports.CollectionChanged += (s, e) =>
            {
                InitializeGraph();
            };
        }
    }
}
//____________________________________EOF_________________________________________________________________________