#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;
using PPRP.Imports.Excel;

#endregion

namespace PPRP.Controls
{
    /// <summary>
    /// Interaction logic for WorksheetMapListViewControl.xaml
    /// </summary>
    public partial class WorksheetMapListViewControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WorksheetMapListViewControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private NExcelImport _import = null;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void Setup(NExcelImport import)
        {
            _import = import;
        }

        public void UpdateItems(List<NExcelMapProperty> mapProperties,
            System.Collections.IEnumerable items)
        {
            // Do work while the dispatcher processing is disabled.
            using (Dispatcher.DisableProcessing())
            {
                // Clear exist data
                this.lvMapPreview.ItemsSource = null;
                // Clear exist columns
                this.lvMapGridView.Columns.Clear();

                // Build new columns
                GridViewColumn col;
                foreach (var prop in mapProperties)
                {
                    col = new GridViewColumn();
                    col.Header = prop.DisplayText;
                    col.Width = 120;
                    col.DisplayMemberBinding = new Binding(prop.PropertyName);

                    this.lvMapGridView.Columns.Add(col);
                }

                // set new ItemsSource
                this.lvMapPreview.ItemsSource = items;
            }
        }

        #endregion
    }
}
