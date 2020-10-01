using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            dt.Start();

            sps.Add(new BetterShape(rect1));
            sps.Add(new BetterShape(rect2));
            sps.Add(new BetterShape(circle1));

            propwindow = new PropertyWindow(sps, this);
            propwindow.Show();
        }

        Stopwatch dt = new Stopwatch();
        PropertyWindow propwindow;
        List<BetterShape> sps = new List<BetterShape>();
        decimal gravity = 0.0m;
        bool IsGKeyDown { get; set; }
        const double maxFPS = 120;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            foreach (var x in sps)
            {
                if (x.PrevClicked == true)
                {
                    HandleInput(x, e);
                    break;
                }
            }
        }

        private void HandleInput(BetterShape s, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // TO DO: FIX
                case Key.Up:
                    if (!IsGKeyDown) s.VelocityY -= 50.0m;
                    break;
                case Key.Down:
                    if (!IsGKeyDown) s.VelocityY += 50.0m;
                    break;
                case Key.Right:
                    s.VelocityX += 50.0m;
                    break;
                case Key.Left:
                    s.VelocityX -= 50.0m;
                    break;
                case Key.G:
                    IsGKeyDown = true;
                    break;
            }

            if (e.Key == Key.Down && IsGKeyDown) gravity += 10.0m;
            if (e.Key == Key.Up && IsGKeyDown) gravity -= 10.0m;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.G)
                IsGKeyDown = false;
        }

        // TO DO: Run as async
        private void Timer_Tick(object sender, EventArgs e)
        {
            sps.ForEach(x => UpdatePosition(x));
            sps.ForEach(x => BlockedWindow(x));
            //sps.ForEach(x => FreeWindow(x));
            CheckCollision();

            if (dt.ElapsedMilliseconds > 1000 / maxFPS) dt.Restart();

            propwindow.UpdateWindow(sps, gravity, Mouse.GetPosition(this));
        }

        // Object can move outside and return to the window
        private void FreeWindow(BetterShape s)
        {
            if (s.Top > this.ActualHeight) s.Bottom = 0;
            else if (s.Bottom < 0) s.Top = this.ActualHeight;

            if (s.Left > this.ActualWidth) s.Right = 0;
            else if (s.Right < 0)s.Left = this.ActualWidth;
        }

        // Object cannot move outside the window
        private void BlockedWindow(BetterShape s)
        {
            // WPF IS BAD
            if (s.Bottom > this.ActualHeight - 35)
            {
                s.Bottom = this.ActualHeight - 35;
                s.VelocityY = -s.VelocityY;
            }
            else if (s.Top < 0)
            {
                s.Top = 0;
                s.VelocityY = -s.VelocityY;
            }

            if (s.Right > this.ActualWidth - 20)
            {
                s.Right = this.ActualWidth - 20;
                s.VelocityY -= gravity * (decimal)dt.Elapsed.TotalSeconds;
                s.VelocityX = -s.VelocityX;
            }
            else if (s.Left < 0)
            {
                s.Left = 0;
                s.VelocityX = -s.VelocityX;
            }
        }
        
        private void UpdatePosition(BetterShape s)
        {
            if (!s.IsHeld && dt.ElapsedMilliseconds > 1000 / maxFPS)
            {
                s.VelocityY += gravity * (decimal)dt.Elapsed.TotalSeconds;
                s.X += (double)s.VelocityX * dt.Elapsed.TotalSeconds;
                s.Y += (double)s.VelocityY * dt.Elapsed.TotalSeconds;
            }
            else if (s.IsHeld) s.VelocityY = Math.Abs(s.VelocityY);
        }

        private void CheckCollision()
        {
            sps.ForEach(x => x.IsColliding = false);

            for (int i = 0; i < sps.Count - 1; i++)
            {
                for (int j = i + 1; j < sps.Count; j++)
                {
                    if (Collsion.Any(sps[i], sps[j]))
                    {
                        sps[i].IsColliding = true;
                        sps[j].IsColliding = true;
                    }
                }
            }

            foreach (var x in sps)
                if (x.IsColliding) x.shape.Fill = new SolidColorBrush(Colors.LightSalmon);
                else x.shape.Fill = x.baseColor;
        }

        private bool IsMouseDown { get; set; }
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) => IsMouseDown = false;
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) => IsMouseDown = true;
        private void Window_Closed(object sender, EventArgs e) => propwindow.Close();

        private void MoveShape(object sender, MouseEventArgs e)
        {
            int index;
            for (index = 0; index < sps.Count; index++)
                if (sps[index].shape == sender as Shape) break;

            if (IsMouseDown)
            {
                sps[index].IsHeld = true;
                foreach (var x in sps)
                {
                    if (x != sps[index]) x.PrevClicked = false;
                    else x.PrevClicked = true;

                    // BAD
                    x.ZIndex--;
                }

                sps[index].ZIndex = sps.Count - 1;
                sps[index].X = e.GetPosition(this).X;
                sps[index].Y = e.GetPosition(this).Y;
            }
            else sps[index].IsHeld = false;
        }
    }
}
