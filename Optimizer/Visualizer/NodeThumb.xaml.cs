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
                
                "X1"  => new SolidColorBrush(Color.FromArgb(255, 137, 140, 255)),
                "X3"  => new SolidColorBrush(Color.FromArgb(255, 255, 137, 181)),
                "X5"  => new SolidColorBrush(Color.FromArgb(255, 255, 220, 137)),
                "X7"  => new SolidColorBrush(Color.FromArgb(255, 144, 212, 247)),
                "X18" => new SolidColorBrush(Color.FromArgb(255, 113, 224, 150)),
                "X20" => new SolidColorBrush(Color.FromArgb(255, 245, 162, 111)),
                "X22" => new SolidColorBrush(Color.FromArgb(255, 102, 141, 229)),
                "X24" => new SolidColorBrush(Color.FromArgb(255, 237, 109, 121)),
                "X16" => new SolidColorBrush(Color.FromArgb(255, 90, 208, 229)),
                "X14" => new SolidColorBrush(Color.FromArgb(255, 218, 151, 224)),
                "X12" => new SolidColorBrush(Color.FromArgb(255, 207, 243, 129)),
                "X10" => new SolidColorBrush(Color.FromArgb(255, 255, 150, 227)),
                "X4"  => new SolidColorBrush(Color.FromArgb(255, 187, 150, 255)),
                "X2"  => new SolidColorBrush(Color.FromArgb(255, 103, 238, 189)),
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