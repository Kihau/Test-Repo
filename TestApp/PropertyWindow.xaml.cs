using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for PropertyWindow.xaml
    /// Changing object proporties from MainWindow here
    /// </summary>
    public partial class PropertyWindow : Window
    {
        public PropertyWindow(List<BetterShape> sps, Window w)
        {
            InitializeComponent();
            this.window = w;
            UpdateWindow(sps, 0.0m);
        }

        Window window;

        public void UpdateWindow(List<BetterShape>sps, decimal gravity)
        {
            /// ???
            shapeView.ItemsSource = new List<BetterShape>(sps);

            labal_grav.Content = "Gravity: " + gravity;
            labal_height.Content = "Height: " + window.ActualHeight;
            labal_width.Content = "Width: " + window.ActualWidth;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            window.Close();
        }
    }
}
