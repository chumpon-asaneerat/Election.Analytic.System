#region Using

using System;
using System.Collections.Generic;
using System.Linq;
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

#endregion

namespace Wpf.Canvas.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private NWpfCanvasManager manager = null;

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitCanvasManager();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            manager = null;
        }

        #endregion

        #region Private Methods

        private void InitCanvasManager()
        {
            manager = new NWpfCanvasManager(this.canvas);
        }

        #endregion
    }
}
