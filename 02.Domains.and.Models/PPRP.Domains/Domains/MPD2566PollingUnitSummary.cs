﻿#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using System.Windows.Media;

using NLib;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    public class MPD2566PollingUnitSummary
    {
        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int PollingUnitCount { get; set; }
        public string AreaRemark { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2566PollingUnitSummary>> Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPD2566PollingUnitSummary>> rets = new NDbResult<List<MPD2566PollingUnitSummary>>();

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
                query += @"
                    SELECT * 
                      FROM MPD2566PollingUnitSummary
                ";

                rets.Value = cnn.Query<MPD2566PollingUnitSummary>(query).ToList();
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
                rets.Value = new List<MPD2566PollingUnitSummary>();
            }

            return rets;
        }

        public static NDbResult<MPD2566PollingUnitSummary> Get(string provinceName, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<MPD2566PollingUnitSummary> ret = new NDbResult<MPD2566PollingUnitSummary>();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                // Set error number/message
                ret.ErrNum = 8000;
                ret.ErrMsg = msg;

                return ret;
            }

            try
            {
                string query = string.Empty;
                query += @"
                    SELECT * 
                      FROM MPD2566PollingUnitSummary
                     WHERE ProvinceName = @ProvinceName
                       AND PollingUnitNo = @PollingUnitNo
                ";

                ret.Value = cnn.Query<MPD2566PollingUnitSummary>(query,
                    new { provinceName, pollingUnitNo }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                ret.ErrNum = 9999;
                ret.ErrMsg = ex.Message;
            }

            return ret;
        }

        public static void Save(MPD2566PollingUnitSummary value)
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
            p.Add("@ProvinceName", value.ProvinceName);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@PollingUnitCount", value.PollingUnitCount);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMPD2566PollingUnitSummary", p, commandType: CommandType.StoredProcedure);

                // Set error number/message
                int errNum = p.Get<int>("@errNum");
                string errMsg = p.Get<string>("@errMsg");
                if (errNum != 0)
                {
                    Console.WriteLine(errMsg);
                }
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