#region Using

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

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for MPDC2562AreaRemarkWindow.xaml
    /// </summary>
    public partial class MPDC2562AreaRemarkWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2562AreaRemarkWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPD2562PollingUnitSummary _item = null;

        #endregion

        #region Public Methods

        public void Setup(MPD2562PollingUnitSummary value)
        {
            _item = value;
            this.DataContext = _item;
            if (null == _item)
            {
                txtTitle.Text = string.Format("ข้อมูลพื้นที่ {0} เขต {1}", "-", "-");
            }
            else
            {
                txtTitle.Text = string.Format("ข้อมูลพื้นที่ {0} เขต {1}", _item.ProvinceName, _item.PollingUnitNo);
            }
            this.Title = txtTitle.Text;
        }

        #endregion
    }
}
