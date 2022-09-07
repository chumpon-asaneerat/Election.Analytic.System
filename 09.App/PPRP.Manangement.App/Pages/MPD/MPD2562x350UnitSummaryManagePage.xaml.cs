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
    /// Interaction logic for MPD2562x350UnitSummaryManagePage.xaml
    /// </summary>
    public partial class MPD2562x350UnitSummaryManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562x350UnitSummaryManagePage()
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
            /*
            var win = PPRPApp.Windows.ImportMPD2562VoteSummary;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            RefreshList();
            */
        }

        private void RefreshList()
        {
            lvMPD2562x350Units.ItemsSource = null;
            var summaries = MPD2562x350UnitSummary.Gets();
            lvMPD2562x350Units.ItemsSource = (null != summaries) ? summaries.Value : new List<MPD2562x350UnitSummary>();
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