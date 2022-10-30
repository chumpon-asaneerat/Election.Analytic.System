#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Domains;
using PPRP.Imports.Excel;

using OfficeOpenXml;
using EPPlus;
using EPPlus.DataExtractor;


#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ImportMPDC2566Window.xaml
    /// </summary>
    public partial class ImportMPDC2566Window : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportMPDC2566Window()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Class

        public class XlsMPDC2566 : MPDC2566
        {
            public static List<XlsMPDC2566> LoadWorksheetTable(NExcelImport import,
                string sheetName, List<NExcelMapProperty> mapProperties)
            {
                MethodBase med = MethodBase.GetCurrentMethod();

                var results = new List<XlsMPDC2566>();
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
                                var inst = new XlsMPDC2566();

                                foreach (var key in columns.Keys)
                                {
                                    int colIdx = columns[key];
                                    if (colIdx < 1)
                                        continue;
                                    try
                                    {
                                        object oVal = sheet.Cells[row, columns[key]].Value;
                                        DynamicAccess<XlsMPDC2566>.Set(inst, key, oVal);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
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

        #endregion

        #region Internal Variables

        private NExcelImport import = new NExcelImport();
        private List<XlsMPDC2566> items = null;

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

                items = XlsMPDC2566.LoadWorksheetTable(import, model.Worksheet.SheetName, model.Maps);
                if (null != items)
                {

                }
                lvMapPreview.UpdateItems(model.Maps, items);
            }
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
            Imports();
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
                    new string[] { "ProvinceName", "ข้อมูลชื่อจังหวัด" },
                    new string[] { "PollingUnitNo", "ข้อมูลเขตเลือกตั้งที่" },
                    new string[] { "CandidateNo", "ข้อมูลลำดับที่" },
                    new string[] { "FullName", "ข้อมูลชื่อผู้สมัคร" },
                    new string[] { "PrevPartyName", "ข้อมูลชื่อสังกัดพรรคเดิม" },
                    new string[] { "EducationLevel", "ข้อมูลวุฒิการศึกษา" },
                    new string[] { "SubGroup", "ข้อมูลกลุ่มย่อย" },
                    new string[] { "Remark", "ข้อมูลหมายเหตุ" }
                };

                wsMap.Setup(import, mapProperties);
            }
        }

        private void Imports()
        {
            if (null != items && items.Count > 0)
            {
                var prog = PPRPApp.Windows.ProgressDialog;
                prog.Owner = this;
                prog.Setup(items.Count);
                prog.Show();

                foreach (var item in items)
                {
                    MPDC2566.Import(item);
                    prog.Increment();
                }
                // Close progress dialog.
                prog.Close();
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
