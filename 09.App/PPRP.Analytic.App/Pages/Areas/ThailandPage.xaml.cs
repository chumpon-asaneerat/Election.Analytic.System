﻿#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Reflection;

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

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPak(PakMenuItem pak)
        {
            AreaNavi.Instance.Current = pak; // set current.
            if (null != pak)
            {
                if (pak.RegionId == "01")
                {
                    var page = PPRPApp.Pages.Pak01;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "02")
                {
                    var page = PPRPApp.Pages.Pak02;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "03")
                {
                    var page = PPRPApp.Pages.Pak03;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "04")
                {
                    var page = PPRPApp.Pages.Pak04;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "05")
                {
                    var page = PPRPApp.Pages.Pak05;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "06")
                {
                    var page = PPRPApp.Pages.Pak06;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "07")
                {
                    var page = PPRPApp.Pages.Pak07;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "08")
                {
                    var page = PPRPApp.Pages.Pak08;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "09")
                {
                    var page = PPRPApp.Pages.Pak09;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (pak.RegionId == "10")
                {
                    var page = PPRPApp.Pages.Pak10;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
            }
        }

        private void GotoProvince(ProvinceMenuItem province)
        {
            if (null == province)
                return;
            var page = PPRPApp.Pages.MPD2562VoteSummary;
            page.Setup(province);
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Button Handlers

        private void CmdPakProvinceClickCallback(AreaMenuItem item)
        {
            if (item.ItemType == AreaMenuItem.PAK)
            {
                GotoPak(item as PakMenuItem);
            }
            else
            {
                GotoProvince(item as ProvinceMenuItem);
            }
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null != AreaNavi.Instance.Regions)
            {
                med.Info("No of Regions : {0}", AreaNavi.Instance.Regions.Count);
            }
            else
            {
                med.Info("Regions is null or Count : 0");
            }

            var menuItems = new List<AreaMenuItem>();
            var regions = AreaNavi.Instance.Regions;
            if (null != regions)
            {
                foreach (var pak in regions)
                {
                    if (null == pak) continue;
                    // add Pak
                    menuItems.Add(pak);
                }
            }

            navPaks.Setup(menuItems, this.CmdPakProvinceClickCallback);
        }

        #endregion
    }
}
