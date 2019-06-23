using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace SpinningDotsWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int numberDots = 255;
        const int T = 20;
        private System.Windows.Threading.DispatcherTimer _timer;
        public double Size {get;set;}
        double[] x = new double[numberDots], y = new double[numberDots];
        double time = 0, deltaTime = 0.002f;//;0.016f;
        int loopCount = 0;
        Line[] lines = new Line[numberDots];
        Ellipse[] dots = new Ellipse[numberDots];

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            for (int i = 0; i < numberDots; i++)
            {
                lines[i] = new Line() { Stroke = new SolidColorBrush(Color.FromArgb((byte)((205f/numberDots*i)+50), 225, 94, 255)), StrokeThickness  = 2 };
                Board.Children.Add(lines[i]);

                dots[i] = new Ellipse() { Fill = new SolidColorBrush(Color.FromArgb((byte)((205f/numberDots*i)+50), 180, 150, 255)), Width=10, Height=10 };
                Board.Children.Add(dots[i]);
            }

            KeyDown += (o, e) => 
            {
                if (_timer.IsEnabled)
                    _timer.Stop();
                else
                    _timer.Start();
            };

            Size = 1.8f;
            SizeSlider.Value = Size;

            _timer = new System.Windows.Threading.DispatcherTimer(DispatcherPriority.Normal);
            _timer.Interval = new TimeSpan(0,0,0,0,5);
            _timer.Tick += _timer_Tick;
            _timer.Start();

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < numberDots; i++)
            {
                x[i] = Size * i * Math.Cos(((2 * Math.PI * i) / T) * (time)) + Board.ActualWidth / 2;
                y[i] = Size * i * Math.Sin(((2 * Math.PI * i) / T) * (time)) + Board.ActualHeight / 2;

                if (i != 0)
                {
                    // positioning was only working with Canvas.Set
                    Canvas.SetLeft(lines[i], x[i]);
                    Canvas.SetTop(lines[i], y[i]);
                    
                    lines[i].X1 = 0;
                    lines[i].Y1 = 0;

                    lines[i].X2 = x[i-1] - x[i];
                    lines[i].Y2 = y[i-1] - y[i];
                }

                Canvas.SetLeft(dots[i], x[i] - dots[i].Width/2);
                Canvas.SetTop(dots[i], y[i] - dots[i].Height/2);
            }

            time += deltaTime;
            loopCount++;

            // save frames
            // RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)Board.ActualWidth, (int)Board.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            // renderTargetBitmap.Render(Board); 
            // PngBitmapEncoder pngImage = new PngBitmapEncoder();
            // pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            // using (Stream fileStream = File.Create($"image_{loopCount:0000000}.png"))
            // {
            //     pngImage.Save(fileStream);
            // }
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Size = e.NewValue;
        }
    }
}
