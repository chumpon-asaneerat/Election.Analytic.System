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

using NLib.Services;

using PPRP;
using PPRP.Models.Maps;

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
            NResourceMonitor.Instance.Start();

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

            NResourceMonitor.Instance.Shutdown();
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


            /*
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
            */

            this.Dispatcher.Invoke(() =>
            {
                LoadMaps();
                UpdateExecuteTime(StopWatch.Stop());
            });
        }

        // Transformation from lon/lat to canvas coordinates.
        private TransformGroup shapeTransform;

        // Combined view transformation (zoom and pan).
        private TransformGroup viewTransform = new TransformGroup();
        private ScaleTransform zoomTransform = new ScaleTransform();
        private TranslateTransform panTransform = new TranslateTransform();

        private void LoadMaps()
        {
            string fileName = System.IO.Path.Combine(NJson.LocalDataFolder, @"Maps\Thailand.json");
            var map = JsonMapFileManager.Load(fileName);
            if (null == map)
                return;
            DisplayShape(map);
        }

        /// <summary>
        /// Computes a transformation so that the shapefile geometry
        /// will maximize the available space on the canvas and be
        /// perfectly centered as well.
        /// </summary>
        /// <param name="info">Shapefile information.</param>
        /// <returns>A transformation object.</returns>
        private TransformGroup CreateShapeTransform(System.Windows.Controls.Canvas canvas, JsonShapeFile jshapeFile)
        {
            // Bounding box for the shapefile.

            //double xmin = jshapeFile.Bound.Left;
            //double xmax = jshapeFile.Bound.Right;
            //double ymin = jshapeFile.Bound.Top;
            //double ymax = jshapeFile.Bound.Bottom;

            // Bounding box for the shapes.
            double xmin = 0;
            double xmax = 0;
            double ymin = 0;
            double ymax = 0;

            foreach (var jshape in jshapeFile.Shapes)
            {
                for (int i = 0; i < jshape.Parts.Count; ++i)
                {
                    var jpart = jshape.Parts[i];
                    for (int j = 0; j < jpart.Count; ++j)
                    {
                        var x = jpart.Points[j, 0];
                        var y = jpart.Points[j, 1];

                        if (j == 0)
                        {
                            xmin = x;
                            xmax = x;
                            ymin = y;
                            ymax = y;
                        }
                        else
                        {
                            xmin = Math.Min(xmin, x);
                            xmax = Math.Max(xmax, x);
                            ymin = Math.Min(ymin, y);
                            ymax = Math.Max(ymax, y);
                        }
                    }
                }
            }

            // Width and height of the bounding box.
            double width = Math.Abs(xmax - xmin);
            double height = Math.Abs(ymax - ymin);

            // Aspect ratio of the bounding box.
            double aspectRatio = width / height;

            // Aspect ratio of the canvas.
            double canvasRatio = canvas.ActualWidth / canvas.ActualHeight;

            // Compute a scale factor so that the shapefile geometry
            // will maximize the space used on the canvas while still
            // maintaining its aspect ratio.
            double scaleFactor = 1.0;
            if (aspectRatio < canvasRatio)
                scaleFactor = canvas.ActualHeight / height;
            else
                scaleFactor = canvas.ActualWidth / width;

            // Compute the scale transformation. Note that we flip
            // the Y-values because the lon/lat grid is like a cartesian
            // coordinate system where Y-values increase upwards.
            ScaleTransform xformScale = new ScaleTransform(scaleFactor, -scaleFactor);

            // Compute the translate transformation so that the shapefile
            // geometry will be centered on the canvas.
            TranslateTransform xformTrans = new TranslateTransform();
            xformTrans.X = (canvas.ActualWidth - (xmin + xmax) * scaleFactor) / 2;
            xformTrans.Y = (canvas.ActualHeight + (ymin + ymax) * scaleFactor) / 2;

            // Add the two transforms to a transform group.
            TransformGroup xformGroup = new TransformGroup();
            xformGroup.Children.Add(xformScale);
            xformGroup.Children.Add(xformTrans);

            return xformGroup;
        }

        private void DisplayShape(JsonShapeFile map)
        {
            // Set up the transformation for WPF shapes.            
            if (this.shapeTransform == null)
                this.shapeTransform = this.CreateShapeTransform(manager.Canvas, map);

            // Add the zoom and pan transforms to the view transform.
            this.viewTransform.Children.Add(this.zoomTransform);
            this.viewTransform.Children.Add(this.panTransform);

            foreach (var jshape in map.Shapes)
            {
                var shape = CreateWPFShape("Shape_" + jshape.RecordNo.ToString("n0"), jshape);
                manager.Canvas.Children.Add(shape);
            }
        }

        private Shape CreateWPFShape(string shapeName, JsonShape jshape)
        {
            Shape ret = null;
            if (null == jshape) return ret;

            //Geometry geometry = CreatePathGeometry(jshape);
            Geometry geometry = CreateStreamGeometry(jshape);

            // Transform the geometry based on current zoom and pan settings.
            geometry.Transform = this.viewTransform;

            // Create a new WPF Path.
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();


            // Assign the geometry to the path and set its name.
            path.Data = geometry;
            path.Name = shapeName;

            // Set path properties.
            path.StrokeThickness = 0.5;

            path.Stroke = Brushes.Gray;
            path.Fill = new SolidColorBrush(GetRandomColor());

            /*
            if (record.ShapeType == (int)ShapeType.Polygon)
            {
                path.Stroke = this.strokeBrush;
                path.Fill = this.GetRandomShapeBrush();
            }
            else
            {
                path.Stroke = Brushes.DimGray;
            }
            */

            ret = path;

            // Return the created WPF shape.
            return ret;
        }

        private Geometry CreatePathGeometry(JsonShape jshape)
        {
            // Create a new geometry.
            PathGeometry geometry = new PathGeometry();

            // Add figures to the geometry.

            foreach (var jpart in jshape.Parts)
            {
                // Create a new path figure.
                PathFigure figure = new PathFigure();

                int maxPts = jpart.Count;
                for (int i = 0; i < maxPts; ++i)
                {
                    System.Windows.Point pt = new System.Windows.Point(
                        jpart.Points[i, 0],
                        jpart.Points[i, 1]);

                    // Transform from lon/lat to canvas coordinates.
                    pt = this.shapeTransform.Transform(pt);

                    if (i == 0)
                        figure.StartPoint = pt;
                    else
                        figure.Segments.Add(new LineSegment(pt, true));
                }

                // Add the new figure to the geometry.
                geometry.Figures.Add(figure);
            }

            // Return the created path geometry.
            return geometry;
        }

        private Geometry CreateStreamGeometry(JsonShape jshape)
        {
            // Create a new stream geometry.
            StreamGeometry geometry = new StreamGeometry();

            // Obtain the stream geometry context for drawing each part.
            using (StreamGeometryContext ctx = geometry.Open())
            {
                foreach (var jpart in jshape.Parts)
                {
                    // Draw figures.

                    // Decide if the line segments are stroked or not. For the
                    // PolyLine type it must be stroked.
                    bool isStroked = true;

                    int maxPts = jpart.Count;
                    for (int i = 0; i < maxPts; ++i)
                    {
                        System.Windows.Point pt = new System.Windows.Point(
                            jpart.Points[i, 0],
                            jpart.Points[i, 1]);
                        // Transform from lon/lat to canvas coordinates.
                        pt = this.shapeTransform.Transform(pt);


                        if (i == 0)
                            ctx.BeginFigure(pt, true, false);
                        else
                            ctx.LineTo(pt, isStroked, true);
                    }
                }
            }

            // Return the created stream geometry.
            return geometry;
        }

        private void UpdateResourceUsage()
        {
            Dispatcher.Invoke(() =>
            {
                var usage = NResourceMonitor.Instance.Current;
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
