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
    /// Interaction logic for MPD2562AreaRemarkManagePage.xaml
    /// </summary>
    public partial class MPD2562AreaRemarkManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562AreaRemarkManagePage()
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

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void Import()
        {
            var win = PPRPApp.Windows.ImportMPD2562AreaRemarkSummary;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            RefreshList();
        }

        private void RefreshList()
        {
            lvMPD2562Items.ItemsSource = null;
            var summaries = MPD2562PollingUnitSummary.Gets();
            lvMPD2562Items.ItemsSource = (null != summaries) ? summaries.Value : new List<MPD2562PollingUnitSummary>();
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            RefreshList();
        }

        #endregion
    }
}
