#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Forms;

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Domains;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for MPD2562ViewWindow.xaml
    /// </summary>
    public partial class MPD2562ViewWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562ViewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPD2562VoteSummary _item;

        #endregion

        #region Public Methods

        public void Setup(MPD2562VoteSummary item)
        {
            if (null != _item)
            {
                this.DataContext = _item;
            }
        }

        #endregion
    }
}
