#region Using

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

#endregion

namespace Wpf.Canvas.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private Random rnd = new Random();
        private NWpfCanvasManager manager = null;
        private DispatcherTimer timer = null;

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            InitCanvasManager();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            manager = null;

            timer.Stop();
            timer.Tick -= Timer_Tick;
            timer = null;
        }

        #endregion

        #region Timer Tick
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateResourceUsage();
        }

        #endregion

        #region Private Methods

        private void InitCanvasManager()
        {
            manager = new NWpfCanvasManager(this.canvas);

            StopWatch.Start();


            // 100,000 use 1.946 s
            //  50,000 use 1.187 s
            //  10,000 use 0.118 s
            //   5,000 use 0.074 s
            //   1,000 use 0.004 s
            var shapes = CreateLines(5000);
            foreach (var shape in shapes)
            {
                manager.AddShape(shape);
            }

            UpdateExecuteTime(StopWatch.Stop());
        }

        private void UpdateResourceUsage()
        {
            Dispatcher.Invoke(() =>
            {
                var usage = ResourceUsage.GetUsage();
                txtResourceUsage.Text = string.Format("CPU: {0:n2} %, RAM {1:n2} MB", usage.CPU, usage.RAM);
            }, DispatcherPriority.Render);
        }

        private void UpdateExecuteTime(TimeSpan ts)
        {
            Dispatcher.Invoke(() =>
            {
                txtExecuteTime.Text = string.Format("Execute time: {0:n0} ms.", ts.TotalMilliseconds);
            }, DispatcherPriority.Render);
        }

        private List<Shape> CreateLines(int max = 1)
        {
            if (max <= 0) max = 1;
            List<Shape> shapes = new List<Shape>();

            //int maxX = Convert.ToInt32(canvas.ActualWidth);
            //int maxY = Convert.ToInt32(canvas.ActualHeight);
            int maxX = 1200;
            int maxY = 800;

            for (int i = 0; i < max; i++)
            {
                double x1 = Convert.ToDouble(rnd.Next(maxX));
                double y1 = Convert.ToDouble(rnd.Next(maxY));
                double x2 = Convert.ToDouble(rnd.Next(maxX));
                double y2 = Convert.ToDouble(rnd.Next(maxY));
                var brush = new SolidColorBrush(GetRandomColor());

                var shape = manager.CreateLineShape(x1, y1, x2, y2, brush, 2);

                shapes.Add(shape);
            }

            return shapes;
        }

        private Color GetRandomColor()
        {
            byte[] b = new byte[3];
            rnd.NextBytes(b);
            return Color.FromRgb(b[0], b[1], b[2]);
        }

        #endregion
    }
}
