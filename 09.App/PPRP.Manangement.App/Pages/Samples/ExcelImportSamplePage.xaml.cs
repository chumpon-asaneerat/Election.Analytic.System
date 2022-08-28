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
//using PPRP.Imports.ShapeFiles;

using OfficeOpenXml;
using EPPlus;
using EPPlus.DataExtractor;

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
            if (null != wsMap && null != wsMap.ImportModel)
            {
                var model = wsMap.ImportModel;
                lvMapPreview.Setup(import);

                var items = Target.LoadWorksheetTable(import, model.Worksheet.SheetName,  model.Maps);
                if (null != items)
                {

                }
                lvMapPreview.UpdateItems(model.Maps, items);
            }
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
                var mapProperties = new string[][]
                {
                    new string[] { "ProvinceName", "ข้อมูลจังหวัด" },
                    new string[] { "UnitNo", "ข้อมูลเขต" }
                };
                wsMap.Setup(import, mapProperties);
            }
        }

        #endregion
    }

    public class Target
    {
        /// <summary>จังหวัด</summary>
        public string ProvinceName { get; set; }

        /// <summary>หน่วยเลือกตั้งที่</summary>
        public string UnitNo { get; set; }


        public static List<Target> LoadWorksheetTable(NExcelImport import,
            string sheetName, List<NExcelMapProperty> mapProperties)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            var results = new List<Target>();
            if (null == import || string.IsNullOrWhiteSpace(sheetName) || 
                null == mapProperties || mapProperties.Count <= 0)
                return results;

            Dictionary<string, int> columns = new Dictionary<string, int>();
            foreach (var prop in mapProperties)
            {
                if (prop.ColumnIndex < 1)
                    continue; // ignore if column < 1

                if (!columns.ContainsKey(prop.PropertyName))
                    columns.Add(prop.PropertyName, prop.ColumnIndex);
                else columns[prop.PropertyName] = prop.ColumnIndex;
            }

            using (var package = new ExcelPackage(import.FileName))
            {
                try
                {
                    var sheet = package.Workbook.Worksheets[sheetName];
                    if (null != sheet)
                    {
                        int colCount = sheet.Dimension.End.Column;  //get Column Count
                        int rowCount = sheet.Dimension.End.Row;     //get row count

                        // start row at position 2.
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var inst = new Target();

                            foreach (var key in columns.Keys)
                            {
                                int colIdx = columns[key];
                                if (colIdx < 1)
                                    continue;
                                object oVal = sheet.Cells[row, columns[key]].Value;
                                DynamicAccess<Target>.Set(inst, key, oVal);
                            }

                            results.Add(inst);
                        }
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    try
                    {
                        if (null != package) package.Dispose();
                    }
                    catch
                    {
                        Console.WriteLine("package dispose error.");
                    }
                }
            }

            return results;
        }
    }
}
