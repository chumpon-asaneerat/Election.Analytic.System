#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Reflection;
using NLib.Services;
using PPRP.Imports.Excel;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ImportPollingStationWindow.xaml
    /// </summary>
    public partial class ImportPollingStationWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportPollingStationWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private int thaiYear = 2562;

        private NExcelImport import = new NExcelImport();

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            import.OnSampleDataChanged += Import_OnSampleDataChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            import.OnSampleDataChanged -= Import_OnSampleDataChanged;
        }

        #endregion

        #region NExcelImport Handlers

        private void Import_OnSampleDataChanged(object sender, EventArgs e)
        {
            /*
            if (null != wsMap && null != wsMap.ImportModel)
            {
                var model = wsMap.ImportModel;
                lvMapPreview.Setup(import);

                var items = Target.LoadWorksheetTable(import, model.Worksheet.SheetName, model.Maps);
                if (null != items)
                {

                }
                lvMapPreview.UpdateItems(model.Maps, items);
            }
            */
        }

        #endregion

        #region Button Handlers

        #region Cancel/Finish

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cmdChooseExcel_Click(object sender, RoutedEventArgs e)
        {
            ChooseExcelFile();
        }

        #endregion

        #endregion

        #region Private Methods

        private void ChooseExcelFile()
        {
            if (import.ShowDialog(PPRPApp.Windows.MainWindow))
            {
                //lstSheets.ItemsSource = import.Worksheets;
                var mapProperties = new string[][]
                {
                    new string[] { "ProvinceName", "ข้อมูลจังหวัด" },
                    new string[] { "UnitNo", "ข้อมูลเขต" }
                };
                wsMap.Setup(import, mapProperties);
            }
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            // Setup excel importer
            NExcelImport.RegisterLicense();

            // setup data context for excel file name.
            this.txtFileName.DataContext = import;
        }

        #endregion
    }
}
