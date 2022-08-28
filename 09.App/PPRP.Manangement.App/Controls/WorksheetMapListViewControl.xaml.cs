#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;
using PPRP.Imports.Excel;

#endregion

namespace PPRP.Controls
{
    /// <summary>
    /// Interaction logic for WorksheetMapListViewControl.xaml
    /// </summary>
    public partial class WorksheetMapListViewControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WorksheetMapListViewControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private NExcelImport _import = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void Setup(NExcelImport import, NExcelSheetImportModel model)
        {
            _import = import;
        }

        public void LoadData(NExcelSheetImportModel model)
        {
            if (null != _import)
            {

            }
        }

        #endregion
    }
}
