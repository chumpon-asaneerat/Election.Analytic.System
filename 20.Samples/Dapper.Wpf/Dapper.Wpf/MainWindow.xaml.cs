#region Using

using System;
using System.Collections.Generic;

using System.Data;

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

            #region For SFDB4

            //config.DataSource.DatabaseName = "SFDB4";

            #endregion
            // PPRPDemo
            config.DataSource.DatabaseName = "PPRPDemo";

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

            #region For SFDB4

            //var orgs = conn.Query<Org>("SELECT * FROM ORG").AsList();
            //dbgrid.ItemsSource = orgs;

            #endregion
        }

        private void cmdGetMTitles_Click(object sender, RoutedEventArgs e)
        {
            dbgrid.ItemsSource = null;
            if (null == connection || !connection.IsConnected)
                return;

            var conn = connection.DbConnection;

            dbgrid.ItemsSource = MTitle.Gets(conn, txtParam1.Text, txtParam2.Text);
        }

        private void cmdSaveMTitle_Click(object sender, RoutedEventArgs e)
        {
            dbgrid.ItemsSource = null;
            if (null == connection || !connection.IsConnected)
                return;

            var conn = connection.DbConnection;

            var inst = new MTitle();
            inst.Description = "TTTTTT3";
            inst.ShortName = "T.T.T.3";
            inst.GenderId = 0;
            MTitle.Save(conn, inst);

            var items = new List<MTitle>();
            items.Add(inst);
            dbgrid.ItemsSource = items;
        }

        #endregion
    }


    public class MapPOCO
    {
        public static void Map()
        {
            // setup mapper

            #region For SFDB4

            // FluentMapper.Initialize(config => { config.AddMap(new Org.OrgMap()); });

            #endregion
        }
    }

    #region For SFDB4
    /*

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

    */
    #endregion

    public class MTitle
    {
        public int TitleId { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public string GenderName { get; set; }
        public int GenderId { get; set; }
        public int DLen { get; set; }
        public int SLen { get; set; }

        public int ErrNum { get; set; }
        public string ErrMsg { get; set; }


        public static List<MTitle> Gets(IDbConnection cnn, 
            string desc = "", string shortName = "", int? genderId = new int?())
        {
            var p = new DynamicParameters();
            p.Add("@description", desc);
            p.Add("@shortname", shortName);
            p.Add("@genderid", genderId);

            var titles = cnn.Query<MTitle>("GetMTitles", p,
                commandType: CommandType.StoredProcedure).AsList();
            return titles;
        }

        public static void Save(IDbConnection cnn, MTitle value)
        {
            var p = new DynamicParameters();
            p.Add("@Description", value.Description);
            p.Add("@ShortName", value.ShortName);
            p.Add("@GenderId", value.GenderId);
            
            p.Add("@TitleId", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            //p.Add("@RET", dbType: DbType.String, direction: ParameterDirection.ReturnValue);


            cnn.Execute("SaveMTitle", p, commandType: CommandType.StoredProcedure);

            value.TitleId = p.Get<int>("@TitleId");
            int errNum = p.Get<int>("@errNum");
            string errMsg = p.Get<string>("@errMsg");
            //int ret = p.Get<int>("@RET");
            value.ErrNum = errNum;
            value.ErrMsg = errMsg;
        }
    }

}
