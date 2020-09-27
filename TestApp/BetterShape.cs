using System.Net.Sockets;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TestApp
{
    public class BetterShape
    {
        public Shape shape { get; set; }
        // temp
        public SolidColorBrush baseColor { get; private set; }

        public decimal VelocityX { get; set; }
        public decimal VelocityY { get; set; }
        public bool PrevClicked { get; set; }
        public bool IsHeld { get; set; }
        public bool IsColliding { get; set; }

        public double X
        { 
            get { return Canvas.GetLeft(shape) + (this.Width / 2); } 
            set { Canvas.SetLeft(shape, value - (shape.Width / 2)); }
        }
        public double Y 
        {
            get { return Canvas.GetTop(shape) + (this.Height / 2); }
            set { Canvas.SetTop(shape, value - (shape.Height / 2)); }
        }

        public BetterShape(Shape shape)
        {
            this.shape = shape;
            this.VelocityX = 0.0m;
            this.VelocityY = 0.0m;
            this.IsHeld = false;
            this.IsColliding = false;

            baseColor = (SolidColorBrush)shape.Fill;
        }

        public double Height
        {
            get { return this.shape.Height; }
            set { this.shape.Height = value; }
        }

        public double Width
        {
            get { return this.shape.Width; }
            set { this.shape.Width = value; }
        }

        public double Top
        {
            get { return Canvas.GetTop(shape); }
            set { Canvas.SetTop(shape, value); }
        }

        public double Bottom
        {
            get { return Canvas.GetTop(shape) + this.Height; }
            set { Canvas.SetTop(shape, value - this.Height); }
        }

        public double Left
        {
            get { return Canvas.GetLeft(shape); }
            set { Canvas.SetLeft(shape, value); }
        }

        public double Right
        {
            get { return Canvas.GetLeft(shape) + this.Width; }
            set { Canvas.SetLeft(shape, value - this.Width); }
        }

        public int ZIndex
        {
            get { return Canvas.GetZIndex(shape); }
            set { Canvas.SetZIndex(shape, value); }
        }
    }
}
