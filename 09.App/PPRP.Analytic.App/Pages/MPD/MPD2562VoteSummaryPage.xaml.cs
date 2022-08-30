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

        private string _regionId = string.Empty;
        private string _provinceId = string.Empty;

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
            if (!string.IsNullOrWhiteSpace(_regionId))
            {
                if (_regionId == "01")
                {
                    var page = PPRPApp.Pages.Pak01;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "02")
                {
                    var page = PPRPApp.Pages.Pak02;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "03")
                {
                    var page = PPRPApp.Pages.Pak03;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "04")
                {
                    var page = PPRPApp.Pages.Pak04;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "05")
                {
                    var page = PPRPApp.Pages.Pak05;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "06")
                {
                    var page = PPRPApp.Pages.Pak06;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "07")
                {
                    var page = PPRPApp.Pages.Pak07;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "08")
                {
                    var page = PPRPApp.Pages.Pak08;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "09")
                {
                    var page = PPRPApp.Pages.Pak09;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (_regionId == "10")
                {
                    var page = PPRPApp.Pages.Pak10;
                    page.Setup(_regionId);
                    PageContentManager.Instance.Current = page;
                }
            }
        }

        #endregion

        #region Public Methods

        public void Setup(string regionId, string provinceId)
        {
            _regionId = regionId;
            _provinceId = provinceId;
        }

        #endregion
    }
}
