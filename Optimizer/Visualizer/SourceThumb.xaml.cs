using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Optimizer.Visualizer
{
    public partial class SourceThumb : UserControl
    {
        public Source Source { get; }

        public SourceThumb(Source source)
        {
            Source = source;
            FillBrush = GetBranchBrush(source.BranchId);
            InitializeComponent();
            Panel.SetZIndex(this, 2);
            DataContext = this;
        }

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
            Select?.Invoke(Source);
        }

        public Brush FillBrush { get; set; }
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            OnSelect();
        }
    }
}