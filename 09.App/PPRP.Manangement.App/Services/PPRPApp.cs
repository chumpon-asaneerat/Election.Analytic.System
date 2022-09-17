#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PPRP.Pages;

#endregion

namespace PPRP
{
    /// <summary>
    /// The PPRPApp class.
    /// </summary>
    public static class PPRPApp
    {
        /// <summary>
        /// Variables Static class.
        /// </summary>
        public static class Variables
        {
            /*
            /// <summary>Chief Revenue Entry Prerender DateTime.</summary>
            public static DateTime ChiefRevenueLastRenderTime = DateTime.MinValue;
            /// <summary>Collector Revenue Entry Prerender DateTime.</summary>
            public static DateTime CollectorRevenueLastRenderTime = DateTime.MinValue;
            */
        }
        /// <summary>
        /// Permissions Static class.
        /// </summary>
        public static class Permissions
        {
        }
        /// <summary>
        /// Pages Static class.
        /// </summary>
        public static class Pages
        {
            #region Main Menu

            private static MainMenuPage _MainMenu;

            /// <summary>Gets Main Menu Page.</summary>
            public static MainMenuPage MainMenu
            {
                get
                {
                    if (null == _MainMenu)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MainMenu = new MainMenuPage();
                        }
                    }
                    return _MainMenu;
                }
            }

            #endregion

            #region SignIn

            private static SignInPage _SignIn;

            /// <summary>Gets Sign In Page.</summary>
            public static SignInPage SignIn
            {
                get
                {
                    if (null == _SignIn)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _SignIn = new SignInPage();
                        }
                    }
                    return _SignIn;
                }
            }

            #endregion

            #region Party

            private static PartyManagePage _PartyManage;

            /// <summary>Gets Party Manage Page.</summary>
            public static PartyManagePage PartyManage
            {
                get
                {
                    if (null == _PartyManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _PartyManage = new PartyManagePage();
                        }
                    }
                    return _PartyManage;
                }
            }

            #endregion

            #region Polling Station

            private static PollingStationManagePage _PollingStationManage;

            /// <summary>Gets Polling Station Manage Page.</summary>
            public static PollingStationManagePage PollingStationManage
            {
                get
                {
                    if (null == _PollingStationManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _PollingStationManage = new PollingStationManagePage();
                        }
                    }
                    return _PollingStationManage;
                }
            }

            #endregion

            #region MProvince

            private static MProvinceManagePage _MProvinceManage;

            /// <summary>Gets MProvince Manage Page.</summary>
            public static MProvinceManagePage MProvinceManage
            {
                get
                {
                    if (null == _MProvinceManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MProvinceManage = new MProvinceManagePage();
                        }
                    }
                    return _MProvinceManage;
                }
            }

            #endregion

            #region MDistrict

            private static MDistrictManagePage _MDistrictManage;

            /// <summary>Gets MDistrict Manage Page.</summary>
            public static MDistrictManagePage MDistrictManage
            {
                get
                {
                    if (null == _MDistrictManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MDistrictManage = new MDistrictManagePage();
                        }
                    }
                    return _MDistrictManage;
                }
            }

            #endregion

            #region MSubdistrict

            private static MSubdistrictManagePage _MSubdistrictManage;

            /// <summary>Gets MSubdistrict Manage Page.</summary>
            public static MSubdistrictManagePage MSubdistrictManage
            {
                get
                {
                    if (null == _MSubdistrictManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MSubdistrictManage = new MSubdistrictManagePage();
                        }
                    }
                    return _MSubdistrictManage;
                }
            }

            #endregion

            #region MPD2562 Vote Summary

            private static MPD2562VoteSummaryManagePage _MPD2562VoteSummaryManage;

            /// <summary>Gets MPD 2562 VoteSummary Manage Page.</summary>
            public static MPD2562VoteSummaryManagePage MPD2562VoteSummaryManage
            {
                get
                {
                    if (null == _MPD2562VoteSummaryManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562VoteSummaryManage = new MPD2562VoteSummaryManagePage();
                        }
                    }
                    return _MPD2562VoteSummaryManage;
                }
            }

            #endregion

            #region MPDC2566

            private static MPDC2566ManagePage _MPDC2566Manage;

            /// <summary>Gets MPDC 2566 Manage Page.</summary>
            public static MPDC2566ManagePage MPDC2566Manage
            {
                get
                {
                    if (null == _MPDC2566Manage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPDC2566Manage = new MPDC2566ManagePage();
                        }
                    }
                    return _MPDC2566Manage;
                }
            }

            #endregion

            #region MPD2562 350 Unit Summary

            private static MPD2562x350UnitSummaryManagePage _MPD2562x350UnitSummaryManage;

            /// <summary>Gets MPD2562 350 Unit Summary Manage Page.</summary>
            public static MPD2562x350UnitSummaryManagePage MPD2562x350UnitSummaryManage
            {
                get
                {
                    if (null == _MPD2562x350UnitSummaryManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562x350UnitSummaryManage = new MPD2562x350UnitSummaryManagePage();
                        }
                    }
                    return _MPD2562x350UnitSummaryManage;
                }
            }

            #endregion

            #region MPD2562 Polling Unit Summary

            private static MPD2562PollingUnitSummaryManagePage _MPD2562PollingUnitSummaryManage;

            /// <summary>Gets MPD2562 Polling Unit Summary Manage Page.</summary>
            public static MPD2562PollingUnitSummaryManagePage MPD2562PollingUnitSummaryManage
            {
                get
                {
                    if (null == _MPD2562PollingUnitSummaryManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562PollingUnitSummaryManage = new MPD2562PollingUnitSummaryManagePage();
                        }
                    }
                    return _MPD2562PollingUnitSummaryManage;
                }
            }

            #endregion

            #region MPD2562 Area Remark Summary

            private static MPD2562AreaRemarkManagePage _MPD2562AreaRemarkManage;

            /// <summary>Gets MPD2562 Area Remark Summary Manage Page.</summary>
            public static MPD2562AreaRemarkManagePage MPD2562AreaRemarkManage
            {
                get
                {
                    if (null == _MPD2562AreaRemarkManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562AreaRemarkManage = new MPD2562AreaRemarkManagePage();
                        }
                    }
                    return _MPD2562AreaRemarkManage;
                }
            }

            #endregion

            #region MPD2566 Polling Unit Summary

            private static MPD2566PollingUnitSummaryManagePage _MPD2566PollingUnitSummaryManage;

            /// <summary>Gets MPD2566 Polling Unit Summary Manage Page.</summary>
            public static MPD2566PollingUnitSummaryManagePage MPD2566PollingUnitSummaryManage
            {
                get
                {
                    if (null == _MPD2566PollingUnitSummaryManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2566PollingUnitSummaryManage = new MPD2566PollingUnitSummaryManagePage();
                        }
                    }
                    return _MPD2566PollingUnitSummaryManage;
                }
            }

            #endregion

            #region Person Image

            private static PersonImageManagePage _PersonImageManage;

            /// <summary>Gets Party Manage Page.</summary>
            public static PersonImageManagePage PersonImageManage
            {
                get
                {
                    if (null == _PersonImageManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _PersonImageManage = new PersonImageManagePage();
                        }
                    }
                    return _PersonImageManage;
                }
            }

            #endregion

            #region Excel Import Sample

            private static ExcelImportSamplePage _ExcelSample;

            /// <summary>Gets Excel Import Sample Page.</summary>
            public static ExcelImportSamplePage ExcelSample
            {
                get
                {
                    if (null == _ExcelSample)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _ExcelSample = new ExcelImportSamplePage();
                        }
                    }
                    return _ExcelSample;
                }
            }

            #endregion
        }
        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {
            #region Application Main Window

            /// <summary>Gets Application Main Window.</summary>
            public static Window MainWindow { get { return Application.Current.MainWindow; } }

            #endregion

            #region Common Progress Dialog

            /// <summary>Gets Common Progress Dialog Window.</summary>
            public static PPRP.Windows.ProgressWindow ProgressDialog
            {
                get
                {
                    var ret = new PPRP.Windows.ProgressWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Party Image

            /// <summary>Gets Import Party Image Window.</summary>
            public static PPRP.Windows.ImportPartyImageWindow ImportPartyImage
            {
                get
                {
                    var ret = new PPRP.Windows.ImportPartyImageWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Polling Station

            /// <summary>Gets Polling Station Import Window.</summary>
            public static PPRP.Windows.ImportPollingStationWindow ImportPollingStation
            {
                get
                {
                    var ret = new PPRP.Windows.ImportPollingStationWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MProvince

            /// <summary>Gets MProvince Import Window.</summary>
            public static PPRP.Windows.ImportMProvinceWindow ImportMProvince
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMProvinceWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MDistrict

            /// <summary>Gets MDistrict Import Window.</summary>
            public static PPRP.Windows.ImportMDistrictWindow ImportMDistrict
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMDistrictWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MSubdistrict

            /// <summary>Gets MSubdistrict Import Window.</summary>
            public static PPRP.Windows.ImportMSubdistrictWindow ImportMSubdistrict
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMSubdistrictWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPD2562 Vote Summary

            /// <summary>Gets MPD2562 Vote Summary Import Window.</summary>
            public static PPRP.Windows.ImportMPD2562VoteSummaryWindow ImportMPD2562VoteSummary
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2562VoteSummaryWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPD2562 x350 Unit Summary

            /// <summary>Gets MPD2562 350 Unit Summary Import Window.</summary>
            public static PPRP.Windows.ImportMPD2562x350UnitSummaryWindow ImportMPD2562x350UnitSummary
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2562x350UnitSummaryWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPD2562 Polling Unit Summary

            /// <summary>Gets MPD2562 Polling Unit Summary Import Window.</summary>
            public static PPRP.Windows.ImportMPD2562PollingUnitSummaryWindow ImportMPD2562PollingUnitSummary
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2562PollingUnitSummaryWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPD2562 Area Remark Summary

            /// <summary>Gets MPD2562 Area Remark Import Window.</summary>
            public static PPRP.Windows.ImportMPD2562AreaRemarkSummaryWindow ImportMPD2562AreaRemarkSummary
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2562AreaRemarkSummaryWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPDC2566

            /// <summary>Gets MPDC 2566 Import Window.</summary>
            public static PPRP.Windows.ImportMPDC2566Window ImportMPDC2566
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPDC2566Window();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Person Image

            /// <summary>Gets Person Image Import Window.</summary>
            public static PPRP.Windows.ImportPersonImageWindow ImportPersonImage
            {
                get
                {
                    var ret = new PPRP.Windows.ImportPersonImageWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion
        }
    }
}
