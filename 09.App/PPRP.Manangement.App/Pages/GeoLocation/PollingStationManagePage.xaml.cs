#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;

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
        }

        #endregion

        #region Public Methods

        public void Setup()
        {

        }

        #endregion
    }
}
