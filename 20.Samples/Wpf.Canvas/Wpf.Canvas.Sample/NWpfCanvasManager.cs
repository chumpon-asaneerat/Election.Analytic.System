#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#endregion

namespace Wpf.Canvas.Sample
{
    public class NWpfCanvasManager
    {
        #region Internal Variables

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private NWpfCanvasManager() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="canvas">The target canvas.</param>
        public NWpfCanvasManager(System.Windows.Controls.Canvas canvas) : this()
        {
            this.Canvas = canvas;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NWpfCanvasManager()
        {
            this.Canvas = null;
        }

        #endregion

        #region Public Methods

        public Shape CreateLineShape(double x1, double y1, double x2, double y2,
            Brush stoke = null, double stokeThickness = 1, 
            HorizontalAlignment horzAlignment = HorizontalAlignment.Left, 
            VerticalAlignment vertAlignment = VerticalAlignment.Center)
        {
            var shape = new Line();
            // Line Stoke
            shape.Stroke = (null == stoke) ? Brushes.Black : stoke;
            shape.StrokeThickness = stokeThickness;
            // Point Start
            shape.X1 = x1;
            shape.Y1 = y1;
            // Point End
            shape.X2 = x2;
            shape.Y2 = y2;

            shape.HorizontalAlignment = horzAlignment;
            shape.VerticalAlignment = vertAlignment;

            return shape;
        }

        public void AddShape(Shape shape)
        {
            if (null == shape) return;
            if (null == this.Canvas) return;

            Canvas.Children.Add(shape);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Target Canvas.
        /// </summary>
        public System.Windows.Controls.Canvas Canvas { get; protected set; }

        #endregion
    }


    public class StopWatch
    {
        private static DateTime _dt = DateTime.Now;

        public static void Start()
        {
            _dt = DateTime.Now;
        }
        
        public static TimeSpan Stop()
        {
            return DateTime.Now - _dt;
        }
    }
}
