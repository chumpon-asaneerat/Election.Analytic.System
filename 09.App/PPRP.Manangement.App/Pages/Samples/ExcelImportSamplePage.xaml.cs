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

        private NExcelImport import = new NExcelImport();

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
            NExcelImport.RegisterLicense();

            import.Steps.Clear(); // clear all steps.
            import.Steps.Add("เลือกไฟล์ที่ต้องการนำเข้า");
            import.Steps.Add("ตรวจสอบความถูกต้องก่อนนำเข้าข้อมูล");
            import.Steps.Add("นำเข้าข้อมูล");
            import.Steps.Add("เสร็จสิ้น");
            import.ApplySteps();

            this.DataContext = import;
            /*
            wzBar.DataContext = import;
            cmdNext.DataContext = wzBar.DataContext; // set data context same as Wizard ProgressBar
            cmdPrev.DataContext = wzBar.DataContext; // set data context same as Wizard ProgressBar
            */
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
            import.PreviousStep();
        }

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {
            import.NextStep();
        }

        private void cmdChooseExcel_Click(object sender, RoutedEventArgs e)
        {
            ChooseExcelFile();
        }

        #endregion

        #region ListBox Handlers
        private void lstSheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lstSheets.SelectedItem as NExcelWorksheet;
            LoadSheetColumns(item);
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
            if (import.ShowDialog(PPRPApp.Windows.MainWindow))
            {
                lstSheets.ItemsSource = import.Worksheets;
            }
        }

        private void LoadSheetColumns(NExcelWorksheet worksheet)
        {
            lstColumns.ItemsSource = null;
            if (null == worksheet) return;
            // load all columns
            lstColumns.ItemsSource = import.GetColumns(worksheet.SheetName);
        }

        #endregion
    }
}
