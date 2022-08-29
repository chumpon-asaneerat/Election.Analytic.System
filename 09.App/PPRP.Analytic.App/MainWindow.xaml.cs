#region Using

using System;
using System.Collections.Generic;
using System.Windows;

using NLib.Services;

#endregion

namespace PPRP
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

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start Server
            DbServer.Instance.Start();

            #region Test Connection
            /*
            var rets = Domains.MTitle.Gets(DbServer.Instance.Db);
            if (null != rets)
            {
                Console.WriteLine("Count: {0}", rets.Value.Count);
            }
            */
            #endregion

            // Initial Page Content Manager
            PageContentManager.Instance.ContentChanged += new EventHandler(Instance_ContentChanged);
            PageContentManager.Instance.Start();

            // Sign In.
            var page = PPRPApp.Pages.SignIn;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            // Release Page Content Manager
            PageContentManager.Instance.Shutdown();
            PageContentManager.Instance.ContentChanged -= new EventHandler(Instance_ContentChanged);

            // Shutdown Server
            DbServer.Instance.Shutdown();
        }

        #endregion

        #region Page Content Manager Handlers

        void Instance_ContentChanged(object sender, EventArgs e)
        {
            this.container.Content = PageContentManager.Instance.Current;
        }

        #endregion
    }
}
