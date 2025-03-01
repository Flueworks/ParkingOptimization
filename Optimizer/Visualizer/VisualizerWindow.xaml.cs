﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Optimizer.Optimizers;

namespace Optimizer.Visualizer
{
    public partial class VisualizerWindow : Window
    {
        public VisualizerWindow()
        {
            InitializeComponent();
        }

        public void AddGraph(string name, List<Node> graph, List<ParkingAssignment> assignments, Score score)
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