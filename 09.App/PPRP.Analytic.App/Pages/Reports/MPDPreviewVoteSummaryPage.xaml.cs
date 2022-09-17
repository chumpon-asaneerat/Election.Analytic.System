﻿#region Using

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
        private MPDPrintVoteSummary _item = null;

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

        #region Navigate Methods

        private void GotoMPD2562VoteSummary()
        {
            // Report Menu Page
            var page = PPRPApp.Pages.MPD2562VoteSummary;
            page.Setup(null, 0);
            PageContentManager.Instance.Current = page;
        }

        private void Print()
        {
            cmdPrint.Visibility = Visibility.Collapsed;

            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                this.rptViewer.Print(ReportDisplayName);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            cmdPrint.Visibility = Visibility.Visible;

            GotoMPD2562VoteSummary();
        }

        #endregion

        #region Report methods

        private string ReportDisplayName
        {
            get { return "MPDVoteSummary." + DateTime.Now.ToThaiDateTimeString("ddMMyyyyHHmmssfff"); }
        }

        private RdlcReportModel GetReportModel()
        {
            Assembly assembly = this.GetType().Assembly;
            RdlcReportModel inst = new RdlcReportModel();

            // Set Display Name (default file name).
            inst.DisplayName = ReportDisplayName;

            inst.Definition.EmbededReportName = "DMT.TOD.Reports.RevenueSlip.rdlc";
            inst.Definition.RdlcInstance = RdlcReportUtils.GetEmbededReport(assembly,
                inst.Definition.EmbededReportName);
            // clear reprot datasource.
            inst.DataSources.Clear();

            List<MPDPrintVoteSummary> items = new List<MPDPrintVoteSummary>();
            if (null != _item)
            {
                items.Add(_item); // Add new because is blank.
            }

            // assign new data source
            RdlcReportDataSource mainDS = new RdlcReportDataSource();
            mainDS.Name = "main"; // the datasource name in the rdlc report.
            mainDS.Items = items; // setup data source
            // Add to datasources
            inst.DataSources.Add(mainDS);

            // Add parameters (if required).
            DateTime today = DateTime.Now;
            string printDate = today.ToThaiDateTimeString("dd/MM/yyyy HH:mm:ss");
            inst.Parameters.Add(RdlcReportParameter.Create("PrintDate", printDate));

            return inst;
        }

        #endregion

        #endregion

        #region Public Methods

        public void Setup()
        {
            _item = null;
        }

        #endregion
    }
}