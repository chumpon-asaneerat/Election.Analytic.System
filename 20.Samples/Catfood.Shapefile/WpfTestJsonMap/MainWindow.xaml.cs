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

#endregion

using Microsoft.Win32; // For File Dialog.
using Newtonsoft.Json; // For Json
using NLib;
using PPRP;

namespace WpfTestJsonMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            if (!fd.ShowDialog(this).Value)
            {
                return;
            }
            string fileName = fd.FileName;
            txtFileName.Text = fileName;
            
            LoadJsonFile(fileName); // Load json file.
        }

        #endregion

        private JsonShapeFile json = null;

        private void LoadJsonFile(string fileName)
        {
            json = NJson.LoadFromFile<JsonShapeFile>(fileName);
            if (null != json)
            {
                DisplayShapes(json);
            }
        }

        #region Canvas methods

        // Transformation from lon/lat to canvas coordinates.
        private TransformGroup shapeTransform;

        // Combined view transformation (zoom and pan).
        private TransformGroup viewTransform = new TransformGroup();
        private ScaleTransform zoomTransform = new ScaleTransform();
        private TranslateTransform panTransform = new TranslateTransform();

        // For coloring of WPF shapes.
        private Brush[] shapeBrushes;
        private Random rand = new Random(379013);
        private Brush strokeBrush = new SolidColorBrush(Color.FromArgb(150, 150, 150, 150));

        private void DisplayShapes(JsonShapeFile jshapeFile)
        {
            // Create shape brushes.
            if (this.shapeBrushes == null)
                this.CreateShapeBrushes(0.40, 45);

            // Set up the transformation for WPF shapes.            
            if (this.shapeTransform == null)
                this.shapeTransform = this.CreateShapeTransform(map, jshapeFile);

            // Add the zoom and pan transforms to the view transform.
            this.viewTransform.Children.Add(this.zoomTransform);
            this.viewTransform.Children.Add(this.panTransform);

            //Zoom(map, 8); // zoom 800%

            foreach (var jshape in jshapeFile.Shapes)
            {
                string shapeName = "shape" + jshape.RecordNo.ToString();
                var shape = CreateWPFShape(shapeName, jshape);
                map.Children.Add(shape);
            }
        }

        #region Brushes for gradient coloring

        /// <summary>
        /// Create a set of linear gradient brushes which we can use
        /// as a random pool for assignment to WPF shapes. A higher
        /// gradient factor results in a stronger gradient effect.
        /// </summary>
        /// <param name="gradientFactor">Gradient factor from 0 to 1.</param>
        /// <param name="gradientAngle">Direction of gradient in degrees.</param>
        private void CreateShapeBrushes(double gradientFactor, double gradientAngle)
        {
            // Pick a set of base colors for the brushes.
            Color[] colors = new Color[] {
                Colors.Crimson, Colors.ForestGreen, Colors.RoyalBlue,
                Colors.Navy, Colors.DarkSeaGreen, Colors.LightSlateGray,
                Colors.DarkKhaki, Colors.Olive, Colors.Indigo, Colors.Violet };

            // Create one brush per color.
            this.shapeBrushes = new Brush[colors.Length];
            for (int i = 0; i < this.shapeBrushes.Length; i++)
            {
                this.shapeBrushes[i] = new LinearGradientBrush(
                    GetAdjustedColor(colors[i], gradientFactor), colors[i], gradientAngle);
            }
        }

        /// <summary>
        /// Given an input color, return an adjusted color using a
        /// factor value which ranges from 0 to 1. The larger the factor,
        /// the lighter the adjusted color. A factor of 0 means no adjustment
        /// to the input color.
        /// </summary>
        /// <remarks>
        /// Note that the alpha component of the input color is not adjusted.
        /// </remarks>
        /// <param name="inColor">Input color.</param>
        /// <param name="factor">Color adjustment factor, from 0 to 1.</param>
        /// <returns>An adjusted color value.</returns>
        private static Color GetAdjustedColor(Color inColor, double factor)
        {
            int red = inColor.R + (int)((255 - inColor.R) * factor);
            red = Math.Max(0, red);
            red = Math.Min(255, red);

            int green = inColor.G + (int)((255 - inColor.G) * factor);
            green = Math.Max(0, green);
            green = Math.Min(255, green);

            int blue = inColor.B + (int)((255 - inColor.B) * factor);
            blue = Math.Max(0, blue);
            blue = Math.Min(255, blue);

            return Color.FromArgb(inColor.A, (byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// Get the next brush that can be used to fill a WPF shape.
        /// </summary>
        /// <returns>A randomly selected brush.</returns>
        private Brush GetRandomShapeBrush()
        {
            int index = this.rand.Next() % this.shapeBrushes.Length;
            return this.shapeBrushes[index];
        }

        #endregion Brushes for gradient coloring

        #region Transformations

        /// <summary>
        /// Perform a zoom operation about the current center
        /// of the canvas.
        /// </summary>
        /// <param name="zoomFactor">Zoom multiplication factor (1, 2, 4, etc).</param>
        public void Zoom(Canvas canvas, double zoomFactor)
        {
            // Compute the coordinates of the center of the canvas
            // in terms of pre-view transformation values. We do this
            // by applying the inverse of the view transform.
            Point canvasCenter = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
            canvasCenter = this.viewTransform.Inverse.Transform(canvasCenter);

            // Temporarily reset the panning transformation.
            this.panTransform.X = 0;
            this.panTransform.Y = 0;

            // Set the new zoom transformation scale factors.
            this.zoomTransform.ScaleX = zoomFactor;
            this.zoomTransform.ScaleY = zoomFactor;

            // Apply the updated view transform to the canvas center.
            // This gives us the updated location of the center point
            // on the canvas. By differencing this with the desired
            // center of the canvas, we can determine the ideal panning
            // transformation parameters.
            Point canvasLocation = this.viewTransform.Transform(canvasCenter);
            this.panTransform.X = (canvas.ActualWidth / 2) - canvasLocation.X;
            this.panTransform.Y = (canvas.ActualHeight / 2) - canvasLocation.Y;
        }

        /// <summary>
        /// Perform a panning operation given X and Y factor values
        /// which can be thought of as a fraction of the canvas actual
        /// width or height.
        /// </summary>
        /// <param name="factorX">Fraction of canvas actual width to pan horizontally.</param>
        /// <param name="factorY">Fraction of canvas actual height to pan vertically.</param>
        public void Pan(Canvas canvas, double factorX, double factorY)
        {
            this.panTransform.X += factorX * canvas.ActualWidth;
            this.panTransform.Y += factorY * canvas.ActualHeight;
        }

        /// <summary>
        /// Computes a transformation so that the shapefile geometry
        /// will maximize the available space on the canvas and be
        /// perfectly centered as well.
        /// </summary>
        /// <param name="info">Shapefile information.</param>
        /// <returns>A transformation object.</returns>
        private TransformGroup CreateShapeTransform(Canvas canvas, JsonShapeFile jshapeFile)
        {
            // Bounding box for the shapefile.
            double xmin = jshapeFile.Bound.Left;
            double xmax = jshapeFile.Bound.Right;
            double ymin = jshapeFile.Bound.Top;
            double ymax = jshapeFile.Bound.Bottom;

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

        #endregion

        private Shape CreateWPFShape(string shapeName, JsonShape jshape)
        {
            Shape ret = null;
            if (null == jshape) return ret;

            Geometry geometry = CreatePathGeometry(jshape);
            // Transform the geometry based on current zoom and pan settings.
            geometry.Transform = this.viewTransform;

            // Create a new WPF Path.
            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();


            // Assign the geometry to the path and set its name.
            path.Data = geometry;
            path.Name = shapeName;

            // Set path properties.
            path.StrokeThickness = 0.5;

            path.Stroke = this.strokeBrush;
            path.Fill = this.GetRandomShapeBrush();

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

        #endregion
    }

    #region Json Shape classes

    #region ShapeType

    /// <summary>
    /// The ShapeType of a shape in a Shapefile
    /// </summary>
    public enum JsonShapeType
    {
        /// <summary>Null Shape</summary>
        Null = 0,
        /// <summary>Point Shape</summary>
        Point = 1,
        /// <summary>PolyLine Shape</summary>
        PolyLine = 3,
        /// <summary>Polygon Shape</summary>
        Polygon = 5,
        /// <summary>MultiPoint Shape</summary>
        MultiPoint = 8,
        /// <summary>PointZ Shape</summary>
        PointZ = 11,
        /// <summary>PolyLineZ Shape</summary>
        PolyLineZ = 13,
        /// <summary>PolygonZ Shape</summary>
        PolygonZ = 15,
        /// <summary>MultiPointZ Shape</summary>
        MultiPointZ = 18,
        /// <summary>PointM Shape</summary>
        PointM = 21,
        /// <summary>PolyLineM Shape</summary>
        PolyLineM = 23,
        /// <summary>PolygonM Shape</summary>
        PolygonM = 25,
        /// <summary>MultiPointM Shape</summary>
        MultiPointM = 28,
        /// <summary>MultiPatch Shape</summary>
        MultiPatch = 31
    }

    #endregion

    #region JsonRectangleD

    [JsonObject(MemberSerialization.OptOut)]
    public class JsonRectangleD
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
    }

    #endregion

    #region JsonShapeFile

    [JsonObject(MemberSerialization.OptOut)]
    public class JsonShapeFile
    {
        public JsonShapeFile() : base()
        {
            this.Bound = new JsonRectangleD();
            this.Shapes = new List<JsonShape>();
        }

        public JsonShapeType ShapeType { get; set; }
        public int Count { get; set; }
        public JsonRectangleD Bound { get; set; }

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<JsonShape> Shapes { get; set; }
    }

    #endregion

    #region JsonShape

    [JsonObject(MemberSerialization.OptOut)]
    public class JsonShape
    {
        public JsonShape() : base()
        {
            this.Parts = new List<JsonShapePart>();
        }

        public int RecordNo { get; set; }
        public JsonShapeType ShapeType { get; set; }
        public string ADM0_EN { get; set; }
        public string ADM0_PCODE { get; set; }
        public string ADM1_EN { get; set; }
        public string ADM1_PCODE { get; set; }
        public string ADM2_EN { get; set; }
        public string ADM2_PCODE { get; set; }
        public string ADM3_EN { get; set; }
        public string ADM3_PCODE { get; set; }
        public double SHAPE_LENGTH { get; set; }
        public double SHAPE_AREA { get; set; }

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public List<JsonShapePart> Parts { get; set; }
    }

    #endregion

    #region JsonShapePart

    [JsonObject(MemberSerialization.OptOut)]
    public class JsonShapePart
    {
        public JsonShapePart() : base()
        {
            Points = null;
        }
        public JsonShapeType Type { get; set; }
        public int Count { get; set; }

        [JsonProperty(ObjectCreationHandling = ObjectCreationHandling.Replace)]
        public double[,] Points { get; set; }
    }

    #endregion

    #endregion
}
