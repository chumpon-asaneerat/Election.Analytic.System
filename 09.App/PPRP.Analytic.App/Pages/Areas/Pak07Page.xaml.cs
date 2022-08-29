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
    /// Interaction logic for Pak07Page.xaml
    /// </summary>
    public partial class Pak07Page : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pak07Page()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdGotoThailandPage_Click(object sender, RoutedEventArgs e)
        {
            GotoThailandPage();
        }

        #endregion

        #region Private Methods

        private void GotoThailandPage()
        {
            var page = PPRPApp.Pages.Thailand;
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
