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

            #region PollingStation

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

            #region MPD2562VoteSummary

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

            #region MPD2562VoteSummary

            private static MPD2562x350UnitSummaryManagePage _MPD2562x350UnitSummaryManage;

            /// <summary>Gets MPDx350Unit 2562 Summary Manage Page.</summary>
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

            #region PersonImage

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

            #region ExcelImportSample

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

            #region ImportPartyImage

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

            #region ImportPartyImage

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

            #region ImportMPD2562VoteSummary

            /// <summary>Gets MPD 2562 Vote Summary Import Window.</summary>
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

            #region ImportMPD2562x350UnitSummary

            /// <summary>Gets MPD 2562x350Unit Summary Import Window.</summary>
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

            #region ImportMPDC2566

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

            #region ImportPersonImage

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
