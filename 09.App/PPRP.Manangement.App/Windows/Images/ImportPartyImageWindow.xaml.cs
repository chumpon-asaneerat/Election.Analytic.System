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

using System.IO;
using System.Windows.Forms;

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Domains;
using PPRP.Imports.Excel;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ImportPartyImageWindow.xaml
    /// </summary>
    public partial class ImportPartyImageWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportPartyImageWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region PartyImageFile Class

        public class PartyImageFile : NInpc
        {
            private byte[] _data = null;
            private ImageSource _imgSrc = null;

            public string FullFileName 
            { 
                get; 
                set; 
            }
            public string PartyName { get; set; }

            public byte[] Data 
            { 
                get { return _data; }
                set
                {
                    _data = value;
                    Raise(() => this.Data);

                    PPRPApp.Windows.MainWindow.Dispatcher.Invoke(() =>
                    {
                        _imgSrc = ByteUtils.GetImageSource(_data);
                        Raise(() => this.Image);
                    });
                }
            }

            public ImageSource Image
            {
                get { return _imgSrc; }
                set { }
            }

        }

        #endregion

        #region Internal Variables

        private List<PartyImageFile> _parties = null;

        #endregion

        #region Button Handlers

        #region Cancel/Finish

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {
            Imports();

            DialogResult = true;
        }

        private void cmdChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            ChooseImageFolder();
        }

        #endregion

        #endregion

        #region Private Methods

        private void ChooseImageFolder()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string targetPath = string.Empty;

            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "กรูณาเลือกโฟลเดอร์รูป";
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                targetPath = fd.SelectedPath;
            }
            fd = null;

            if (string.IsNullOrEmpty(targetPath))
                return;

            try
            {
                DirectoryInfo di = new DirectoryInfo(targetPath);

                string searchPattern = "*.*";
                var exts = new string[] { "*.png", "*.jpg" };

                ICollection<string> files = di.GetFiles(searchPattern, SearchOption.AllDirectories)
                    .Where(f => f.Extension == ".gif" || f.Extension == ".jpg")
                    .Select(x => x.FullName)
                    .ToList();


                lvFiles.ItemsSource = null;

                _parties = new List<PartyImageFile>();
                if (null != files && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        try
                        {
                            var inst = new PartyImageFile();

                            inst.FullFileName = file;
                            inst.PartyName = Path.GetFileNameWithoutExtension(file);
                            inst.Data = ByteUtils.GetFileBuffer(file);

                            _parties.Add(inst);
                        }
                        catch { }
                    }

                    lvFiles.ItemsSource = _parties;
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
                System.Windows.MessageBox.Show("Error access file in folder: " + targetPath, "PPRP");
            }
        }

        private void Imports()
        {
            if (null != _parties && _parties.Count > 0)
            {
                var prog = PPRPApp.Windows.ProgressDialog;
                prog.Owner = this;
                prog.Setup(_parties.Count);
                prog.Show();

                foreach (var party in _parties)
                {
                    MParty.ImportPartyImage(party.PartyName, party.Data);
                    prog.Increment();
                }
                // Close progress dialog.
                prog.Close();
            }
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            txtFolderName.Text = string.Empty;
        }

        #endregion
    }
}
