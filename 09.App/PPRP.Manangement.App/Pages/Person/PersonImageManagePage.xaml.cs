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
    /// Interaction logic for PersonImageManagePage.xaml
    /// </summary>
    public partial class PersonImageManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PersonImageManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sFullNameFilter = string.Empty;
        private int iPageNo = 1;
        private int iMaxPage = 1;
        private int iRowsPerPage = 10;

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
            var win = PPRPApp.Windows.ImportPersonImage;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            RefreshList();
        }

        private void RefreshList()
        {
            lvPersons.ItemsSource = null;
            var persons = PersonImage.Gets(sFullNameFilter, iPageNo, iRowsPerPage);
            lvPersons.ItemsSource = (null != persons) ? persons.Value : new List<PersonImage>();

            iPageNo = (null != persons) ? iPageNo = persons.PageNo : 1;
            iMaxPage = (null != persons) ? iPageNo = persons.MaxPage : 1;

            nav.Setup(iPageNo, iMaxPage);
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            sFullNameFilter = string.Empty;
            iPageNo = 1;
            iMaxPage = 1;

            RefreshList();
        }

        #endregion
    }
}
