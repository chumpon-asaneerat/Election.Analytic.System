#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

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

        #region Public Methods

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
