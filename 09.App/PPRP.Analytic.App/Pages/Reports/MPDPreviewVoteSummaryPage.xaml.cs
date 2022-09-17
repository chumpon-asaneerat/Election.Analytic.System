#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using PPRP.Domains;
using PPRP.Services;
using NLib;
using NLib.Services;
using NLib.Reflection;
using NLib.Reports.Rdlc;
using System.Reflection;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MPDPreviewVoteSummaryPage.xaml
    /// </summary>
    public partial class MPDPreviewVoteSummaryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDPreviewVoteSummaryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private PollingUnitMenuItem _pullingUnitItem = null;

        #endregion

        #region Button Handlers

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Print();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            GotoMPD2562VoteSummary();
        }

        #endregion

        #region Private Methods

        private void GotoMPD2562VoteSummary()
        {
            // Report Menu Page
            var page = PPRPApp.Pages.MPD2562VoteSummary;
            page.Setup(null, 0);
            PageContentManager.Instance.Current = page;
        }

        private void Print()
        {

        }

        #endregion

        #region Public Methods

        public void Setup()
        {

        }

        #endregion
    }
}
