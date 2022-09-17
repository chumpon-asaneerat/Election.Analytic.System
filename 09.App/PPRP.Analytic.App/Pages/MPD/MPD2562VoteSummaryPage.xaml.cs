﻿#region Using

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

        #region Internal Class

        public class GeneralSummary
        {
            public List<MPD2562PersonalVoteSummary> Top6 { get; set; }
            public int PollingUnitCount { get; set; }
            public int RightCount { get; set; }
            public int ExerciseCount { get; set; }
            public int VoteCount7toLast { get; set; }
            public MPDC2566Summary CandidateNo1 { get; set; }
        }

        #endregion

        #region Internal Variables

        private ProvinceMenuItem _provinceItem = null;
        private PollingUnitMenuItem _pullingUnitItem = null;
        private GeneralSummary _generalSummary = null;

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

        private void cmdAreaInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowAreaInfo();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            GotoPrintPreview();
        }

        #endregion

        #region lstPollingUnits Handlers

        private void lstPollingUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pollingUnit = lstPollingUnits.SelectedItem as PollingUnitMenuItem;
            LoadSummary(pollingUnit);
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
            var top100 = MPD2562PersonalVoteSummary.Gets(100,
                _pullingUnitItem.ProvinceId, _pullingUnitItem.PollingUnitNo).Value;

            int sum6 = 0;
            if (null != top6 && top6.Count > 0)
            {
                foreach (var item in top100)
                {
                    sum6 += item.VoteCount;
                }
            }

            int sum100 = 0;
            if (null != top100 && top100.Count > 0)
            {
                foreach (var item in top100)
                {
                    sum100 += item.VoteCount;
                }
            }

            int diff = sum100 - sum6; // sum from 7-100
            txtTotalVotes.Text = diff.ToString("n0");

            lstSummary.ItemsSource = top6;

            var candidates = MPDC2566Summary.Gets(
            _pullingUnitItem.ProvinceId, _pullingUnitItem.PollingUnitNo).Value;
            lstCandidates.ItemsSource = candidates;

            // Create cache summary for print.
            _generalSummary = new GeneralSummary();
            _generalSummary.Top6 = top6;
            _generalSummary.CandidateNo1 = (null != candidates && candidates.Count > 0) ? candidates[0] : null;
            _generalSummary.VoteCount7toLast = diff;

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

            if (null != _generalSummary)
            {
                _generalSummary.PollingUnitCount = unitSum.PollingUnitCount;
                _generalSummary.RightCount = unitSum.RightCount;
                _generalSummary.ExerciseCount = unitSum.ExerciseCount;
            }
        }

        private void GotoPrintPreview()
        {
            // prepare report item.
            MPDPrintVoteSummary item = new MPDPrintVoteSummary();
            if (null != _generalSummary)
            {
                if (null != _generalSummary.Top6 && _generalSummary.Top6.Count > 0)
                {
                    // Person 1
                    if (null != _generalSummary.Top6[0])
                    {
                        var p = _generalSummary.Top6[0];
                        item.Logo1 = p.LogoData;
                        item.PersonImage1 = p.PersonImageData;
                        item.PartyName1 = "1." + p.PartyName;
                        item.FullName1 = p.FullName;
                        item.VoteCount1 = p.VoteCount;
                    }
                    // Person 2
                    if (null != _generalSummary.Top6[1])
                    {
                        var p = _generalSummary.Top6[1];
                        item.Logo2 = p.LogoData;
                        item.PersonImage2 = p.PersonImageData;
                        item.PartyName2 = "2." + p.PartyName;
                        item.FullName2 = p.FullName;
                        item.VoteCount2 = p.VoteCount;
                    }
                    // Person 3
                    if (null != _generalSummary.Top6[2])
                    {
                        var p = _generalSummary.Top6[2];
                        item.Logo3 = p.LogoData;
                        item.PersonImage3 = p.PersonImageData;
                        item.PartyName3 = "3." + p.PartyName;
                        item.FullName3 = p.FullName;
                        item.VoteCount3 = p.VoteCount;
                    }
                    // Person 4
                    if (null != _generalSummary.Top6[3])
                    {
                        var p = _generalSummary.Top6[3];
                        item.Logo4 = p.LogoData;
                        item.PersonImage4 = p.PersonImageData;
                        item.PartyName4 = "3." + p.PartyName;
                        item.FullName4 = p.FullName;
                        item.VoteCount4 = p.VoteCount;
                    }
                    // Person 5
                    if (null != _generalSummary.Top6[4])
                    {
                        var p = _generalSummary.Top6[4];
                        item.Logo5 = p.LogoData;
                        item.PersonImage5 = p.PersonImageData;
                        item.PartyName5 = "5." + p.PartyName;
                        item.FullName5 = p.FullName;
                        item.VoteCount5 = p.VoteCount;
                    }
                    // Person 6
                    if (null != _generalSummary.Top6[5])
                    {
                        var p = _generalSummary.Top6[5];
                        item.Logo6 = p.LogoData;
                        item.PersonImage6 = p.PersonImageData;
                        item.PartyName6 = "6." + p.PartyName;
                        item.FullName6 = p.FullName;
                        item.VoteCount6 = p.VoteCount;
                    }

                    // Candidate
                    if (null != _generalSummary.CandidateNo1)
                    {
                        var candidate = _generalSummary.CandidateNo1;
                        item.CandidateImage = candidate.PersonImageData;
                        item.CandidateFullName = candidate.FullName;
                        item.CandidateSubGroup = candidate.SubGroup;
                        item.CandidateRemark = candidate.Remark;

                        item.CandidatePrevYear = string.Empty; // Hardcode Thai Year
                        item.CandidatePrevVote = string.Empty;
                    }

                    // General
                    item.PrevVoteYear = 2562; // Hardcode Thai Year
                    item.RightCount = _generalSummary.RightCount;
                    item.ExerciseCount = _generalSummary.ExerciseCount;
                    item.DifferenceVoteFromNo2 = (null != _generalSummary.Top6[0] && null != _generalSummary.Top6[1]) ?
                        _generalSummary.Top6[0].VoteCount - _generalSummary.Top6[1].VoteCount : 0;
                    item.VoteCount7toLast = _generalSummary.VoteCount7toLast;
                }
            }

            var page = PPRPApp.Pages.MPDPreviewVoteSummary;
            int idx = lstPollingUnits.SelectedIndex;
            page.Setup(_provinceItem, idx, item);
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Methods

        public void Setup(ProvinceMenuItem province)
        {
            txtProvinceName.Text = "จ.";
            _pullingUnitItem = null;
            lstPollingUnits.SelectedIndex = -1;
            lstPollingUnits.SelectedItem = null;
            lstPollingUnits.ItemsSource = null;

            _provinceItem = province;
            _generalSummary = null;

            if (null == province)
                return;

            txtProvinceName.Text = "จ." + province.ProvinceNameTH;
            var items = PollingUnitMenuItem.Gets(province.RegionId, province.ProvinceId).Value;
            lstPollingUnits.ItemsSource = items;
            if (null != items && items.Count > 0)
            {
                lstPollingUnits.SelectedIndex = 0; // auto select first item.
                lstPollingUnits.ScrollIntoView(items[0]);
                LoadSummary(items[0]); // update display
            }
        }

        public void Setup(ProvinceMenuItem province, int selectIndex)
        {
            txtProvinceName.Text = "จ.";
            _pullingUnitItem = null;
            lstPollingUnits.SelectedIndex = -1;
            lstPollingUnits.SelectedItem = null;
            lstPollingUnits.ItemsSource = null;

            _provinceItem = province;
            _generalSummary = null;

            if (null == province)
                return;

            txtProvinceName.Text = "จ." + province.ProvinceNameTH;
            var items = PollingUnitMenuItem.Gets(province.RegionId, province.ProvinceId).Value;
            lstPollingUnits.ItemsSource = items;
            if (null != items && items.Count > 0)
            {
                if (selectIndex > -1 && selectIndex < items.Count)
                {
                    lstPollingUnits.SelectedIndex = selectIndex; // auto select first item.
                    lstPollingUnits.ScrollIntoView(items[selectIndex]);
                    LoadSummary(items[selectIndex]); // update display
                }
                else
                {
                    lstPollingUnits.SelectedIndex = 0; // auto select first item.
                    lstPollingUnits.ScrollIntoView(items[0]);
                    LoadSummary(items[0]); // update display
                }
            }
        }

        #endregion
    }
}
