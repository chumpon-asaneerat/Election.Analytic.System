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

namespace NLib.Services
{
    #region NResourceUsage

    /// <summary>
    /// The NResourceUsage class.
    /// </summary>
    public class NResourceUsage
    {
        #region Public Properties

        public double CPU { get; internal set; }
        public double RAM { get; internal set; }

        #endregion
    }

    #endregion

    #region NResourceMonitor

    /// <summary>
    /// The NResourceMonitor class.
    /// </summary>
    public class NResourceMonitor
    {
        #region Singelton Access

        public static NResourceMonitor _instance = null;

        /// <summary>
        /// Singelton Access Instance.
        /// </summary>
        public static NResourceMonitor Instance
        {
            get 
            { 
                if (null == _instance)
                {
                    lock (typeof(NResourceMonitor))
                    {
                        _instance = new NResourceMonitor();
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Internal Variables

        private Thread _th = null;
        private bool _running = false;
        private DateTime _lastUpdate = DateTime.MinValue;
        private NResourceUsage _usage = new NResourceUsage();

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private NResourceMonitor() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NResourceMonitor()
        {
            Shutdown();
        }

        #endregion

        #region Private Methods

        private void Processing()
        {
            while (_running && null != _th)
            {
                var ts = DateTime.Now - _lastUpdate;
                if (ts.TotalSeconds >= 1)
                {
                    UpdateResourceInfo();
                }
            }
        }

        private void UpdateResourceInfo()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start Service.
        /// </summary>
        public void Start()
        {
            if (null == _th)
            {
                _th = new Thread(this.Processing);
                _th.Priority = ThreadPriority.BelowNormal;
                _th.IsBackground = true;
                // set flag
                _running = true;
                // start thread
                _th.Start();
            }
        }
        /// <summary>
        /// Shutdown Service.
        /// </summary>
        public void Shutdown()
        {
            _running = false;
            if (null != _th)
            {
                try { _th.Abort(); }
                catch //(ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
            }
            _th = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets current resource usage.
        /// </summary>
        public NResourceUsage Current
        {
            get { return _usage;  }
        }
        /// <summary>
        /// Checks is running.
        /// </summary>
        public bool IsRunning
        {
            get { return (_running && null != _th); }
        }

        #endregion
    }

    #endregion
}
