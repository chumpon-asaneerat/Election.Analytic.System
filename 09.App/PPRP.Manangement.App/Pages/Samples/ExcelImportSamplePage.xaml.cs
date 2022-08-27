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
//using PPRP.Imports.ShapeFiles;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for ExcelImportSamplePage.xaml
    /// </summary>
    public partial class ExcelImportSamplePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelImportSamplePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private NExcelImportWizard wizard = new NExcelImportWizard();

        private NExcelImport import = new NExcelImport();

        private List<NExcelMapProperty> maps = new List<NExcelMapProperty>();

        class Target
        {
            /// <summary>จังหวัด</summary>
            public string ProvinceName { get; set; }

            /// <summary>หน่วยเลือกตั้งที่</summary>
            public string UnitNo { get; set; }
        }

        /// <summary>
        /// For Binding combobox within listview columns
        /// </summary>
        public List<NExcelColumn> ExcelColumns { get; set; }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            // Setup wizard steps.
            wizard.Steps.Clear(); // clear all steps.
            wizard.Steps.Add("เลือกไฟล์ที่ต้องการนำเข้า");
            wizard.Steps.Add("ตรวจสอบความถูกต้องก่อนนำเข้าข้อมูล");
            wizard.Steps.Add("นำเข้าข้อมูล");
            wizard.Steps.Add("เสร็จสิ้น");
            wizard.FirstStep(); // set to first step.
            // setup wizard DataContext
            wzBar.DataContext = wizard;
            cmdPrev.DataContext = wzBar.DataContext;
            cmdNext.DataContext = wzBar.DataContext;
            cmdFinish.DataContext = wzBar.DataContext;

            // Setup excel importer
            NExcelImport.RegisterLicense();
            this.DataContext = import;
        }

        #endregion

        #region Button Handlers

        #region Cancel/Prev/Next

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenuPage();
        }

        private void cmdPrev_Click(object sender, RoutedEventArgs e)
        {
            wizard.PreviousStep();
        }

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {
            wizard.NextStep();
        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdChooseExcel_Click(object sender, RoutedEventArgs e)
        {
            ChooseExcelFile();
        }

        #endregion

        #region ListBox Handlers

        private void lstSheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            var item = lstSheets.SelectedItem as NExcelWorksheet;
            LoadSheetColumns(item);
            */
        }

        #endregion

        #region ListView's Combobox Handlers

        private void cbColumns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion

        #endregion

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void ChooseExcelFile()
        {
            /*
            if (import.ShowDialog(PPRPApp.Windows.MainWindow))
            {
                lstSheets.ItemsSource = import.Worksheets;
            }
            */
        }

        private void LoadSheetColumns(NExcelWorksheet worksheet)
        {
            /*
            lvColumns.ItemsSource = null;
            if (null == worksheet) return;
            // load all columns
            lvColumns.ItemsSource = worksheet.Columns;
            // Set current ViewModel 
            this.ExcelColumns = worksheet.Columns;

            // Create map properties

            if (null == maps) maps = new List<NExcelMapProperty>();
            maps.Clear();
            maps.Add(new NExcelMapProperty() { PropertyName = "ProvinceName", ColumnLetter = "", Columns = this.ExcelColumns });
            maps.Add(new NExcelMapProperty() { PropertyName = "UnitNo", ColumnLetter = "", Columns = this.ExcelColumns });
            lvMap.ItemsSource = maps;
            */
        }

        #endregion
    }
}
