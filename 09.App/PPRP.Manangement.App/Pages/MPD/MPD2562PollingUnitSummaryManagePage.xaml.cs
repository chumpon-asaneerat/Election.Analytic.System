#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;

using PPRP.Domains;
using PPRP.Exports.Excel;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MPD2562PollingUnitSummaryManagePage.xaml
    /// </summary>
    public partial class MPD2562PollingUnitSummaryManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562PollingUnitSummaryManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdHome_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenuPage();
        }

        private void cmdImport_Click(object sender, RoutedEventArgs e)
        {
            Import();
        }

        private void cmdExport_Click(object sender, RoutedEventArgs e)
        {
            Export();
        }

        #endregion

        #region ComboBox Handlers

        private void cbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }

        #endregion

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void Import()
        {
            var win = PPRPApp.Windows.ImportMPD2562PollingUnitSummary;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            LoadProvinces();
        }

        private void Export()
        {
            var items = MPD2562PollingUnitSummary.Gets().Value;
            if (null == items)
            {
                // Show Dialog.
                return;
            }

            NExcelExport export = new NExcelExport();
            if (export.ShowDialog("ข้อมูลการเขตเลือกตั้งปี 2562.xlsx"))
            {
                // map column and property
                export.Maps.Add(new NExcelExportColumn { ColumnName = "จังหวัด", PropertyName = "ProvinceName" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "เขตเลือกตั้ง", PropertyName = "PollingUnitNo" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "จำนวนหน่วยเลือกตั้ง", PropertyName = "PollingUnitCount" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "ข้อมูลพื้นที่", PropertyName = "AreaRemark" });

                if (export.Save(items, "หน่วยเลือกตั้งแบบแบ่งเขต 2562"))
                {
                    MessageBox.Show("ส่งออกข้อมูลสำเร็จ", "ผลการส่งออกข้อมูล");
                }
                else
                {
                    MessageBox.Show("ส่งออกข้อมูลไม่สำเร็จ", "ผลการส่งออกข้อมูล");
                }
            }
        }

        private void LoadProvinces()
        {
            cbProvince.ItemsSource = null;
            var provinces = MProvince.Gets().Value;
            if (null != provinces)
            {
                provinces.Insert(0, new MProvince { ProvinceNameTH = "ทุกจังหวัด" });
            }
            cbProvince.ItemsSource = (null != provinces) ? provinces : new List<MProvince>();
            if (null != provinces)
            {
                cbProvince.SelectedIndex = 0;
            }
        }

        private void RefreshList()
        {
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            lvMPD2562Summaries.ItemsSource = null;
            var summaries = MPD2562PollingUnitSummary.Gets(provinceName);
            lvMPD2562Summaries.ItemsSource = (null != summaries) ? summaries.Value : new List<MPD2562PollingUnitSummary>();
        }

        #endregion

        #region Public Methods

        public void Setup(bool reload = true)
        {
            if (reload)
            {
                LoadProvinces();
            }
        }

        #endregion
    }
}
