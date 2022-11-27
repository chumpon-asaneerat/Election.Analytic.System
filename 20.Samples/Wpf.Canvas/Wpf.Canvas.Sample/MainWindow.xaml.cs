#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
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
        //private NWpfCanvasManager manager = null;
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
            //manager = null;

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
            //manager = new NWpfCanvasManager(this.canvas);

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
        }

        private void cmdThailand_Click(object sender, RoutedEventArgs e)
        {
            this.canvas.Children.Clear(); // reset canvas
            // Reset transformations.
            this.panTransform.X = 0;
            this.panTransform.Y = 0;
            this.zoomTransform.ScaleX = 1;
            this.zoomTransform.ScaleY = 1;
            this.shapeTransform = null;

            this.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DoOperation(this.DisplayThailandMap));
        }

        private void cmdYasothon_Click(object sender, RoutedEventArgs e)
        {
            this.canvas.Children.Clear(); // reset canvas
            // Reset transformations.
            this.panTransform.X = 0;
            this.panTransform.Y = 0;
            this.zoomTransform.ScaleX = 1;
            this.zoomTransform.ScaleY = 1;
            this.shapeTransform = null;

            this.canvas.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DoOperation(this.DisplayYasothonMaps));
        }

        private void cmdThailand2_Click(object sender, RoutedEventArgs e)
        {
            this.canvas2.Children.Clear(); // reset canvas
            // Reset transformations.
            this.panTransform.X = 0;
            this.panTransform.Y = 0;
            this.zoomTransform.ScaleX = 1;
            this.zoomTransform.ScaleY = 1;
            this.shapeTransform = null;

            this.canvas2.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DoOperation(this.DisplayThailandMap2));
        }

        private void cmdYasothon2_Click(object sender, RoutedEventArgs e)
        {
            this.canvas2.Children.Clear(); // reset canvas
            // Reset transformations.
            this.panTransform.X = 0;
            this.panTransform.Y = 0;
            this.zoomTransform.ScaleX = 1;
            this.zoomTransform.ScaleY = 1;
            this.shapeTransform = null;

            this.canvas2.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DoOperation(this.DisplayYasothonMaps2));
        }

        private delegate void DoOperation();

        private JsonShapeFile thailandMap;
        private List<JsonShapeFile> yasothonMaps = new List<JsonShapeFile>();

        private void DisplayThailandMap()
        {
            if (null == thailandMap)
            {
                string fileName = System.IO.Path.Combine(NJson.LocalDataFolder, @"Maps\Thailand.json");
                thailandMap = JsonMapFileManager.Load(fileName);
            }
            if (null == thailandMap)
                return;

            StopWatch.Start();
            DisplayShape(thailandMap);
            UpdateExecuteTime(StopWatch.Stop());
        }

        private void DisplayYasothonMaps()
        {
            if (null == yasothonMaps) yasothonMaps = new List<JsonShapeFile>();
            if (yasothonMaps.Count <= 0)
            {
                string[] files = new string[]
                {
                    @"Maps\Yasothon\Thailand.Yasothon.json",
                    @"Maps\Yasothon\Kham Khuean Kaeo\Thailand.Yasothon.Kham Khuean Kaeo.json",
                    @"Maps\Yasothon\Kho Wang\Thailand.Yasothon.Kho Wang.json",
                    @"Maps\Yasothon\Kut Chum\Thailand.Yasothon.Kut Chum.json",
                    @"Maps\Yasothon\Loeng Nok Tha\Thailand.Yasothon.Loeng Nok Tha.json",
                    @"Maps\Yasothon\Maha Chana Chai\Thailand.Yasothon.Maha Chana Chai.json",
                    @"Maps\Yasothon\Mueang Yasothon\Thailand.Yasothon.Mueang Yasothon.json",
                    @"Maps\Yasothon\Pa Tio\Thailand.Yasothon.Pa Tio.json",
                    @"Maps\Yasothon\Sai Mun\Thailand.Yasothon.Sai Mun.json",
                    @"Maps\Yasothon\Thai Charoen\Thailand.Yasothon.Thai Charoen.json"
                };
                foreach (var file in files)
                {
                    string fileName = System.IO.Path.Combine(NJson.LocalDataFolder, file);
                    var map = JsonMapFileManager.Load(fileName);
                    if (null != map)
                    {
                        yasothonMaps.Add(map);
                    }
                }
            }
            if (yasothonMaps.Count <= 0)
                return;
            StopWatch.Start();
            DisplayShapes(yasothonMaps);
            UpdateExecuteTime(StopWatch.Stop());
        }

        private JsonShapeFile thailandMap2;
        private List<JsonShapeFile> yasothonMaps2 = new List<JsonShapeFile>();

        private void DisplayThailandMap2()
        {
            if (null == thailandMap2)
            {
                string fileName = System.IO.Path.Combine(NJson.LocalDataFolder, @"Maps\Thailand.json");
                thailandMap2 = JsonMapFileManager.Load(fileName);
            }
            if (null == thailandMap2)
                return;

            StopWatch.Start();
            DisplayShape2(thailandMap2);
            UpdateExecuteTime(StopWatch.Stop());
        }

        private void DisplayYasothonMaps2()
        {
            if (null == yasothonMaps2) yasothonMaps2 = new List<JsonShapeFile>();
            if (yasothonMaps2.Count <= 0)
            {
                string[] files = new string[]
                {
                    @"Maps\Yasothon\Thailand.Yasothon.json",
                    @"Maps\Yasothon\Kham Khuean Kaeo\Thailand.Yasothon.Kham Khuean Kaeo.json",
                    @"Maps\Yasothon\Kho Wang\Thailand.Yasothon.Kho Wang.json",
                    @"Maps\Yasothon\Kut Chum\Thailand.Yasothon.Kut Chum.json",
                    @"Maps\Yasothon\Loeng Nok Tha\Thailand.Yasothon.Loeng Nok Tha.json",
                    @"Maps\Yasothon\Maha Chana Chai\Thailand.Yasothon.Maha Chana Chai.json",
                    @"Maps\Yasothon\Mueang Yasothon\Thailand.Yasothon.Mueang Yasothon.json",
                    @"Maps\Yasothon\Pa Tio\Thailand.Yasothon.Pa Tio.json",
                    @"Maps\Yasothon\Sai Mun\Thailand.Yasothon.Sai Mun.json",
                    @"Maps\Yasothon\Thai Charoen\Thailand.Yasothon.Thai Charoen.json"
                };
                foreach (var file in files)
                {
                    string fileName = System.IO.Path.Combine(NJson.LocalDataFolder, file);
                    var map = JsonMapFileManager.Load(fileName);
                    if (null != map)
                    {
                        yasothonMaps2.Add(map);
                    }
                }
            }
            if (yasothonMaps2.Count <= 0)
                return;
            StopWatch.Start();
            DisplayShapes2(yasothonMaps2);
            UpdateExecuteTime(StopWatch.Stop());
        }

        // Transformation from lon/lat to canvas coordinates.
        private TransformGroup shapeTransform;

        // Combined view transformation (zoom and pan).
        private TransformGroup viewTransform = new TransformGroup();
        private ScaleTransform zoomTransform = new ScaleTransform();
        private TranslateTransform panTransform = new TranslateTransform();

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

            // Width and height of the bounding box + 50 for margin.
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
                this.shapeTransform = this.CreateShapeTransform(this.canvas, map);

            // Add the zoom and pan transforms to the view transform.
            this.viewTransform.Children.Add(this.zoomTransform);
            this.viewTransform.Children.Add(this.panTransform);

            foreach (var jshape in map.Shapes)
            {
                /*
                string text = string.Empty;
                if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM3_EN))
                    text = jshape.ADM3_EN.Trim();
                if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM2_EN))
                    text = jshape.ADM2_EN.Trim();
                if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM1_EN))
                    text = jshape.ADM1_EN.Trim();

                var shape = CreateWPFShape("Shape_" + jshape.RecordNo.ToString("n0"), jshape);

                var grid = new Grid();
                grid.Children.Add(shape); // add shape.

                var textBlock = new TextBlock();
                textBlock.Text = text;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.Foreground = Brushes.WhiteSmoke;
                textBlock.Background = Brushes.Silver;
                textBlock.Height = 20;
                textBlock.Width = 200;

                grid.Children.Add(textBlock); // add text

                this.canvas.Children.Add(grid); // add to canvas
                */

                var shape = CreateWPFShape("Shape_" + jshape.RecordNo.ToString("n0"), jshape);
                this.canvas.Children.Add(shape); // add to canvas
            }
        }

        private void DisplayShapes(List<JsonShapeFile> maps)
        {
            int i = 0;
            foreach (var map in maps)
            {
                if (i == 0)
                {
                    // Set up the transformation for WPF shapes.            
                    if (this.shapeTransform == null)
                    {
                        this.shapeTransform = this.CreateShapeTransform(this.canvas, map);
                    }

                    // Add the zoom and pan transforms to the view transform.
                    this.viewTransform.Children.Add(this.zoomTransform);
                    this.viewTransform.Children.Add(this.panTransform);
                }

                foreach (var jshape in map.Shapes)
                {
                    /*
                    string text = string.Empty;
                    if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM3_EN))
                        text = jshape.ADM3_EN.Trim();
                    if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM2_EN))
                        text = jshape.ADM2_EN.Trim();
                    if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM1_EN))
                        text = jshape.ADM1_EN.Trim();

                    var shape = CreateWPFShape("Shape_" + jshape.RecordNo.ToString("n0"), jshape);

                    var grid = new Grid();
                    grid.Children.Add(shape); // add shape.

                    var textBlock = new TextBlock();
                    textBlock.Text = text;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Foreground = Brushes.Yellow;

                    grid.Children.Add(textBlock); // add text

                    this.canvas.Children.Add(grid); // add to canvas
                    */

                    var shape = CreateWPFShape("Shape_" + jshape.RecordNo.ToString("n0"), jshape);
                    this.canvas.Children.Add(shape); // add to canvas
                }

                i++;
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
            GeometryGroup combine = new GeometryGroup();

            RectangleD rect = new RectangleD();
            int iCnt = 0;

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

                        // Calc Rectancle
                        rect.Left = Math.Min(rect.Left, pt.X);
                        rect.Right = Math.Max(rect.Right, pt.X);

                        rect.Top = Math.Min(rect.Top, pt.Y);
                        rect.Bottom = Math.Max(rect.Bottom, pt.Y);

                        if (i == 0)
                            ctx.BeginFigure(pt, true, false);
                        else
                            ctx.LineTo(pt, isStroked, true);

                        iCnt++; // count all points
                    }
                }
            }

            Console.WriteLine("Total: {0} pts", iCnt);
            combine.Children.Add(geometry);

            string text = string.Empty;
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM3_EN))
                text = jshape.ADM3_EN.Trim();
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM2_EN))
                text = jshape.ADM2_EN.Trim();
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM1_EN))
                text = jshape.ADM1_EN.Trim();

            // Create the formatted text based on the properties set.
            FormattedText formattedText = new FormattedText(text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight, new Typeface("Tahoma"),
                16,
                Brushes.White,
                120 / 96);

            var txtWd = formattedText.Width;
            var txtHt = formattedText.Height;
            var rectWd = rect.Right - rect.Left;
            var rectHt = rect.Bottom - rect.Top;
            var ptX = rect.Left + ((rectWd - txtWd) / 2);
            var ptY = rect.Top + ((rectHt - txtHt) / 2);

            System.Windows.Point centerPt = new Point(ptX, ptY);

            Geometry textGeometry = formattedText.BuildGeometry(centerPt);
            combine.Children.Add(textGeometry);

            // Return the created stream geometry.
            return combine;
        }

        private void DisplayShape2(JsonShapeFile map)
        {
            // Set up the transformation for WPF shapes.            
            if (this.shapeTransform == null)
                this.shapeTransform = this.CreateShapeTransform(this.canvas2, map);

            // Add the zoom and pan transforms to the view transform.
            this.viewTransform.Children.Add(this.zoomTransform);
            this.viewTransform.Children.Add(this.panTransform);

            foreach (var jshape in map.Shapes)
            {
                /*
                string text = string.Empty;
                if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM3_EN))
                    text = jshape.ADM3_EN.Trim();
                if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM2_EN))
                    text = jshape.ADM2_EN.Trim();
                if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM1_EN))
                    text = jshape.ADM1_EN.Trim();

                var shape = CreateWPFShape("Shape_" + jshape.RecordNo.ToString("n0"), jshape);

                var grid = new Grid();
                grid.Children.Add(shape); // add shape.

                var textBlock = new TextBlock();
                textBlock.Text = text;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.Foreground = Brushes.WhiteSmoke;
                textBlock.Background = Brushes.Silver;
                textBlock.Height = 20;
                textBlock.Width = 200;

                grid.Children.Add(textBlock); // add text

                this.canvas2.Children.Add(grid); // add to canvas
                */

                var shape = CreateWPFShape2("Shape_" + jshape.RecordNo.ToString("n0"), jshape);
                this.canvas2.Children.Add(shape); // add to canvas
            }
        }

        private void DisplayShapes2(List<JsonShapeFile> maps)
        {
            int i = 0;
            foreach (var map in maps)
            {
                if (i == 0)
                {
                    // Set up the transformation for WPF shapes.            
                    if (this.shapeTransform == null)
                    {
                        this.shapeTransform = this.CreateShapeTransform(this.canvas2, map);
                    }

                    // Add the zoom and pan transforms to the view transform.
                    this.viewTransform.Children.Add(this.zoomTransform);
                    this.viewTransform.Children.Add(this.panTransform);
                }

                foreach (var jshape in map.Shapes)
                {
                    /*
                    string text = string.Empty;
                    if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM3_EN))
                        text = jshape.ADM3_EN.Trim();
                    if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM2_EN))
                        text = jshape.ADM2_EN.Trim();
                    if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM1_EN))
                        text = jshape.ADM1_EN.Trim();

                    var shape = CreateWPFShape("Shape_" + jshape.RecordNo.ToString("n0"), jshape);

                    var grid = new Grid();
                    grid.Children.Add(shape); // add shape.

                    var textBlock = new TextBlock();
                    textBlock.Text = text;
                    textBlock.VerticalAlignment = VerticalAlignment.Center;
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.Foreground = Brushes.Yellow;

                    grid.Children.Add(textBlock); // add text

                    this.canvas2.Children.Add(grid); // add to canvas
                    */

                    var shape = CreateWPFShape2("Shape_" + jshape.RecordNo.ToString("n0"), jshape);
                    this.canvas2.Children.Add(shape); // add to canvas
                }

                i++;
            }
        }

        private Shape CreateWPFShape2(string shapeName, JsonShape jshape)
        {
            Shape ret = null;
            if (null == jshape) return ret;

            //Geometry geometry = CreatePathGeometry(jshape);
            Geometry geometry = CreateStreamGeometry2(jshape);

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

        private Geometry CreatePathGeometry2(JsonShape jshape)
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

        private Geometry CreateStreamGeometry2(JsonShape jshape)
        {
            GeometryGroup combine = new GeometryGroup();

            RectangleD rect = new RectangleD();
            int iCnt = 0;

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

                        // Calc Rectancle
                        rect.Left = Math.Min(rect.Left, pt.X);
                        rect.Right = Math.Max(rect.Right, pt.X);

                        rect.Top = Math.Min(rect.Top, pt.Y);
                        rect.Bottom = Math.Max(rect.Bottom, pt.Y);

                        if (i == 0)
                            ctx.BeginFigure(pt, true, false);
                        else
                            ctx.LineTo(pt, isStroked, true);

                        iCnt++; // count all points
                    }
                }
            }

            Console.WriteLine("Total: {0} pts", iCnt);
            combine.Children.Add(geometry);

            string text = string.Empty;
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM3_EN))
                text = jshape.ADM3_EN.Trim();
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM2_EN))
                text = jshape.ADM2_EN.Trim();
            if (string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(jshape.ADM1_EN))
                text = jshape.ADM1_EN.Trim();

            // Create the formatted text based on the properties set.
            FormattedText formattedText = new FormattedText(text,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight, new Typeface("Tahoma"),
                16,
                Brushes.White,
                120 / 96);

            var txtWd = formattedText.Width;
            var txtHt = formattedText.Height;
            var rectWd = rect.Right - rect.Left;
            var rectHt = rect.Bottom - rect.Top;
            var ptX = rect.Left + ((rectWd - txtWd) / 2);
            var ptY = rect.Top + ((rectHt - txtHt) / 2);

            System.Windows.Point centerPt = new Point(ptX, ptY);

            Geometry textGeometry = formattedText.BuildGeometry(centerPt);
            combine.Children.Add(textGeometry);

            // Return the created stream geometry.
            return combine;
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
                /*
                var shape = manager.CreateLineShape(x1, y1, x2, y2, brush, 2);

                shapes.Add(shape);
                */
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
