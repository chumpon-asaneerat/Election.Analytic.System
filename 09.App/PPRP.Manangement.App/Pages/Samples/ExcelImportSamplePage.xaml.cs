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
        private NExcelSheetImportModel importModel { get; set; }

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

            // setup data context for excel file name.
            this.txtFileName.DataContext = import;

            // setup wizard DataContext
            wzBar.DataContext = wizard;
            cmdPrev.DataContext = wzBar.DataContext;
            cmdNext.DataContext = wzBar.DataContext;
            cmdFinish.DataContext = wzBar.DataContext;

            // Setup excel importer
            NExcelImport.RegisterLicense();
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
                //lstSheets.ItemsSource = import.Worksheets;
                var mapProperties = new string[]
                {
                    "ProvinceName",
                    "UnitNo"
                };
                wsMap.Setup(import, mapProperties);
            }
        }

        #endregion
    }
}
