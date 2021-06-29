using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Optimizer.Optimizers;

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
                
                "X1" => Brushes.BurlyWood,
                "X3" => Brushes.BurlyWood,
                "X5" => Brushes.BurlyWood,
                "X7" => Brushes.BurlyWood,
                "X18" => Brushes.BurlyWood,
                "X20" => Brushes.Plum,
                "X22" => Brushes.Plum,
                "X24" => Brushes.Plum,
                "X16" => Brushes.Khaki,
                "X14" => Brushes.LightSeaGreen,
                "X12" => Brushes.LightSeaGreen,
                "X10" => Brushes.LightSalmon,
                "X4" => Brushes.LightSkyBlue,
                "X2" => Brushes.LightGreen,
                _ => Brushes.LightGray
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