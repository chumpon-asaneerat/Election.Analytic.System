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

        #region Internal Variables

        private PollingUnitMenuItem _pullingUnitItem = null;

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
            LoadSummary(pollingUnit);
        }

        private void cmdAreaInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowAreaInfo();
        }

        #endregion

        #region lstCandidates ListBoxItem Handlers

        private void lstCandidates_ListBoxItem_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Uncomment code in xaml to use this method.
            var item = sender as ListBoxItem;
            if (null == item || null == item.DataContext) return;
            var inst = item.DataContext as MPDC2566Summary;
            ShowPreview(inst);
        }

        private void lstCandidates_ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (null == item || null == item.DataContext) return;
            var inst = item.DataContext as MPDC2566Summary;
            ShowPreview(inst);
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

        private void ShowPreview(MPDC2566Summary inst)
        {
            if (null == inst) return;

            var win = PPRPApp.Windows.MPDC2566Preview;
            win.Setup(inst);
            win.ShowDialog();
        }

        private void ShowAreaInfo()
        {
            MPD2562PollingUnitSummary summary = null;

            if (null != _pullingUnitItem)
            {
                summary = MPD2562PollingUnitSummary.Get(
                    _pullingUnitItem.ProvinceNameTH, _pullingUnitItem.PollingUnitNo).Value;
            }

            var win = PPRPApp.Windows.MPDC2562AreaRemark;
            win.Setup(summary);
            win.ShowDialog();
        }

        private void LoadSummary(PollingUnitMenuItem pollingUnit)
        {
            txtPollingUnitInfo.Text = "-";
            txtTotalVotes.Text = "-";
            txtPollingUnitCount.Text = "-";
            txtRightCount.Text = "-";
            txtExerciseCount.Text = "-";

            _pullingUnitItem = pollingUnit; // keep current

            if (null == _pullingUnitItem)
                return;

            txtPollingUnitInfo.Text = pollingUnit.DisplayText;

            var top6 = MPD2562PersonalVoteSummary.Gets(6,
                _pullingUnitItem.ProvinceId, _pullingUnitItem.PollingUnitNo).Value;
            var top16 = MPD2562PersonalVoteSummary.Gets(16,
                _pullingUnitItem.ProvinceId, _pullingUnitItem.PollingUnitNo).Value;

            int sum6 = 0;
            if (null != top6 && top6.Count > 0)
            {
                foreach (var item in top6)
                {
                    sum6 += item.VoteCount;
                }
            }

            int sum16 = 0;
            if (null != top16 && top16.Count > 0)
            {
                foreach (var item in top16)
                {
                    sum16 += item.VoteCount;
                }
            }

            int diff = sum16 - sum6; // sum from 7-16
            txtTotalVotes.Text = diff.ToString("n0");

            lstSummary.ItemsSource = top6;

            lstCandidates.ItemsSource = MPDC2566Summary.Gets(
                _pullingUnitItem.ProvinceId, _pullingUnitItem.PollingUnitNo).Value;

            UpdatePollingUnitSummary(_pullingUnitItem.ProvinceNameTH, _pullingUnitItem.PollingUnitNo);
        }

        private void UpdatePollingUnitSummary(string provinceName, int pollingUnitNo)
        {
            txtPollingUnitCount.Text = "-";
            txtRightCount.Text = "-";
            txtExerciseCount.Text = "-";

            var unitSum = MPD2562x350UnitSummary.Get(provinceName, pollingUnitNo).Value;
            if (null == unitSum) return;
            txtPollingUnitCount.Text = unitSum.PollingUnitCount.ToString("n0");
            txtRightCount.Text = unitSum.RightCount.ToString("n0");
            txtExerciseCount.Text = unitSum.ExerciseCount.ToString("n0");
        }

        #endregion

        #region Public Methods

        public void Setup(ProvinceMenuItem province)
        {
            txtProvinceName.Text = "จ.";
            _pullingUnitItem = null;
            lstPollingUnits.ItemsSource = null;


            if (null == province)
                return;

            txtProvinceName.Text = "จ." + province.ProvinceNameTH;
            var items = PollingUnitMenuItem.Gets(province.RegionId, province.ProvinceId).Value;
            lstPollingUnits.ItemsSource = items;
            if (null != items && items.Count > 0)
            {
                lstPollingUnits.SelectedIndex = 0; // auto select first item.
                LoadSummary(items[0]); // update display
            }
        }

        #endregion
    }
}
