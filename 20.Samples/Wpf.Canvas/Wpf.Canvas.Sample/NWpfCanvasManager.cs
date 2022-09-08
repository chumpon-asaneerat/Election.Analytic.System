#region Using

using System;
using System.Collections.Generic;

using System.Diagnostics;

using System.Linq;
using System.Text;
using System.Threading;
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

    public class ResourceUsage
    {
        #region Public Properties

        public double CPU { get; set; }
        public double RAM { get; set; }

        #endregion

        #region Static Variables

        private static Process _process;
        private static string _processName;
        private static PerformanceCounter _cpu;
        private static PerformanceCounter _ram;

        #endregion

        #region Static Methods

        public static ResourceUsage GetUsage()
        {
            // Getting information about current process
            if (null == _process || string.IsNullOrWhiteSpace(_processName.Trim()))
            {
                _process = Process.GetCurrentProcess();

                // Preparing variable for application instance name
                _processName = string.Empty;

                foreach (var instance in new PerformanceCounterCategory("Process").GetInstanceNames())
                {
                    if (instance.StartsWith(_process.ProcessName))
                    {
                        using (var processId = new PerformanceCounter("Process", "ID Process", instance, true))
                        {
                            if (_process.Id == (int)processId.RawValue)
                            {
                                _processName = instance;
                                break;
                            }
                        }
                    }
                }
            }

            if (null == _cpu && !string.IsNullOrWhiteSpace(_processName.Trim()))
            {
                _cpu = new PerformanceCounter("Process", "% Processor Time", _processName, true);
            }
            if (null == _ram && !string.IsNullOrWhiteSpace(_processName.Trim()))
            {
                _ram = new PerformanceCounter("Process", "Private Bytes", _processName, true);
            }

            if (null == _cpu || null == _ram)
                return null;

            // Getting first initial values
            _cpu.NextValue();
            _ram.NextValue();

            // Creating delay to get correct values of CPU usage during next query
            Thread.Sleep(500);

            var result = new ResourceUsage();

            // If system has multiple cores, that should be taken into account
            result.CPU = Math.Round(_cpu.NextValue() / Environment.ProcessorCount, 2);
            // Returns number of MB consumed by application
            result.RAM = Math.Round(_ram.NextValue() / 1024 / 1024, 2);

            return result;
        }

        #endregion
    }
}
