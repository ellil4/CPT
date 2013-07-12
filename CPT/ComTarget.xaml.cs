using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPT
{
    /// <summary>
    /// ComTarget.xaml 的互動邏輯
    /// </summary>
    /// 

    public enum ComState
    {
        OFF, TARGET, NON_TARGET
    }

    public partial class ComTarget : UserControl
    {
        public static int GAP = 45;
        public static int SIZE = 600;

        public ComTarget()
        {
            InitializeComponent();
        }

        public void SetState(ComState state)
        {
            switch (state)
            {
                case ComState.OFF:
                    this.Visibility = Visibility.Hidden;
                    break;
                case ComState.TARGET:
                    this.Visibility = Visibility.Visible;
                    Canvas.SetTop(amRect, GAP);
                    break;
                case ComState.NON_TARGET:
                    this.Visibility = Visibility.Visible;
                    Canvas.SetTop(amRect, SIZE - amRect.Height - GAP);
                    break;
            }
        }
    }
}
