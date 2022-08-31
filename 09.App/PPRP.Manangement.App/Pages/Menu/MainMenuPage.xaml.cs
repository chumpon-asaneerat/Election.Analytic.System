#region Using

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

        #endregion

        #region Group 2 - ข้อมูลทางภูมิศาสตร์

        private void mnu21_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลพื้นที่ (จังหวัด/อำเภอ/ตำบล(แขวง))
            GotoPullingStation();
        }

        private void mnu22_Click(object sender, RoutedEventArgs e)
        {
            // ข้อมูลแผนที่
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

        private void GotoMPD2562VoteSummary()
        {
            var page = PPRPApp.Pages.MPD2562VoteSummaryManage;
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

        #endregion

        #region Public Methods

        public void Setup() 
        {

        }

        #endregion
    }
}
