using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        decimal velocityX = 0.0m;
        decimal velocityY = 0.0m;
        decimal gravity = 0.0m;

        bool IsGKeyDown { get; set; }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if(!IsGKeyDown) velocityY -= 0.1m;
                    break;
                case Key.Down:
                    if (!IsGKeyDown) velocityY += 0.1m;
                    break;
                case Key.Right:
                    velocityX += 0.1m;
                    break;
                case Key.Left:
                    velocityX -= 0.1m;
                    break;
                case Key.G:
                    IsGKeyDown = true;
                    break;
            }

            if (e.Key == Key.Down && IsGKeyDown) gravity += 0.01m;
            if (e.Key == Key.Up && IsGKeyDown) gravity -= 0.01m;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G)
                IsGKeyDown = false;
        }

        // TO DO: Run as async
        private void timer_Tick(object sender, EventArgs e)
        {
            UpdatePosition(rect1);
            //FreeWindow(rect1);
            BlockedWindow(rect1);
            UpdateLabels();
            CheckCollision();
        }

        // Object can move outside the window
        private void FreeWindow(Shape shape)
        {
            if (Canvas.GetTop(shape) > this.ActualHeight)
                Canvas.SetTop(shape, 0 - shape.Height);
            else if (Canvas.GetTop(shape) + shape.Height < 0)
                Canvas.SetTop(shape, this.ActualHeight);

            if (Canvas.GetLeft(shape) > this.ActualWidth)
                Canvas.SetLeft(shape, 0 - shape.Width);
            else if (Canvas.GetLeft(shape) + shape.Width < 0)
                Canvas.SetLeft(shape, this.ActualWidth);
        }

        // Object cannot move outside the window
        private void BlockedWindow(Shape shape)
        {
            if (Canvas.GetTop(shape) + shape.Height > this.ActualHeight - 35 || Canvas.GetTop(shape) < 0)
                velocityY = -velocityY;

            if (Canvas.GetLeft(shape) + shape.Width > this.ActualWidth - 35 || Canvas.GetLeft(shape) < 0)
                velocityX = -velocityX;
        }

        private void UpdatePosition(Shape shape)
        {
            // remove
            if (!IsHeld)
            {
                velocityY += gravity;
                Canvas.SetTop(shape, Canvas.GetTop(shape) + (double)velocityY);
                Canvas.SetLeft(shape, Canvas.GetLeft(shape) + (double)velocityX);
            }
        }

        private void UpdateLabels()
        {
            label_grav.Content = "Gravity: " + gravity;
            label_vely.Content = "Velocity X: " + velocityX;
            label_velx.Content = "Velocity Y: " + velocityY;
        }

        private void CheckCollision()
        {
            if (RectRectCollision(rect1, rect2)) rect1.Fill = new SolidColorBrush(Colors.LightSalmon);
            else rect1.Fill = new SolidColorBrush(Colors.Aquamarine);

            if (RectCircleCollision(rect1, circle1) || RectCircleCollision(rect2, circle1))
                circle1.Fill = new SolidColorBrush(Colors.LightSalmon);
            else circle1.Fill = new SolidColorBrush(Colors.LightBlue);
        }

        private bool IsMouseDown { get; set; }
        private void canvas_MouseUp(object sender, MouseButtonEventArgs e) => IsMouseDown = false;
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e) => IsMouseDown = true;
        private void rect1_MouseMove(object sender, MouseEventArgs e) => MoveShape(rect1, e);
        private void rect2_MouseMove(object sender, MouseEventArgs e) => MoveShape(rect2, e);
        private void circle1_MouseMove(object sender, MouseEventArgs e) => MoveShape(circle1, e);

        // temp - remove later
        bool IsHeld { get; set; }

        // TO DO: dont reset focus after if !IsMouseDown
        private void MoveShape(Shape shape, MouseEventArgs e)
        {
            if (IsMouseDown)
            {
                IsHeld = true;
                Canvas.SetZIndex(shape, 1);
                Point pos = e.GetPosition(this);
                Canvas.SetLeft(shape, pos.X - (shape.Width / 2));
                Canvas.SetTop(shape, pos.Y - (shape.Height / 2));
            }
            else
            {
                IsHeld = false;
                Canvas.SetZIndex(shape, 0);
            }
        }

        private bool RectRectCollision(Rectangle r1, Rectangle r2)
        {
            if (Canvas.GetLeft(r1) < Canvas.GetLeft(r2) + r2.Width &&
                Canvas.GetLeft(r1) + r1.Width > Canvas.GetLeft(r2) &&
                Canvas.GetTop(r1) < Canvas.GetTop(r2) + r2.Height &&
                 Canvas.GetTop(r1) + r1.Height > Canvas.GetTop(r2)) return true;
            // if false - return which side was collided
            else return false;
        }

        private bool RectCircleCollision(Rectangle r1, Ellipse c1)
        {
            double circleX = Canvas.GetLeft(c1) + (c1.Width / 2);
            double circleY = Canvas.GetTop(c1) + (c1.Height / 2);

            double rectX = Canvas.GetLeft(r1);
            double rectY = Canvas.GetTop(r1);

            if (circleX < rectX) circleX = rectX; // test left edge
            else if (circleX > rectX + r1.Width) circleX = rectX + r1.Width; // right edge

            if (circleY < rectY) circleY = rectY; // top edge
            else if (circleY > rectY + r1.Height) circleY = rectY + r1.Height; // bottom edge

            double distX = (Canvas.GetLeft(c1) + (c1.Width / 2)) - circleX;
            double distY = (Canvas.GetTop(c1) + (c1.Height / 2)) - circleY;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));

            if (distance <= c1.Height / 2)
                return true;
            else return false;
        }
    }
}
