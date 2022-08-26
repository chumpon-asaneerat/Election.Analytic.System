#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Linq;

using System.Windows;
using System.Windows.Forms;

using Microsoft.Win32;

using NLib;
using OfficeOpenXml;
using EPPlus;
using EPPlus.DataExtractor;

#endregion

namespace PPRP.Imports.Excel
{
    #region NExcelMapProperty

    /// <summary>
    /// The NExcelMapProperty class.
    /// </summary>
    public class NExcelMapProperty
    {
        #region Public Properties

        /// <summary>Gets or sets Target property name.</summary>
        public string PropertyName { get; set; }
        /// <summary>Gets or sets Excel column'name like 'A', 'B', 'C', etc.</summary>
        public string ColumnName { get; set; }

        #endregion
    }

    #endregion

    #region NExcelWorksheet

    /// <summary>
    /// The NExcelWorksheet class.
    /// </summary>
    public class NExcelWorksheet
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets excel worksheet name.
        /// </summary>
        public string SheetName { get; set; }

        #endregion
    }

    #endregion

    #region NExcelAnalyzeResult

    #endregion

    #region NExcelImport

    /// <summary>
    /// The NExcelImport class.
    /// </summary>
    public class NExcelImport : NInpc, IDisposable
    {
        #region Static Variables

        /// <summary>The DESKTOP path.</summary>
        protected static string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        #endregion

        #region Internal Variables

        private bool disposedValue; // flag for dispose

        private ObservableCollection<string> _steps = null;

        private int _maxStep = 0;
        private int _progress = 0;

        private string _fileName = string.Empty;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NExcelImport() : base()
        {
            _steps = new ObservableCollection<string>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NExcelImport()
        {
            // Do not change this code.
            // Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        #endregion

        #region Private Methods

        public void ResetStep()
        {
            _progress = 0;
            _maxStep = 0;
            _maxStep = (null == _steps || _steps.Count <= 0) ? 0 : _steps.Count - 1;

            // Raise ProeprtyChanged Events
            this.RaiseProgressEvents();
        }

        public void PreviousStep()
        {
            --_progress;
            if (_progress < 0) _progress = 0;

            // Raise ProeprtyChanged Events
            this.RaiseProgressEvents();
        }

        public void NextStep()
        {
            ++_progress;
            if (_progress > _maxStep) _progress = _maxStep;

            // Raise ProeprtyChanged Events
            this.RaiseProgressEvents();
        }

        private void RaiseProgressEvents()
        {
            // Raise ProeprtyChanged Events
            this.Raise(() => this.Progress);

            this.Raise(() => this.CanGoPrevious);
            this.Raise(() => this.CanGoNext);

            this.Raise(() => this.GoPreviousVisibility);
            this.Raise(() => this.GoNextVisibility);
        }

        private void LoadWorksheets()
        {

        }

        #endregion

        #region protected Methods

        #region Dispose

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">True when in disposing process.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    if (null != _steps)
                    {
                        _steps.Clear();
                    }
                    _steps = null;
                }
                // Free unmanaged resources (unmanaged objects) and override finalizer

                // Set large fields to null
                disposedValue = true;
            }
        }

        #endregion

        #endregion

        #region Public Methods

        #region Dispose

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ShowDialog

        public bool ShowDialog(string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล", 
            string initDir = "")
        {
            bool ret = false;

            var od = new Microsoft.Win32.OpenFileDialog();
            od.Multiselect = false;
            
            od.InitialDirectory = (string.IsNullOrEmpty(initDir) || !Directory.Exists(initDir)) ?
                DESKTOP_PATH : initDir;
            
            od.Title = string.IsNullOrEmpty(title) ? "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล" : title;
            
            od.Filter = "Excel Files(*.xls, *.xlsx)|*.xls,*.xlsx";

            ret = (od.ShowDialog().HasValue && od.ShowDialog().Value) ? true : false;
            if (ret)
            {
                // assigned to FileName
                this.FileName = od.FileName;
            }
            od = null;

            return ret;
        }

        #endregion

        #endregion

        #region Public Properties

        #region For Wizard Process Bar

        public ObservableCollection<string> Steps
        {
            get { return _steps; }
            set { }
        }

        public int Progress
        {
            get { return _progress; }
            set { }
        }

        public bool CanGoPrevious
        {
            get 
            {
                if (_maxStep == 0) 
                    return false;
                bool isFirstStep = _progress <= 0;
                return !isFirstStep;
            }
            set { }
        }

        public Visibility GoPreviousVisibility
        {
            get 
            {
                var ret = (CanGoPrevious) ? Visibility.Visible : Visibility.Hidden;
                return ret;
            }
            set { }
        }

        public bool CanGoNext
        {
            get 
            {
                if (_maxStep == 0) 
                    return false;
                bool isLastStep = _progress >= _maxStep;
                return !isLastStep;
            }
            set { }
        }

        public Visibility GoNextVisibility
        {
            get 
            { 
                var ret = (CanGoNext) ? Visibility.Visible : Visibility.Hidden;
                return ret;
            }
            set { }
        }

        #endregion

        #region For excel information

        /// <summary>
        /// Gets or sets Exel File Name (xls, xlsx).
        /// </summary>
        public string FileName 
        {
            get { return _fileName; } 
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    LoadWorksheets();
                    this.Raise(() => this.FileName); // Raise ProeprtyChanged Event
                }
            }
        }

        #endregion

        #endregion
    }

    #endregion
}
