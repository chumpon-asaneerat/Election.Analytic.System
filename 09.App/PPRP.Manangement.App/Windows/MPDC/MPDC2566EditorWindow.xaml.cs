﻿#region Using

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
        private MPDC2566 _item = null;

        #endregion

        #region Button Handlers

        private void cmdChangeImage_Click(object sender, RoutedEventArgs e)
        {
            if (null == _item)
                return;
            ChooseImageFile();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (null == _item)
                return;
            MPDC2566.Save(_item);
            DialogResult = true;
        }

        #endregion

        #region Private Methods

        private void ChooseImageFile()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string targetPath = string.Empty;

            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "กรูณาเลือกรูป";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                targetPath = fd.FileName;
            }
            fd = null;

            if (string.IsNullOrEmpty(targetPath))
                return;

            try
            {
                _item.LoadImageFile(targetPath);
            }
            catch (Exception ex)
            {
                med.Err(ex);
                System.Windows.MessageBox.Show("Error access file: " + targetPath, "PPRP");
            }
        }

        private void LoadProvinces()
        {
            cbProvinces.ItemsSource = null;
            var provinces = MProvince.Gets().Value;
            cbProvinces.ItemsSource = (null != provinces) ? provinces : new List<MProvince>();
        }

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

        public void Setup(MPDC2566 item, bool isNew = false)
        {
            LoadProvinces();
            _item = item;
            _isNew = isNew;
            if (null != _item)
            {
                this.DataContext = _item;
            }
        }

        #endregion
    }
}
