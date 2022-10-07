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

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Domains;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for MPDC2566EditorWindow.xaml
    /// </summary>
    public partial class MPDC2566EditorWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566EditorWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private bool _isNew = false;

        #endregion

        #region Private Methods

        private void Save()
        {
            if (_isNew)
            {
                // Add New
            }
            else
            {
                // Update/change/reorder
            }
        }

        #endregion

        #region Public Methods

        public void Setup(bool isNew = false)
        {
            _isNew = isNew;
        }

        #endregion
    }
}
