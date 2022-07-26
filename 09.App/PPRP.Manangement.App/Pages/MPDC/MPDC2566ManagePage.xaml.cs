﻿#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    /// Interaction logic for MPDC2566ManagePage.xaml
    /// </summary>
    public partial class MPDC2566ManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566ManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sFullNameFilter = string.Empty;

        private int iPageNo = 1;
        private int iMaxPage = 1;
        private int iRowsPerPage = 4;

        #endregion

        #region Button Handlers

        private void cmdHome_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenuPage();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cmdImport_Click(object sender, RoutedEventArgs e)
        {
            Import();
        }

        private void cmdExport_Click(object sender, RoutedEventArgs e)
        {
            Export();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Print();
        }

        private void cmdAddNew_Click(object sender, RoutedEventArgs e)
        {
            AddNew();
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDC2566;
            Edit(item);
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDC2566;
            Delete(item);
        }

        #endregion

        #region ComboBox Handlers

        private void cbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            iPageNo = 1;
            iMaxPage = 1;

            RefreshList(true);
        }

        #endregion

        #region TextBox Handlers

        private void txtFullNameFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true; // mark as handled
                // search
                Search();
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                e.Handled = true; // mark as handled
                // reset filter and search
                //txtFullNameFilter.Text = string.Empty;
                Search();
            }
        }

        #endregion

        #region Paging Handlers

        private void nav_PagingChanged(object sender, EventArgs e)
        {
            iPageNo = nav.PageNo;
            RefreshList(false);
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
            var win = PPRPApp.Windows.ImportMPDC2566;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            LoadProvinces();
        }

        private void Export()
        {
            var items = MPDC2566PrintSummary.Gets(null).Value;
            if (null == items)
            {
                // Show Dialog.
                return;
            }
            NExcelExport export = new NExcelExport();
            if (export.ShowDialog("รายชื่อว่าที่ผู้สมัครปี 2566.xlsx"))
            {
                // map column and property
                export.Maps.Add(new NExcelExportColumn { ColumnName = "จังหวัด", PropertyName = "ProvinceName" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "เขต", PropertyName = "PollingUnitNo" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "ลำดับที่", PropertyName = "CandidateNo" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "ชื่อผู้สมัคร", PropertyName = "FullName" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "สังกัดพรรคเดิม", PropertyName = "PervPartyName" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "วุฒิการศึกษา", PropertyName = "EducationLvl" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "กลุ่มย่อย", PropertyName = "SubGroup" });
                export.Maps.Add(new NExcelExportColumn { ColumnName = "หมายเหตุ", PropertyName = "Remark" });
                if (export.Save(items, "รายชื่อว่าที่ผู้สมัครปี 2566"))
                {
                    MessageBox.Show("ส่งออกข้อมูลสำเร็จ", "ผลการส่งออกข้อมูล");
                }
                else
                {
                    MessageBox.Show("ส่งออกข้อมูลไม่สำเร็จ", "ผลการส่งออกข้อมูล");
                }
            }
        }

        private void Search()
        {
            if (sFullNameFilter.Trim() != txtFullNameFilter.Text.Trim())
            {
                sFullNameFilter = txtFullNameFilter.Text.Trim();
                RefreshList(true);
            }
        }

        private void Print()
        {
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            var items = MPDC2566PrintSummary.Gets(provinceName, sFullNameFilter).Value;
            if (null == items)
            {
                // Show Dialog.
                return;
            }
            var page = PPRPApp.Pages.MPDC2566PreviewSummaryPage;
            page.Setup(items);
            PageContentManager.Instance.Current = page;
        }

        private void AddNew()
        {
            MPDC2566 item = new MPDC2566();
            var editor = PPRPApp.Windows.MPDC2566Editor;
            editor.Setup(item, true);
            editor.ShowDialog();
            RefreshList(true);
        }

        private void Edit(MPDC2566 item)
        {
            if (null == item)
                return;
            var editor = PPRPApp.Windows.MPDC2566Editor;
            editor.Setup(item, false);
            editor.ShowDialog();
            RefreshList(true);
        }

        private void Delete(MPDC2566 item)
        {
            if (null == item)
                return;
            string msg = string.Format("ต้องการลบข้อมูล '{0}' ?", item.FullName);
            if (MessageBox.Show(msg, "ยืนยันการลบข้อมูล", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MPDC2566.Delete(item);
                RefreshList(true);
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

        private void RefreshList(bool refresh)
        {
            if (refresh)
            {
                iPageNo = 1;
            }

            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            lvMPDC2566.ItemsSource = null;
            var candidates = MPDC2566PullingUnit.Gets(provinceName, sFullNameFilter, iPageNo, iRowsPerPage);

            lvMPDC2566.ItemsSource = (null != candidates) ? candidates.Value : new List<MPDC2566PullingUnit>();
            sv.ScrollToHome(); // scroll to home position

            iPageNo = (null != candidates) ? candidates.PageNo : 1;
            iMaxPage = (null != candidates) ? candidates.MaxPage : 1;

            nav.Setup(iPageNo, iMaxPage);
        }

        #endregion

        #region Public Methods

        public void Setup(bool reload = true)
        {
            if (reload)
            {
                sFullNameFilter = string.Empty;
                LoadProvinces();
            }
        }

        #endregion
    }
}
