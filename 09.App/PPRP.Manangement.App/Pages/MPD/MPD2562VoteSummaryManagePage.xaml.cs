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
    /// Interaction logic for MPD2562VoteSummaryManagePage.xaml
    /// </summary>
    public partial class MPD2562VoteSummaryManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562VoteSummaryManagePage()
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

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Print();
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
            var win = PPRPApp.Windows.ImportMPD2562VoteSummary;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            RefreshList();
        }

        private void Print()
        {
            var items = MPD2562PrintVoteSummary.Gets(null).Value;
            if (null == items)
            {
                // Show Dialog.
                return;
            }
            var page = PPRPApp.Pages.MPD2562PreviewVoteSummary;
            page.Setup(items);
            PageContentManager.Instance.Current = page;
        }

        private void RefreshList()
        {
            lvMPD2562Summaries.ItemsSource = null;
            var summaries = MPD2562VoteSummary.Gets().Value;
            lvMPD2562Summaries.ItemsSource = (null != summaries) ? summaries : new List<MPD2562VoteSummary>();
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
