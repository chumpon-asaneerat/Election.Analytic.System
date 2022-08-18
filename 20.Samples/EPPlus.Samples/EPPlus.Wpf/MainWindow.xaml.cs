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

namespace EPPlus.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // set license.
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        #endregion

        #region Button Handlers

        private void cmdOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var xlsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Examples");
            string fileName = string.Empty;

            OpenFileDialog fd = new OpenFileDialog();
            //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fd.InitialDirectory = xlsPath;
            fd.Multiselect = false;
            fd.Filter = "Excel file (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileName = fd.FileName;
            }
            fd.Dispose();
            fd = null;
            if (string.IsNullOrWhiteSpace(fileName)) return;

            txtFileName.Text = fileName;
            LoadWorksheets(fileName);
        }

        #endregion

        #region ListView Handlers

        private void lvSheets_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string fileName = txtFileName.Text;
            var item = lvSheets.SelectedItem as Sheet;
            if (null == item) return;

            LoadSheetData(fileName, item.Name);
        }

        #endregion

        #region Private Methods

        private void LoadWorksheets(string fileName)
        {
            lvSheets.ItemsSource = null;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            List<Sheet> sheets = new List<Sheet>();
            using (var package = new ExcelPackage(fileName))
            {
                var worksheets = package.Workbook.Worksheets;
                foreach (var worksheet in worksheets)
                {
                    sheets.Add(new Sheet() { Name = worksheet.Name });
                }
            }

            lvSheets.ItemsSource = sheets;
        }


        private void LoadSheetData(string fileName, string sheetName)
        {
            gridSheetData.ItemsSource = null;

            using (var package = new ExcelPackage(fileName))
            {
                var sheet = package.Workbook.Worksheets[sheetName];
                if (null != sheet)
                {
                    var accessor = sheet.Extract<GeoLocation>()
                        // Here we can chain multiple definition for the columns
                        .WithProperty(p => p.ProvinceCode, "A")
                        .WithProperty(p => p.AmphurCode, "B")
                        .WithProperty(p => p.TumbonCode, "C")
                        .WithProperty(p => p.ProvinceName, "E")
                        .WithProperty(p => p.AmphurName, "F")
                        .WithProperty(p => p.TumbonName, "G");
                    var results = accessor
                        .GetData(3, row => null != sheet.Cells[row, 1].Value).ToList();
                    gridSheetData.ItemsSource = results;
                }
            }
        }

        #endregion

        public class Sheet
        {
            public string Name { get; set; }
        }

        public class GeoLocation
        {
            public string ProvinceCode { get; set; }
            public string AmphurCode { get; set; }

            public string TumbonCode { get; set; }

            public string ProvinceName { get; set; }

            public string AmphurName { get; set; }

            public string TumbonName { get; set; }
        }
    }
}
