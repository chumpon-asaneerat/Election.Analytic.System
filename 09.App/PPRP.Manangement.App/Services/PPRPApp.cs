#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace PPRP
{
    public class PPRPApp
    {
        public class Pages
        {
            #region Main Menu

            private static PPRP.Pages.MainMenuPage _MainMenu;

            /// <summary>Gets Main Menu Page.</summary>
            public static PPRP.Pages.MainMenuPage MainMenu
            {
                get
                {
                    if (null == _MainMenu)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MainMenu = new PPRP.Pages.MainMenuPage();
                        }
                    }
                    return _MainMenu;
                }
            }

            #endregion
        }
    }
}
