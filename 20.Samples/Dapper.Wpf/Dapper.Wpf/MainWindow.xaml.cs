#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using NLib.Data;
using NLib.Components;

using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using System.Windows.Media.Imaging;

using Newtonsoft.Json;

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

        #region Tab - General

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

        #region Tab - Image

        private void cmdChooseImage_Click(object sender, RoutedEventArgs e)
        {
            string fileName = ShowImageDialog(this);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            lbImageFileName.Text = fileName;

            byte[] buffers = GetFileBuffer(fileName);
            ImageSource imgSrc = GetImageSource(buffers);
            img.Source = imgSrc;
        }

        private void cmdChooseJson_Click(object sender, RoutedEventArgs e)
        {
            string fileName = ShowJsonDialog(this);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            lbJsonFileName.Text = fileName;

            byte[] buffers = GetFileBuffer(fileName);
            string json = System.Text.Encoding.UTF8.GetString(buffers);

            // formatting json
            dynamic dJson = JsonConvert.DeserializeObject(json);
            string fJson = JsonConvert.SerializeObject(dJson, Formatting.Indented);

            if (MessageBox.Show("Show in Textbox?", "Confirm show large file?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                txtJson.Clear();
                using (txtJson.Dispatcher.DisableProcessing())
                {
                    txtJson.AppendText(fJson);
                }
            }
        }

        private void cmdSaveImageToDb_Click(object sender, RoutedEventArgs e)
        {
            string fileName = lbImageFileName.Text;
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            if (null == connection || !connection.IsConnected)
                return;

            var conn = connection.DbConnection;

            byte[] buffers = GetFileBuffer(fileName);

            var inst = new MContent();
            inst.Data = buffers;
            inst.FileTypeId = MContent.FileTypes.Image;
            inst.FileSubTypeId = MContent.FileSubTypes.PersonOrJson;

            MContent.Save(conn, inst);
            txtImageContentId.Text = inst.ContentId.ToString();
        }

        private void cmdLoadImageFromDb_Click(object sender, RoutedEventArgs e)
        {
            string contentId = txtImageContentId.Text;
            if (string.IsNullOrWhiteSpace(contentId))
                return;
        }

        private void cmdSaveJsonToDb_Click(object sender, RoutedEventArgs e)
        {
            string fileName = lbJsonFileName.Text;
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            if (null == connection || !connection.IsConnected)
                return;

            var conn = connection.DbConnection;

            byte[] buffers = GetFileBuffer(fileName);

            var inst = new MContent();
            inst.Data = buffers;
            inst.FileTypeId = MContent.FileTypes.Data;
            inst.FileSubTypeId = MContent.FileSubTypes.PersonOrJson;

            MContent.Save(conn, inst);

            txtJsonContentId.Text = inst.ContentId.ToString();
        }

        private void cmdLoadJsonFromDb_Click(object sender, RoutedEventArgs e)
        {
            string contentId = txtJsonContentId.Text;
            if (string.IsNullOrWhiteSpace(contentId))
                return;
        }

        #endregion

        public string ShowImageDialog(Window owner,
            string title = "กรุณาเลือก image file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null)
        {
            string filename = string.Empty;

            // setup dialog options
            var od = new Microsoft.Win32.OpenFileDialog();
            od.Multiselect = false;
            od.InitialDirectory = initDir;
            od.Title = string.IsNullOrEmpty(title) ? "กรุณาเลือก image file ที่ต้องการนำเข้าข้อมูล" : title;
            od.Filter = "Images Files(*.jpg, *.png)|*.jpg;*.png";

            bool ret = od.ShowDialog(owner) == true;
            if (ret)
            {
                // assigned to FileName
                filename = od.FileName;
            }
            od = null;

            return filename;
        }

        public string ShowJsonDialog(Window owner,
            string title = "กรุณาเลือก json file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null)
        {
            string filename = string.Empty;

            // setup dialog options
            var od = new Microsoft.Win32.OpenFileDialog();
            od.Multiselect = false;
            od.InitialDirectory = initDir;
            od.Title = string.IsNullOrEmpty(title) ? "กรุณาเลือก json file ที่ต้องการนำเข้าข้อมูล" : title;
            od.Filter = "Json Files(*.json)|*.json";

            bool ret = od.ShowDialog(owner) == true;
            if (ret)
            {
                // assigned to FileName
                filename = od.FileName;
            }
            od = null;

            return filename;
        }


        public byte[] GetFileBuffer(string fileName)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

            byte[] buffers;

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    buffers = reader.ReadBytes((int)stream.Length);
                }
            }
            return buffers;
        }

        public ImageSource GetImageSource(byte[] buffers)
        {
            try
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

                using (Stream stream = new MemoryStream(buffers))
                {
                    BitmapImage image = new BitmapImage();
                    stream.Position = 0;
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    image.Freeze();
                    return image;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        #region Public Properties

        public int TitleId { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public string GenderName { get; set; }
        public int GenderId { get; set; }
        public int DLen { get; set; }
        public int SLen { get; set; }

        public int ErrNum { get; set; }
        public string ErrMsg { get; set; }

        #endregion

        #region Static Methods

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

        #endregion
    }

    public class MContent
    {
        public enum FileTypes : int 
        { 
            Image = 1,
            Data = 2
        }

        public enum FileSubTypes : int
        {
            PersonOrJson = 1,
            Logo = 2
        }

        #region Public Properties

        public Guid? ContentId { get; set; }
        public FileTypes FileTypeId { get; set; }
        public FileSubTypes FileSubTypeId { get; set; }

        public byte[] Data { get; set; }

        public int ErrNum { get; set; }
        public string ErrMsg { get; set; }

        #endregion

        #region Static Methods

        public static void Save(IDbConnection cnn, MContent value)
        {
            var p = new DynamicParameters();
            p.Add("@Data", value.Data, dbType: DbType.Binary, direction: ParameterDirection.Input);
            p.Add("@FileTypeId", value.FileTypeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@FileSubTypeId", value.FileSubTypeId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            p.Add("@ContentId", dbType: DbType.Guid, direction: ParameterDirection.InputOutput);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
            //p.Add("@RET", dbType: DbType.String, direction: ParameterDirection.ReturnValue);


            cnn.Execute("SaveMContent", p, commandType: CommandType.StoredProcedure);

            value.ContentId = p.Get<Guid>("@ContentId");
            int errNum = p.Get<int>("@errNum");
            string errMsg = p.Get<string>("@errMsg");
            //int ret = p.Get<int>("@RET");
            value.ErrNum = errNum;
            value.ErrMsg = errMsg;
        }

        #endregion
    }
}
