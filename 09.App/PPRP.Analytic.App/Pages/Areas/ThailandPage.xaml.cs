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
    /// Interaction logic for ThailandPage.xaml
    /// </summary>
    public partial class ThailandPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ThailandPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private List<PakMenuItem> _regions = null;

        #endregion

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Button Handlers

        private void cmdPak_Click(object sender, RoutedEventArgs e)
        {
            var pak = (sender as Button).DataContext as PakMenuItem;
            if (null != pak)
            {
                if (pak.RegionId == "01")
                {
                    var page = PPRPApp.Pages.Pak01;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "02")
                {
                    var page = PPRPApp.Pages.Pak02;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "03")
                {
                    var page = PPRPApp.Pages.Pak03;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "04")
                {
                    var page = PPRPApp.Pages.Pak04;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "05")
                {
                    var page = PPRPApp.Pages.Pak05;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "06")
                {
                    var page = PPRPApp.Pages.Pak06;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "07")
                {
                    var page = PPRPApp.Pages.Pak07;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "08")
                {
                    var page = PPRPApp.Pages.Pak08;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "09")
                {
                    var page = PPRPApp.Pages.Pak09;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "10")
                {
                    var page = PPRPApp.Pages.Pak10;
                    page.Setup(pak.RegionId);
                    PageContentManager.Instance.Current = page;
                }
            }
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            _regions = PakMenuItem.Gets().Value;
            if (null != _regions)
            {
                Console.WriteLine("No of region : {0}", _regions.Count);
            }
            lstPaks.ItemsSource = _regions;
        }

        #endregion
    }
}
