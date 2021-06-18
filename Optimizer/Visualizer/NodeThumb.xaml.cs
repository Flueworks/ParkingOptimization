using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Optimizer
{
    public partial class NodeThumb : UserControl
    {
        public Node Node { get; }
        public ParkingAssignment Assignment { get; }

        public NodeThumb(Node node, ParkingAssignment assignment)
        {
            Node = node;
            Assignment = assignment;
            FillBrush = GetBranchBrush(node.BranchId);
            if (assignment != null)
            {
                AssignedHotelBrush = GetBranchBrush(assignment.Customer.Hotel);
                Steps = assignment.GetWalkingDistance();
            }

            InitializeComponent();
            Panel.SetZIndex(this, 2);
            DataContext = this;
        }

        public int Steps { get; set; }

        private Brush GetBranchBrush(string branchId)
        {
            return branchId switch
            {
                "A" => Brushes.LightSkyBlue,
                "B" => Brushes.LightGreen,
                "C" => Brushes.LightSalmon,
                "D" => Brushes.LightSeaGreen,
                "E" => Brushes.Khaki,
                "F" => Brushes.Plum,
                "G" => Brushes.BurlyWood,
                "H" => Brushes.LightPink,
                "I" => Brushes.RosyBrown,
                "J" => Brushes.SteelBlue,
                _ => Brushes.Black
            };
        }

        public event Action<Node> Select;

        protected virtual void OnSelect()
        {
            Select?.Invoke(Node);
        }

        public Brush FillBrush { get; set; }
        public Brush AssignedHotelBrush { get; set; }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            OnSelect();
        }
    }
}