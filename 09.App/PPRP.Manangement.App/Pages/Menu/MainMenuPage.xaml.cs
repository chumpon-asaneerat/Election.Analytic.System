﻿#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainMenuPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Menu Handlers

        private void imgCmd1_Click(object sender, RoutedEventArgs e)
        {
            GotoExcelImportPage();
        }

        #region Group 0 - การจัดการข้อมูลการเลือกตั้ง

        private void mnu01_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทน แบบแบ่งเขต ปี 2562 - MPD (summary)
            GotoMPD2562VoteSummary();
        }

        private void mnu02_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทน แบบบัญชีรายชื่อ ปี 2562 - MPR (summary)
        }

        private void mnu03_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลว่าที่ผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทน แบบแบ่งเขต ปี 2566 - MPD (candidate)
            GotoMPDC2566();
        }

        private void mnu04_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลว่าที่ผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทน แบบบัญชีรายชื่อ ปี 2566 - MPR (candidate)
        }

        private void mnu05_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลผู้ใช้สิทธิ 350 เขต ปี 2562 - MPD 350 Unit(summary)
            GotoMPD2562x350UnitSummary();
        }

        private void mnu06_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลหน่วยเลือกตั้งแบบแบ่งเขต ปี 2562 - MPD Polling Unit summary.
            GotoMPD2562PollingUnitSummary();
        }

        private void mnu07_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลพื้นที่ 2562 (เพิ่มเติม)
            GotoMPD2562AreaRemarkSummary();
        }

        private void mnu08_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลหน่วยเลือกตั้งแบบแบ่งเขต ปี 2566 - MPD Polling Unit summary.
            GotoMPD2566PollingUnitSummary();
        }

        #endregion

        #region Group 2 - ข้อมูลทางภูมิศาสตร์

        private void mnu21_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลพื้นที่ (จังหวัด/อำเภอ/ตำบล(แขวง))
            GotoPullingStation();
        }

        private void mnu22_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลจังหวัด (MProvince)
            GotoProvinceManagePage();
        }

        private void mnu23_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลอำเภอ (MDistrict)
            GotoDistrictManagePage();
        }

        private void mnu24_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลตำบล (MSubdistrict)
            GotoSubdistrictManagePage();
        }

        #endregion

        #region Group 4 - ข้อมูลหลัก

        private void mnu41_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลพรรคการเมือง
            GotoPartyManagePage();
        }

        private void mnu42_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลรูปผู้สมัคร-ว่าที่ผู้สมัคร
            GotoPersonImagePage();
        }

        private void mnu43_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลอื่น ๆ
        }

        #endregion

        #endregion

        #region Private Methods

        private void GotoExcelImportPage()
        {
            var page = PPRPApp.Pages.ExcelSample;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPartyManagePage()
        {
            var page = PPRPApp.Pages.PartyManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPullingStation()
        {
            var page = PPRPApp.Pages.PollingStationManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPD2562PollingUnitSummary()
        {
            var page = PPRPApp.Pages.MPD2562PollingUnitSummaryManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPD2562AreaRemarkSummary()
        {
            var page = PPRPApp.Pages.MPD2562AreaRemarkManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPD2566PollingUnitSummary()
        {
            var page = PPRPApp.Pages.MPD2566PollingUnitSummaryManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPD2562VoteSummary()
        {
            var page = PPRPApp.Pages.MPD2562VoteSummaryManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPD2562x350UnitSummary()
        {
            var page = PPRPApp.Pages.MPD2562x350UnitSummaryManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPDC2566()
        {
            var page = PPRPApp.Pages.MPDC2566Manage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPersonImagePage()
        {
            var page = PPRPApp.Pages.PersonImageManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoProvinceManagePage()
        {
            var page = PPRPApp.Pages.MProvinceManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoDistrictManagePage()
        {
            var page = PPRPApp.Pages.MDistrictManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoSubdistrictManagePage()
        {
            var page = PPRPApp.Pages.MSubdistrictManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Methods

        public void Setup() 
        {

        }

        #endregion
    }
}
