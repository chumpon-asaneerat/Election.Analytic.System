#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Linq;

using System.Windows;

using NLib;
using NLib.Reflection;

using OfficeOpenXml;
using EPPlus;
using EPPlus.DataExtractor;

#endregion


namespace PPRP.Exports.Excel
{
    #region NExcelExportColumn

    public class NExcelExportColumn
    {
        public string ColumnName { get; set; }
        public string PropertyName { get; set; }
    }


    #endregion

    #region NExcelExport

    public class NExcelExport
    {
        #region Static Methods - Register License

        /// <summary>
        /// Register License.
        /// </summary>
        public static void RegisterLicense()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NExcelExport() : base()
        {
            RegisterLicense();
            this.Maps = new List<NExcelExportColumn>();
        }

        #endregion

        #region Show Save Excel Dialog

        public bool ShowDialog(string defaultFileName)
        {
            return ShowDialog(null, null, "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล", defaultFileName);
        }
        public bool ShowDialog(string title = "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล",
            string initDir = null)
        {
            return ShowDialog(null, title, initDir);
        }
        public bool ShowDialog(Window owner,
            string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null,
            string defaultFileName = "")
        {
            bool ret = false;

            // setup dialog options
            var sd = new Microsoft.Win32.SaveFileDialog();
            sd.InitialDirectory = initDir;
            sd.Title = string.IsNullOrEmpty(title) ? "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล" : title;
            sd.Filter = "Excel Files(*.xls, *.xlsx)|*.xls;*.xlsx";
            sd.FileName = defaultFileName;
            ret = sd.ShowDialog(owner) == true;
            if (ret)
            {
                // assigned to FileName
                this.FileName = sd.FileName;
            }
            sd = null;

            return ret;
        }

        #endregion

        #region Public Methods

        public bool Save<T>(List<T> items, string sheetName = "Sheet1")
            where T:class
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            bool ret = false;
            if (string.IsNullOrWhiteSpace(FileName))
                return ret;

            if (null == items || items.Count <= 0)
            {
                return ret;
            }
            // ปรับ เพิ่ม Export Excel
            using (var package = new ExcelPackage(FileName))
            {
                try
                {
                    var sheet = package.Workbook.Worksheets[sheetName]; // check exists
                    if (null == sheet) sheet = package.Workbook.Worksheets.Add(sheetName); // not exist create new
                    if (null != sheet)
                    {
                        int iRo1 = 1;
                        items.ForEach(item => 
                        {
                            int iCol = 1;
                            Maps.ForEach(map =>
                            {
                                if (iRo1 == 1)
                                {
                                    sheet.Cells[iRo1, iCol].Value = map.ColumnName;
                                    sheet.Cells[2, iCol].Value = DynamicAccess<T>.Get(item, map.PropertyName); ;
                                }
                                else if (iRo1 == 2)
                                {
                                    iRo1 = 3;
                                    sheet.Cells[iRo1, iCol].Value = DynamicAccess<T>.Get(item, map.PropertyName);
                                }
                                else
                                {
                                    // write data
                                    sheet.Cells[iRo1, iCol].Value = DynamicAccess<T>.Get(item, map.PropertyName);
                                }
                                iCol++;
                            });
                            iRo1++;
                        });


                        // save to file.
                        package.SaveAs(this.FileName);
                        ret = true;
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }

            return ret;
        }

        #endregion

        #region Public Properties

        public string FileName { get; private set; }

        public List<NExcelExportColumn> Maps { get; set; }

        #endregion
    }

    #endregion
}
