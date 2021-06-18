using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Optimizer.Annotations;
using Optimizer.Visualizer;
using Vector = System.Windows.Vector;

namespace Optimizer
{
    public partial class GraphVisualizer : UserControl, INotifyPropertyChanged
    {
        public GraphVisualizer(Graph graph, List<ParkingAssignment> assignments, Score score)
        {
            InitializeComponent();
            DataContext = this;

            ScaleFactor = 1M;

            _assignments = assignments;
            AddNodeInternal(graph.Nodes.First().Value, new Point(100, 100), new Vector(25, 0));
            Score = score;
        }

        public List<Node> addedNodes = new List<Node>();

        private List<ParkingAssignment> _assignments;
        public Score Score { get; set; }

        int rotationindex = 0;

        private void AddNodeInternal(Node node, Point point, Vector direction, int rotation = 0)
        {
            var newPoint = Vector.Add(direction, point);

            if (addedNodes.Contains(node))
                return;

            if (node is Source source)
            {
                var nodeThumb = new SourceThumb(source);
                nodeCanvas.Children.Add(nodeThumb);
                Canvas.SetLeft(nodeThumb, newPoint.X);
                Canvas.SetTop(nodeThumb, newPoint.Y);
                nodeThumb.Select += OnSelectNode;
            }
            else
            {
                var assignment = _assignments.FirstOrDefault(x => x.ParkingSpot == node);
                var nodeThumb = new NodeThumb(node, assignment);
                nodeCanvas.Children.Add(nodeThumb);
                Canvas.SetLeft(nodeThumb, newPoint.X);
                Canvas.SetTop(nodeThumb, newPoint.Y);
                nodeThumb.Select += OnSelectNode;
            }

            //nodeThumb.LayoutTransform = new RotateTransform(rotation, 10, 10);

            addedNodes.Add(node);

            var id = node.Id.Split("-")[0];

            var rotationMatrix = new[]
            {
                90, -90
            };

            foreach (var edge in node.Edges)
            {
                Vector nextDirection = direction;
                int nextRotation = rotation;

                var nextId = edge.Id.Split("-")[0];
                if (nextId != id || edge is Source)
                {
                    var rotationAngle = rotationMatrix[rotationindex];
                    rotationindex += 1;
                    rotationindex %= rotationMatrix.Length;
                    var identity = Matrix.Identity;
                    identity.Rotate(rotationAngle);
                    nextDirection = identity.Transform(direction);
                    nextRotation = rotation + rotationAngle;
                }


                AddNodeInternal(edge, newPoint, nextDirection, nextRotation);
            }

        }

        private void OnSelectNode(Node node)
        {
            SelectedNode = node;
        }

        private Node selectedNode;

        public Node SelectedNode
        {
            get => selectedNode;
            set
            {
                if (Equals(value, selectedNode)) return;
                selectedNode = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
            "ScaleFactor", typeof(Decimal), typeof(GraphVisualizer), new PropertyMetadata(default(Decimal)));

        public Decimal ScaleFactor
        {
            get { return (Decimal) GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        public static readonly DependencyProperty TranslateXProperty = DependencyProperty.Register(
            "TranslateX", typeof(int), typeof(GraphVisualizer), new PropertyMetadata(default(int)));

        public int TranslateX
        {
            get { return (int) GetValue(TranslateXProperty); }
            set { SetValue(TranslateXProperty, value); }
        }

        public static readonly DependencyProperty TranslateYProperty = DependencyProperty.Register(
            "TranslateY", typeof(int), typeof(GraphVisualizer), new PropertyMetadata(default(int)));

        public int TranslateY
        {
            get { return (int) GetValue(TranslateYProperty); }
            set { SetValue(TranslateYProperty, value); }
        }

        public static readonly DependencyProperty ScaleOriginXProperty = DependencyProperty.Register(
            "ScaleOriginX", typeof(int), typeof(GraphVisualizer), new PropertyMetadata(default(int)));

        public int ScaleOriginX
        {
            get { return (int) GetValue(ScaleOriginXProperty); }
            set { SetValue(ScaleOriginXProperty, value); }
        }

        public static readonly DependencyProperty ScaleOriginYProperty = DependencyProperty.Register(
            "ScaleOriginY", typeof(int), typeof(GraphVisualizer), new PropertyMetadata(default(int)));

        public int ScaleOriginY
        {
            get { return (int) GetValue(ScaleOriginYProperty); }
            set { SetValue(ScaleOriginYProperty, value); }
        }

        private Point panningOrigin;

        private void nodeCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            Point position = e.GetPosition(nodeCanvas);
            if (!IsPanning)
                return;
            Vector vector = position - panningOrigin;
            TranslateX = (int) Math.Round(TranslateX + vector.X);
            TranslateY = (int) Math.Round(TranslateY + vector.Y);
        }

        private void nodeCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

            StopPanning();
        }

        private void nodeCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Window window = Window.GetWindow(this);
            Point point = PointFromScreen(new Point(window.Left + window.ActualWidth / 2.0, window.Top + window.ActualHeight / 2.0));
            ScaleOriginX = checked((int) Math.Round(point.X));
            ScaleOriginY = checked((int) Math.Round(point.Y));
            ScaleFactor = new Decimal(Math.Min(Math.Max(0.1, Convert.ToDouble(ScaleFactor) + e.Delta / 1000.0), 10.0));
        }

        private void nodeCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((uint) (e.MiddleButton & MouseButtonState.Pressed) <= 0U)
                return;
            StartPanning(e.GetPosition(nodeCanvas));
        }

        private void nodeCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((uint) (~e.MiddleButton & MouseButtonState.Pressed) <= 0U)
                return;
            StopPanning();
        }

        private bool IsPanning => panningOrigin != new Point();

        private void StartPanning(Point origin) => panningOrigin = origin;

        private void StopPanning() => panningOrigin = new Point();
    }
}