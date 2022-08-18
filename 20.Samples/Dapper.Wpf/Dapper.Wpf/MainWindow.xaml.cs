#region Using

using System;
using System.Collections.Generic;
using System.Windows;

using NLib.Data;
using NLib.Components;

using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;

#endregion

namespace Dapper.Wpf
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

        private SqlServerConfig config = null;
        private NDbConnection connection = null;

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            config = new SqlServerConfig();
            config.DataSource.ServerName = "localhost";
            config.DataSource.DatabaseName = "SFDB4";
            config.Security.Authentication = AuthenticationMode.Server;
            config.Security.PersistSecurity = PersistSecurityMode.Default;
            config.Security.UserName = "sa";
            config.Security.Password = "winnt123";

            connection = new NDbConnection();
            connection.Config = config;
            bool connected = connection.Connect();
            cmdExecute1.IsEnabled = connected;

            // setup mapper
            MapPOCO.Map();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            connection.Disconnect();
            connection = null;
        }

        #endregion

        #region Button Handlers

        private void cmdExecute1_Click(object sender, RoutedEventArgs e)
        {
            dbgrid.ItemsSource = null;
            if (null == connection || !connection.IsConnected)
                return;

            var conn = connection.DbConnection;
            var orgs = conn.Query<Org>("SELECT * FROM ORG").AsList();

            dbgrid.ItemsSource = orgs;
        }

        #endregion
    }


    public class MapPOCO
    {
        public static void Map()
        {
            // setup mapper
            FluentMapper.Initialize(config => { config.AddMap(new Org.OrgMap()); });
        }
    }

    public class Org
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string OrgCode { get; set; }
        public string NameEN { get; set; }
        public string NameTH { get; set; }

        public int IgnoredProperty { get { return 1; } }

        #region Mapper class

        public class OrgMap : EntityMap<Org>
        {
            public OrgMap()
            {
                // Map property 'Name' to column 'strName' (default is case sensitive).
                Map(p => p.Id).ToColumn("OrgId", caseSensitive: false);
                Map(p => p.ParentId).ToColumn("ParentOrgId", caseSensitive: false);
                Map(p => p.NameEN).ToColumn("OrgEngName", caseSensitive: false);
                Map(p => p.NameTH).ToColumn("OrgNativeName", caseSensitive: false);
                // Ignore property when mapping.
                Map(p => p.IgnoredProperty).Ignore();
            }
        }

        #endregion
    }
}
