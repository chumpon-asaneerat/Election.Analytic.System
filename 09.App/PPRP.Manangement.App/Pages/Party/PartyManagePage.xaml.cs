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
    /// Interaction logic for PartyManagePage.xaml
    /// </summary>
    public partial class PartyManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PartyManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sPartyNameFilter = string.Empty;
        private int iPageNo = 1;
        private int iMaxPage = 1;
        private int iRowsPerPage = 40;

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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MParty;
            Edit(item);
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MParty;
            Delete(item);
        }

        #endregion

        #region TextBox Handlers

        private void txtPartyNameFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true; // mark as handled
                // search
                Search();
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                e.Handled = true; // mark as handled
                // reset filter and search
                txtPartyNameFilter.Text = string.Empty;
                Search();
            }
        }

        #endregion

        #region Paging Handlers

        private void nav_PagingChanged(object sender, EventArgs e)
        {
            iPageNo = nav.PageNo;
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
            var win = PPRPApp.Windows.ImportPartyImage;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            RefreshList();
        }

        private void Search()
        {
            if (sPartyNameFilter.Trim() != txtPartyNameFilter.Text.Trim())
            {
                sPartyNameFilter = txtPartyNameFilter.Text.Trim();
                RefreshList();
            }
        }

        private void Edit(MParty item)
        {
            if (null == item)
                return;
            Console.WriteLine("Edit");
        }

        private void Delete(MParty item)
        {
            if (null == item)
                return;
            string msg = string.Format("ต้องการลบข้อมูล '{0}' ?", item.PartyName);
            if (MessageBox.Show(msg, "ยืนยันการลบข้อมูล", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                MParty.Delete(item);
                RefreshList();
            }
        }

        private void RefreshList()
        {
            lvParties.ItemsSource = null;
            var parties = MParty.Gets(sPartyNameFilter, iPageNo, iRowsPerPage);
            lvParties.ItemsSource = (null != parties) ? parties.Value : new List<MParty>();

            if (null != parties)
            {
                lvParties.SelectedIndex = 0;
                lvParties.ScrollIntoView(lvParties.SelectedItem);
            }

            iPageNo = (null != parties) ? parties.PageNo : 1;
            iMaxPage = (null != parties) ? parties.MaxPage : 1;

            nav.Setup(iPageNo, iMaxPage);
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            sPartyNameFilter = string.Empty;
            iPageNo = 1;
            iMaxPage = 1;

            RefreshList();
        }

        #endregion
    }
}
