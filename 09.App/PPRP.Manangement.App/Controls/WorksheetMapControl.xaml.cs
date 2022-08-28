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
        private NExcelWorksheet _worksheet = null;
        private string[][] _properties = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region ListBox Handlers

        private void cbSheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = cbSheets.SelectedItem as NExcelWorksheet;
            LoadSheetColumns(item);
        }

        #endregion

        #region Button Handlers

        private void cmdResetMapProperty_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ctx = (null != button) ? button.DataContext : null;
            var map = (null != ctx) ? ctx as NExcelMapProperty : null;
            if (null != map)
            {
                map.SelectedColumn = null; // reset
            }
        }

        private void cmdLoadExcelData_Click(object sender, RoutedEventArgs e)
        {
            if (null == _import)
                return;
        }

        #endregion

        #region Private Methods

        private void LoadSheetColumns(NExcelWorksheet worksheet)
        {
            lvMaps.ItemsSource = null;
            _worksheet = worksheet;

            ResetMaps();
        }

        private void ResetMaps()
        {
            if (null == _worksheet) return;

            ImportModel = new NExcelSheetImportModel(_worksheet);
            ImportModel.Maps.Clear();
            if (null != _properties && _properties.Length > 0)
            {
                foreach (var prop in _properties)
                {
                    var pName = prop[0];
                    var pText = prop[1];
                    ImportModel.Maps.Add(new NExcelMapProperty(ImportModel) 
                    { 
                        PropertyName = pName, 
                        DisplayText = pText 
                    }); ;
                }
            }

            // set map items source and data context for combobox columns.
            lvMaps.ItemsSource = ImportModel.Maps;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="import">The NExcelImport instance.</param>
        /// <param name="properties">The ProperyName, DisplayText pair array.</param>
        public void Setup(NExcelImport import, string[][] properties)
        {
            _import = import;
            _properties = properties;
            ImportModel = null; // reset

            cbSheets.ItemsSource = null;
            if (null != _import)
            {
                cbSheets.ItemsSource = _import.Worksheets;
                if (null != _import.Worksheets && _import.Worksheets.Count > 0)
                {
                    // auto select first index.
                    cbSheets.SelectedIndex = 0;
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets NExcelSheetImportModel.
        /// </summary>
        public NExcelSheetImportModel ImportModel { get; set; }

        #endregion
    }
}
