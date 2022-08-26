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
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainMenuPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Button Handlers

        private void imgCmd1_Click(object sender, RoutedEventArgs e)
        {
            GotoExcelImportPage();
        }

        #endregion

        #region Private Methods

        private void GotoExcelImportPage()
        {
            var page = PPRPApp.Pages.ExcelSample;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Methods

        public void Setup() 
        {

        }

        #endregion
    }
}
