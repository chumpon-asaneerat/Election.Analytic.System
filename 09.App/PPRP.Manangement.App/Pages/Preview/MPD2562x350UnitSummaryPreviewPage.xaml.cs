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
    /// Interaction logic for MPD2562x350UnitSummaryPreviewPage.xaml
    /// </summary>
    public partial class MPD2562x350UnitSummaryPreviewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562x350UnitSummaryPreviewPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<MPD2562x350PrintUnitSummary> _items = null;

        #endregion

        #region Button Handlers

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Print();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            GotoMPD2562x350UnitSummary();
        }

        #endregion

        #region Private Methods

        #region Navigate Methods

        private void GotoMPD2562x350UnitSummary()
        {
            // Back to Manage Page
            var page = PPRPApp.Pages.MPD2562x350UnitSummaryManage;
            page.Setup(false);
            PageContentManager.Instance.Current = page;
        }

        private void Print()
        {
            cmdPrint.Visibility = Visibility.Collapsed;

            MethodBase med = MethodBase.GetCurrentMethod();
            try
            {
                if (null != _items)
                {
                    this.rptViewer.Print(ReportDisplayName);
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            cmdPrint.Visibility = Visibility.Visible;

            GotoMPD2562x350UnitSummary();
        }

        #endregion

        #region Report methods

        private string ReportDisplayName
        {
            get { return "MPD2562x350PrintUnitSummary." + DateTime.Now.ToThaiDateTimeString("ddMMyyyyHHmmssfff"); }
        }

        private RdlcReportModel GetReportModel()
        {
            Assembly assembly = this.GetType().Assembly;
            RdlcReportModel inst = new RdlcReportModel();

            // Set Display Name (default file name).
            inst.DisplayName = ReportDisplayName;

            inst.Definition.EmbededReportName = "PPRP.Reports.MPD2562x350UnitSummaryReport.rdlc";
            inst.Definition.RdlcInstance = RdlcReportUtils.GetEmbededReport(assembly,
                inst.Definition.EmbededReportName);
            // clear reprot datasource.
            inst.DataSources.Clear();

            List<MPD2562x350PrintUnitSummary> items = new List<MPD2562x350PrintUnitSummary>();
            if (null != _items)
            {
                foreach (var item in _items)
                {
                    items.Add(item); // Add new because is blank.
                }
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

        public void Setup(List<MPD2562x350PrintUnitSummary> items)
        {
            _items = items;

            if (null == _items)
            {
                // something invalid?.
            }
            var model = GetReportModel();
            if (null == model ||
                null == model.DataSources || model.DataSources.Count <= 0 ||
                null == model.DataSources[0] || null == model.DataSources[0].Items)
            {
                /*
                var win = TODApp.Windows.MessageBox;
                win.Setup("ไม่พบข้อมูลในการจัดพิมพ์รายงาน.", "DMT - Tour of Duty");
                win.ShowDialog();
                */
                this.rptViewer.ClearReport();
            }
            else
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    this.rptViewer.LoadReport(model);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
        }

        #endregion
    }
}
