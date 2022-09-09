#region Using

using System;
using System.Diagnostics;
using System.Threading;

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

        private static NResourceMonitor _instance = null;

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
        private bool _isExit = false;
        private bool _running = false;

        private int _RefreshInSeconds = 1;
        private DateTime _lastUpdate = DateTime.MinValue;

        private NResourceUsage _usage = new NResourceUsage();

        private static PerformanceCounter _cpu = null;
        private static PerformanceCounter _ram = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private NResourceMonitor() : base() 
        {
            _isExit = false;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NResourceMonitor()
        {
            if (null != _cpu)
            {
                try { _cpu.Dispose(); }
                catch { }
            }
            _cpu = null;

            if (null != _ram)
            {
                try { _ram.Dispose(); }
                catch { }
            }
            _ram = null;

            AppDomain.CurrentDomain.ProcessExit -= CurrentDomain_ProcessExit;
            Shutdown();
        }

        #endregion

        #region Private Methods

        #region App Domain Handlers

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _isExit = true; // application is exit.
        }

        #endregion

        #region Process/PerformanceCounters Methods

        private string GetCurrentProcessName()
        {
            string name = string.Empty;

            var process = Process.GetCurrentProcess();
            process = Process.GetCurrentProcess();

            // Preparing variable for application instance name
            foreach (var instance in new PerformanceCounterCategory("Process").GetInstanceNames())
            {
                if (instance.StartsWith(process.ProcessName))
                {
                    using (var processId = new PerformanceCounter("Process", "ID Process", instance, true))
                    {
                        if (process.Id == (int)processId.RawValue)
                        {
                            name = instance;
                            break;
                        }
                    }
                }
            }

            return name;
        }

        private void CreatePerformanceCounters()
        {
            if (null == _cpu || null == _ram)
            {
                string processName = GetCurrentProcessName();
                if (null == _cpu && !string.IsNullOrWhiteSpace(processName.Trim()))
                {
                    _cpu = new PerformanceCounter("Process", "% Processor Time", processName, true);
                }
                if (null == _ram && !string.IsNullOrWhiteSpace(processName.Trim()))
                {
                    _ram = new PerformanceCounter("Process", "Private Bytes", processName, true);
                }
            }
            if (null == _cpu || null == _ram)
                return;

            // Getting first initial values
            try
            {
                // If system has multiple cores, that should be taken into account
                _usage.CPU = Math.Round(_cpu.NextValue() / Environment.ProcessorCount, 2);
                // Returns number of MB consumed by application
                _usage.RAM = Math.Round(_ram.NextValue() / 1024 / 1024, 2);
            }
            catch { }
        }

        private void UpdateResourceInfo()
        {
            if (null == _cpu || null == _ram)
            {
                CreatePerformanceCounters();
            }
            // recheck
            if (null == _cpu || null == _ram)
                return;
            // Creating delay to get correct values of CPU usage during next query
            Thread.Sleep(500);

            // check instance.
            if (null == _usage) _usage = new NResourceUsage();

            lock (_usage)
            {
                try
                {
                    // If system has multiple cores, that should be taken into account
                    _usage.CPU = Math.Round(_cpu.NextValue() / Environment.ProcessorCount, 2);
                    // Returns number of MB consumed by application
                    _usage.RAM = Math.Round(_ram.NextValue() / 1024 / 1024, 2);
                }
                catch { }
            }
        }

        #endregion

        #region Thread Method

        private void Processing()
        {
            bool onScanning = false;

            while (_running && null != _th && !_isExit)
            {
                var ts = DateTime.Now - _lastUpdate;

                if (ts.TotalSeconds >= _RefreshInSeconds && !onScanning)
                {
                    onScanning = true;

                    UpdateResourceInfo();

                    _lastUpdate = DateTime.Now; // update last check time.
                    Thread.Sleep(50);

                    onScanning = false;
                }
            }
        }

        #endregion

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
        /// <summary>
        /// Gets or sets refresh per seconds.
        /// </summary>
        public int RefreshInSeconds
        {
            get { return _RefreshInSeconds; }
            set
            {
                if (_RefreshInSeconds != value)
                {
                    lock (this)
                    {
                        _RefreshInSeconds = value;
                        if (_RefreshInSeconds < 1) _RefreshInSeconds = 1;
                    }
                }
            }
        }

        #endregion
    }

    #endregion
}
