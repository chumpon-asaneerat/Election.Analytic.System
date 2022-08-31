#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Reflection;

using NLib;
using NLib.Services;

using PPRP.Domains;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MPD2562VoteSummaryPage.xaml
    /// </summary>
    public partial class MPD2562VoteSummaryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562VoteSummaryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Helper Peroperties

        private PakMenuItem Current
        {
            get { return AreaNavi.Instance.Current; }
        }

        private List<ProvinceMenuItem> Provinces
        {
            get
            {
                var provinces = (null != AreaNavi.Instance.Current && null != AreaNavi.Instance.Current.Provinces) ?
                    AreaNavi.Instance.Current.Provinces : null;
                return provinces;
            }
        }

        #endregion

        #region Button Handlers

        private void cmdGotoThailandPage_Click(object sender, RoutedEventArgs e)
        {
            GotoThailandPage();
        }

        private void cmdGotoPrev_Click(object sender, RoutedEventArgs e)
        {
            GotoPrevPage();
        }

        private void cmdPollingUnit_Click(object sender, RoutedEventArgs e)
        {
            var pollingUnit = (sender as Button).DataContext as PollingUnitMenuItem;

            lstSummary.ItemsSource = MPD2562PersonalVoteSummary.Gets(
                pollingUnit.ProvinceId, pollingUnit.PollingUnitNo).Value;
        }

        #endregion

        #region Private Methods

        private void GotoThailandPage()
        {
            var page = PPRPApp.Pages.Thailand;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPrevPage()
        {
            AreaNavi.Instance.GoPrev();
            string regionId = (null != Current) ? Current.RegionId : string.Empty;

            if (!string.IsNullOrWhiteSpace(regionId))
            {
                if (regionId == "01")
                {
                    var page = PPRPApp.Pages.Pak01;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "02")
                {
                    var page = PPRPApp.Pages.Pak02;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "03")
                {
                    var page = PPRPApp.Pages.Pak03;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "04")
                {
                    var page = PPRPApp.Pages.Pak04;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "05")
                {
                    var page = PPRPApp.Pages.Pak05;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "06")
                {
                    var page = PPRPApp.Pages.Pak06;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "07")
                {
                    var page = PPRPApp.Pages.Pak07;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "08")
                {
                    var page = PPRPApp.Pages.Pak08;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "09")
                {
                    var page = PPRPApp.Pages.Pak09;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "10")
                {
                    var page = PPRPApp.Pages.Pak10;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
            }
        }

        #endregion

        #region Public Methods

        public void Setup(ProvinceMenuItem province)
        {
            txtProvinceName.Text = "จ.";
            lstPollingUnits.ItemsSource = null;

            if (null == province)
                return;

            txtProvinceName.Text = "จ." + province.ProvinceNameTH;
            lstPollingUnits.ItemsSource = PollingUnitMenuItem.Gets(province.RegionId, province.ProvinceId).Value;
        }

        #endregion
    }
}
