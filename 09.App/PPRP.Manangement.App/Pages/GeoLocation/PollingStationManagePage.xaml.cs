﻿#region Using

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

        #endregion

        #region ComboBox Handlers

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
            LoadProvinces();
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

        private void RefreshList()
        {
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            lvGeoLocations.ItemsSource = null;
            var locations = PollingStation.Gets();
            lvGeoLocations.ItemsSource = (null != locations) ? locations.Value : new List<PollingStation>();
        }

        #endregion

        #region Public Methods

        public void Setup(bool reload = true)
        {
            if (reload)
            {
                LoadProvinces();
            }
        }

        #endregion
    }
}
