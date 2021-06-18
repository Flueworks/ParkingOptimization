using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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

            if(addedNodes.Contains(node))
                return;

            if(node is Source source)
            {
                var nodeThumb = new SourceThumb(source);
                nodeCanvas.Children.Add(nodeThumb);
                Canvas.SetLeft(nodeThumb, newPoint.X);
                Canvas.SetTop(nodeThumb, newPoint.Y);
                nodeThumb.Select += OnSelectNode;
            }
            else
            {
                var assignment = _assignments.FirstOrDefault(x=>x.ParkingSpot == node);
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
    }
}