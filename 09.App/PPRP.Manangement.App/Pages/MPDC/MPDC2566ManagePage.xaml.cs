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
    /// Interaction logic for MPDC2566ManagePage.xaml
    /// </summary>
    public partial class MPDC2566ManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566ManagePage()
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
            var win = PPRPApp.Windows.ImportMPDC2566;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            RefreshList();
        }

        private void RefreshList()
        {
            lvMPDC2566.ItemsSource = null;
            var candidates = MPDC2566.Gets();
            lvMPDC2566.ItemsSource = (null != candidates) ? candidates.Value : new List<MPDC2566>();
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
