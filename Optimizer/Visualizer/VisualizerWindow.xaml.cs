using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Optimizer.Visualizer
{
    public partial class VisualizerWindow : Window
    {
        public VisualizerWindow()
        {
            InitializeComponent();
        }

        public void AddGraph(string name, Graph graph, List<ParkingAssignment> assignments, Score score)
        {
            TabItem item = new TabItem()
            {
                Header = name,
                Content = new GraphVisualizer(graph, assignments, score)
            };
            Tabs.Items.Add(item);
        }
    }
}