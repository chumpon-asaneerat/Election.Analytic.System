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
    /// Interaction logic for PollingStationManagePage.xaml
    /// </summary>
    public partial class PollingStationManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PollingStationManagePage()
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

        private void cbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProvinces();
        }

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
            var win = PPRPApp.Windows.ImportPollingStation;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            LoadRegions();
        }

        private void Export()
        {
            var items = PollingStation.Gets().Value;
            if (null == items)
            {
                // Show Dialog.
                return;
            }

            NExcelExport export = new NExcelExport();
            if (export.ShowDialog("ข้อมูลการเขต Polling Location (2562).xlsx"))
            {
                // map column and property
                export.Maps.Add(new NExcelExportColumn { ColumnName = "ภาค", PropertyName = "RegionName" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "ภาคทางภูมิศาสตร์", PropertyName = "GeoSubGroup" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "รหัสจังหวัด", PropertyName = "ProvinceId" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "จังหวัด", PropertyName = "ProvinceNameTH" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "รหัสอำเภอ", PropertyName = "DistrictId" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "อำเภอ", PropertyName = "DistrictNameTH" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "รหัสตำบล", PropertyName = "SubdistrictId" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "ตำบล", PropertyName = "SubdistrictNameTH" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "เขตเลือกตั้ง", PropertyName = "PollingUnitNo" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "รหัสหน่วยเลือกตั้งย่อย", PropertyName = "PollingSubUnitNo" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "จำนวนหมู่บ้าน", PropertyName = "VillageCount" });

                if (export.Save(items, "Polling"))
                {
                    MessageBox.Show("ส่งออกข้อมูลสำเร็จ", "ผลการส่งออกข้อมูล");
                }
                else
                {
                    MessageBox.Show("ส่งออกข้อมูลไม่สำเร็จ", "ผลการส่งออกข้อมูล");
                }
            }
        }

        private void LoadRegions()
        {
            cbRegion.ItemsSource = null;
            var regions = MRegion.Gets().Value;
            if (null != regions)
            {
                regions.Insert(0, new MRegion { RegionName = "ทุกภาค" });
            }
            cbRegion.ItemsSource = (null != regions) ? regions : new List<MRegion>();
            if (null != regions)
            {
                cbRegion.SelectedIndex = 0;
            }
        }

        private void LoadProvinces()
        {
            // Check region.
            var reion = cbRegion.SelectedItem as MRegion;
            string regionId = (null != reion) ? reion.RegionId : null;
            if (null != regionId && regionId.Contains("ทุกภาค"))
            {
                regionId = null;
            }

            cbProvince.ItemsSource = null;
            var provinces = MProvince.Gets(regionId: regionId).Value;
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
            // Check region.
            var reion = cbRegion.SelectedItem as MRegion;
            string regionName = (null != reion) ? reion.RegionName : null;
            if (null != regionName && regionName.Contains("ทุกภาค"))
            {
                regionName = null;
            }

            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            lvGeoLocations.ItemsSource = null;
            var locations = PollingStation.Gets(regionName, provinceName).Value;
            lvGeoLocations.ItemsSource = (null != locations) ? locations : new List<PollingStation>();
        }

        #endregion

        #region Public Methods

        public void Setup(bool reload = true)
        {
            if (reload)
            {
                LoadRegions();
            }
        }

        #endregion
    }
}
