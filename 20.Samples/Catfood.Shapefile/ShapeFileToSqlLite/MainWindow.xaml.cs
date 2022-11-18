#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ShapeFileToSqlLite.ShapeFiles;
using ShapeFileToSqlLite.Models;
using ShapeFileToSqlLite.Services;

#endregion

namespace ShapeFileToSqlLite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Class
        private class MapFiles : ModelBase
        {
            private string _ShapeFilePath = string.Empty;

            public MapFiles()
            {
                ShapeFilePath = @".\shapefiles";
            }
            public string ShapeFilePath
            {
                get { return _ShapeFilePath; }
                set 
                {
                    if (_ShapeFilePath != value)
                    {
                        _ShapeFilePath = value;
                        RaiseChanged("ShapeFilePath");
                        RaiseChanged("ADM0ShapeFile");
                        RaiseChanged("ADM1ShapeFile");
                        RaiseChanged("ADM2ShapeFile");
                    }
                }
            }

            public string ADM0ShapeFile
            {
                get { return System.IO.Path.Combine(ShapeFilePath, "thailand_adm0.shp"); }
                set { }
            }
            public string ADM1ShapeFile
            {
                get { return System.IO.Path.Combine(ShapeFilePath, "thailand_adm1.shp"); }
                set { }
            }
            public string ADM2ShapeFile
            {
                get { return System.IO.Path.Combine(ShapeFilePath, "thailand_adm2.shp"); }
                set { }
            }
            public string ADM3ShapeFile
            {
                get { return System.IO.Path.Combine(ShapeFilePath, "thailand_adm3.shp"); }
                set { }
            }
        }

        #endregion

        #region Internal Variables

        private MapFiles _mapFile = new MapFiles();

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScanFiles();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        #endregion

        #region Button Handlers

        private void cmdImportADM0_Click(object sender, RoutedEventArgs e)
        {
            ImportADM0();
        }

        private void cmdImportADM1_Click(object sender, RoutedEventArgs e)
        {
            ImportADM1();
        }

        private void cmdImportADM2_Click(object sender, RoutedEventArgs e)
        {
            ImportADM2();
        }

        private void cmdImportADM3_Click(object sender, RoutedEventArgs e)
        {
            ImportADM3();
        }

        #endregion

        #region Private Methods

        private void ScanFiles()
        {
            if (null == _mapFile) return;
            txtADM0FileName.Text = _mapFile.ADM0ShapeFile;
            cmdImportADM0.IsEnabled = File.Exists(_mapFile.ADM0ShapeFile) ? true : false;
            txtADM1FileName.Text = _mapFile.ADM1ShapeFile;
            cmdImportADM1.IsEnabled = File.Exists(_mapFile.ADM1ShapeFile) ? true : false;
            txtADM2FileName.Text = _mapFile.ADM2ShapeFile;
            cmdImportADM2.IsEnabled = File.Exists(_mapFile.ADM2ShapeFile) ? true : false;
            txtADM3FileName.Text = _mapFile.ADM3ShapeFile;
            cmdImportADM3.IsEnabled = File.Exists(_mapFile.ADM3ShapeFile) ? true : false;
        }

        private void ImportADM0()
        {
            if (null == _mapFile) return;
            var fileName = _mapFile.ADM0ShapeFile;
            if (!File.Exists(fileName)) return;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() => { cmdImportADM0.IsEnabled = false; });
                using (Shapefile shapefile = new Shapefile(fileName))
                {
                    ShapeFileDbImport.Import(shapefile);
                }
                Dispatcher.Invoke(() =>  { cmdImportADM0.IsEnabled = true; });
            });
        }

        private void ImportADM1()
        {
            if (null == _mapFile) return;
            var fileName = _mapFile.ADM1ShapeFile;
            if (!File.Exists(fileName)) return;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() => { cmdImportADM1.IsEnabled = false; });
                using (Shapefile shapefile = new Shapefile(fileName))
                {
                    ShapeFileDbImport.Import(shapefile);
                }
                Dispatcher.Invoke(() => { cmdImportADM1.IsEnabled = true; });
            });
        }

        private void ImportADM2()
        {
            if (null == _mapFile) return;
            var fileName = _mapFile.ADM2ShapeFile;
            if (!File.Exists(fileName)) return;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() => { cmdImportADM2.IsEnabled = false; });
                using (Shapefile shapefile = new Shapefile(fileName))
                {
                    ShapeFileDbImport.Import(shapefile);
                }
                Dispatcher.Invoke(() => { cmdImportADM2.IsEnabled = true; });
            });
        }

        private void ImportADM3()
        {
            if (null == _mapFile) return;
            var fileName = _mapFile.ADM3ShapeFile;
            if (!File.Exists(fileName)) return;

            Task.Run(() =>
            {
                Dispatcher.Invoke(() => { cmdImportADM3.IsEnabled = false; });
                using (Shapefile shapefile = new Shapefile(fileName))
                {
                    ShapeFileDbImport.Import(shapefile);
                }
                Dispatcher.Invoke(() => { cmdImportADM3.IsEnabled = true; });
            });
        }

        #endregion
    }
}
