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
        }
    }
}
