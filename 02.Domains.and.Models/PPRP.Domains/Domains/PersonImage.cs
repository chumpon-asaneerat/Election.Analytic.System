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
    public class PersonImage
    {
        private ImageSource _img = null;

        public string FullName { get; set; }
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


        public static NDbResult<List<PersonImage>> Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<PersonImage>> rets = new NDbResult<List<PersonImage>>();

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
                query += "SELECT TOP 100 * ";
                query += " FROM PersonImage ";

                rets.Value = cnn.Query<PersonImage>(query).ToList();
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
                rets.Value = new List<PersonImage>();
            }

            return rets;
        }

        public static void ImportPersonImage(string fullName, byte[] data)
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
            p.Add("@FullName", fullName);
            p.Add("@Data", data, dbType: DbType.Binary, direction: ParameterDirection.Input, size: -1);

            try
            {
                cnn.Execute("SavePersonImage", p, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return;
        }
    }
}
