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
    /// Interaction logic for Pak01Page.xaml
    /// </summary>
    public partial class Pak01Page : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pak01Page()
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

        public void Setup(PakMenuItem pak)
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
