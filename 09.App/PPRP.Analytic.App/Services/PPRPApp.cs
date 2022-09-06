﻿#region Using

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

            #region Thailand, Pak Pages

            private static ThailandPage _ThailandPage;

            /// <summary>Gets Thailand Page.</summary>
            public static ThailandPage Thailand
            {
                get
                {
                    if (null == _ThailandPage)
                    {
                        lock (typeof(ThailandPage))
                        {
                            _ThailandPage = new ThailandPage();
                        }
                    }
                    return _ThailandPage;
                }
            }

            private static Pak01Page _Pak01Page;

            /// <summary>Gets Pak01 Page.</summary>
            public static Pak01Page Pak01
            {
                get
                {
                    if (null == _Pak01Page)
                    {
                        lock (typeof(Pak01Page))
                        {
                            _Pak01Page = new Pak01Page();
                        }
                    }
                    return _Pak01Page;
                }
            }

            private static Pak02Page _Pak02Page;

            /// <summary>Gets Pak02 Page.</summary>
            public static Pak02Page Pak02
            {
                get
                {
                    if (null == _Pak02Page)
                    {
                        lock (typeof(Pak02Page))
                        {
                            _Pak02Page = new Pak02Page();
                        }
                    }
                    return _Pak02Page;
                }
            }

            private static Pak03Page _Pak03Page;

            /// <summary>Gets Pak03 Page.</summary>
            public static Pak03Page Pak03
            {
                get
                {
                    if (null == _Pak03Page)
                    {
                        lock (typeof(Pak03Page))
                        {
                            _Pak03Page = new Pak03Page();
                        }
                    }
                    return _Pak03Page;
                }
            }

            private static Pak04Page _Pak04Page;

            /// <summary>Gets Pak04 Page.</summary>
            public static Pak04Page Pak04
            {
                get
                {
                    if (null == _Pak04Page)
                    {
                        lock (typeof(Pak04Page))
                        {
                            _Pak04Page = new Pak04Page();
                        }
                    }
                    return _Pak04Page;
                }
            }

            private static Pak05Page _Pak05Page;

            /// <summary>Gets Pak05 Page.</summary>
            public static Pak05Page Pak05
            {
                get
                {
                    if (null == _Pak05Page)
                    {
                        lock (typeof(Pak05Page))
                        {
                            _Pak05Page = new Pak05Page();
                        }
                    }
                    return _Pak05Page;
                }
            }

            private static Pak06Page _Pak06Page;

            /// <summary>Gets Pak06 Page.</summary>
            public static Pak06Page Pak06
            {
                get
                {
                    if (null == _Pak06Page)
                    {
                        lock (typeof(Pak06Page))
                        {
                            _Pak06Page = new Pak06Page();
                        }
                    }
                    return _Pak06Page;
                }
            }

            private static Pak07Page _Pak07Page;

            /// <summary>Gets Pak07 Page.</summary>
            public static Pak07Page Pak07
            {
                get
                {
                    if (null == _Pak07Page)
                    {
                        lock (typeof(Pak07Page))
                        {
                            _Pak07Page = new Pak07Page();
                        }
                    }
                    return _Pak07Page;
                }
            }

            private static Pak08Page _Pak08Page;

            /// <summary>Gets Pak08 Page.</summary>
            public static Pak08Page Pak08
            {
                get
                {
                    if (null == _Pak08Page)
                    {
                        lock (typeof(Pak08Page))
                        {
                            _Pak08Page = new Pak08Page();
                        }
                    }
                    return _Pak08Page;
                }
            }

            private static Pak09Page _Pak09Page;

            /// <summary>Gets Pak09 Page.</summary>
            public static Pak09Page Pak09
            {
                get
                {
                    if (null == _Pak09Page)
                    {
                        lock (typeof(Pak09Page))
                        {
                            _Pak09Page = new Pak09Page();
                        }
                    }
                    return _Pak09Page;
                }
            }

            private static Pak10Page _Pak10Page;

            /// <summary>Gets Pak10 Page.</summary>
            public static Pak10Page Pak10
            {
                get
                {
                    if (null == _Pak10Page)
                    {
                        lock (typeof(Pak10Page))
                        {
                            _Pak10Page = new Pak10Page();
                        }
                    }
                    return _Pak10Page;
                }
            }

            #endregion

            #region MPD2562VoteSummary


            private static MPD2562VoteSummaryPage _MPD2562VoteSummaryPage;

            /// <summary>Gets MPD2562VoteSummaryPage Page.</summary>
            public static MPD2562VoteSummaryPage MPD2562VoteSummary
            {
                get
                {
                    if (null == _MPD2562VoteSummaryPage)
                    {
                        lock (typeof(Pak10Page))
                        {
                            _MPD2562VoteSummaryPage = new MPD2562VoteSummaryPage();
                        }
                    }
                    return _MPD2562VoteSummaryPage;
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

            #region MPDC2566Preview

            /// <summary>Gets MPDC 2566 Preview Window.</summary>
            public static PPRP.Windows.MPDC2566PreviewWindow MPDC2566Preview
            {
                get
                {
                    var ret = new PPRP.Windows.MPDC2566PreviewWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion
        }
    }
}
