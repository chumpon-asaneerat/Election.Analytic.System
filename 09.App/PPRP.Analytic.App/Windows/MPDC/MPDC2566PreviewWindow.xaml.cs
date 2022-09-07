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
    /// Interaction logic for MPDC2566PreviewWindow.xaml
    /// </summary>
    public partial class MPDC2566PreviewWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566PreviewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPDC2566Summary _person = null;

        #endregion

        #region Public Methods

        public void Setup(MPDC2566Summary person)
        {
            _person = person;
            this.DataContext = _person;
            if (null == _person)
            {
                
            }
        }

        #endregion
    }
}
