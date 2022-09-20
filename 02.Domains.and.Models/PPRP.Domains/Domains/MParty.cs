#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

using System.Windows.Media;

using NLib;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    public class MParty
    {
        #region Internal Variables

        private bool _ImageLoading = false;
        private ImageSource _img = null;

        #endregion

        #region Public Properties

        public int PartyId { get; set; }

        public string PartyName { get; set; }
        public Guid ContendId { get; set; }
        public byte[] Data { get; set; }

        public ImageSource Image 
        {
            get 
            {
                _img = ByteUtils.GetImageSource(Data);
                return _img;
            }
            set { }
        }

        #endregion

        #region Static Methods

        public static NDbResult<List<MParty>> Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MParty>> rets = new NDbResult<List<MParty>>();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                // Set error number/message
                rets.ErrNum = 8000;
                rets.ErrMsg = msg;

                return rets;
            }

            try
            {
                string query = string.Empty;
                query += "SELECT TOP 100 A.*, B.Data ";
                query += " FROM MParty A, MContent B ";
                query += "WHERE A.ContentId = B.ContentId";

                rets.Value = cnn.Query<MParty>(query).ToList();
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                rets.ErrNum = 9999;
                rets.ErrMsg = ex.Message;
            }

            if (null == rets.Value)
            {
                // create empty list.
                rets.Value = new List<MParty>();
            }

            return rets;
        }

        public static void ImportPartyImage(string partyName, byte[] data)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);

                return;
            }

            var p = new DynamicParameters();
            p.Add("@partyName", partyName);
            p.Add("@Data", data, dbType: DbType.Binary, direction: ParameterDirection.Input, size: -1);

            try
            {
                cnn.Execute("ImportPartyImage", p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return;
        }

        #endregion
    }
}
