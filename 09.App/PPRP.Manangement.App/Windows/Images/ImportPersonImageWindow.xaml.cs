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
    /// Interaction logic for ImportPersonImageWindow.xaml
    /// </summary>
    public partial class ImportPersonImageWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportPersonImageWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region PartyImageFile Class

        public class PersonImageFile : NInpc
        {
            private byte[] _data = null;
            private ImageSource _imgSrc = null;

            public string FullFileName
            {
                get;
                set;
            }
            public string PersonName { get; set; }

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

        private List<PersonImageFile> _persons = null;

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

                _persons = new List<PersonImageFile>();
                if (null != files && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var inst = new PersonImageFile();

                        try
                        {
                            inst.FullFileName = file;
                            inst.PersonName = Path.GetFileNameWithoutExtension(file);
                            inst.Data = ByteUtils.GetFileBuffer(file);

                            _persons.Add(inst);
                        }
                        catch { }
                    }

                    lvFiles.ItemsSource = _persons;
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
            if (null != _persons)
            {
                foreach (var person in _persons)
                {
                    PersonImage.ImportPersonImage(person.PersonName, person.Data);
                }
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
