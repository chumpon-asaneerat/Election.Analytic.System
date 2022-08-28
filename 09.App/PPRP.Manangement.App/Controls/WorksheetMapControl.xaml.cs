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
    /// Interaction logic for WorksheetMapControl.xaml
    /// </summary>
    public partial class WorksheetMapControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WorksheetMapControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private NExcelImport _import = null;
        private string[] _properties = null;

        #endregion

        #region ListBox Handlers
        private void lbSheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lbSheets.SelectedItem as NExcelWorksheet;
            LoadSheetColumns(item);
        }

        #endregion

        #region Private Methods

        private void LoadSheetColumns(NExcelWorksheet worksheet)
        {
            lvMaps.ItemsSource = null;

            if (null == worksheet) return;

            var importModel = new NExcelSheetImportModel(worksheet);
            importModel.Maps.Clear();
            if (null != _properties && _properties.Length > 0)
            {
                foreach (string prop in _properties)
                {
                    importModel.Maps.Add(new NExcelMapProperty(importModel) { PropertyName = prop });
                }
            }

            // set map items source and data context for combobox columns.
            lvMaps.ItemsSource = importModel.Maps;
        }

        #endregion

        #region Public Methods

        public void Setup(NExcelImport import, string[] properties)
        {
            _import = import;
            _properties = properties;

            lbSheets.ItemsSource = null;
            if (null != _import)
            {
                lbSheets.ItemsSource = _import.Worksheets;
            }
        }

        #endregion
    }
}
