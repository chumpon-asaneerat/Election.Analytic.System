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
    /// Interaction logic for Pak04Page.xaml
    /// </summary>
    public partial class Pak04Page : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pak04Page()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<ProvinceMenuItem> _provinces = null;

        #endregion

        #region Button Handlers

        private void cmdGotoThailandPage_Click(object sender, RoutedEventArgs e)
        {
            GotoThailandPage();
        }

        private void cmdProvince_Click(object sender, RoutedEventArgs e)
        {

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

        public void Setup(string regiondId)
        {
            _provinces = ProvinceMenuItem.Gets(regiondId).Value;
            if (null != _provinces)
            {
                Console.WriteLine("No of region : {0}", _provinces.Count);
            }
            lstProvinces.ItemsSource = _provinces;
        }

        #endregion
    }
}
