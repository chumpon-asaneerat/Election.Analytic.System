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
    /// Interaction logic for Pak10Page.xaml
    /// </summary>
    public partial class Pak10Page : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pak10Page()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<ProvinceMenuItem> _provinces = null;

        #endregion

        #region Button Handlers

        private void cmdGotoThailandPage_Click(object sender, RoutedEventArgs e)
        {
            GotoThailandPage();
        }

        private void cmdProvince_Click(object sender, RoutedEventArgs e)
        {
            var province = (sender as Button).DataContext as ProvinceMenuItem;
            if (null != province)
            {
                var page = PPRPApp.Pages.MPD2562VoteSummary;
                page.Setup(province.RegionId, province.ProvinceId);
                PageContentManager.Instance.Current = page;
            }
        }

        #endregion

        #region Private Methods

        private void GotoThailandPage()
        {
            var page = PPRPApp.Pages.Thailand;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Methods

        public void Setup(string regiondId)
        {
            _provinces = ProvinceMenuItem.Gets(regiondId).Value;
            if (null != _provinces)
            {
                Console.WriteLine("No of region : {0}", _provinces.Count);
            }
            lstProvinces.ItemsSource = _provinces;
        }

        #endregion
    }
}
