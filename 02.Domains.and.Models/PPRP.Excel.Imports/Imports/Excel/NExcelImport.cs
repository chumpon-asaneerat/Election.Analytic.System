#region Using

using System;
using System.Collections.Generic;
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

        private string _fileName = string.Empty;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NExcelImport() : base()
        {

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
    }

    #endregion
}
