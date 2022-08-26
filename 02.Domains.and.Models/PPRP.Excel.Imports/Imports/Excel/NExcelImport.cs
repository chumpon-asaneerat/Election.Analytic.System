#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

using OfficeOpenXml;
using EPPlus;
using EPPlus.DataExtractor;

#endregion

namespace PPRP.Imports.Excel
{
    #region NExcelImport

    /// <summary>
    /// The NExcelImport class.
    /// </summary>
    public class NExcelImport : IDisposable
    {
        #region Internal Variables

        private bool disposedValue;

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

        public string FileName { get; set; }

        #endregion
    }

    #endregion
}
