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
    public class PakMenuItem
    {
        private List<ProvinceMenuItem> _Items = null;

        public string RegionId { get; set; }
        public string RegionName { get; set; }

        public List<ProvinceMenuItem> Provinces
        {
            get 
            {
                lock (typeof(PakMenuItem))
                {
                    if (null == _Items)
                    {
                        _Items = ProvinceMenuItem.Gets(RegionId).Value;
                    }
                }
                return _Items;
            }
            set { }
        }

        public static NDbResult<List<PakMenuItem>> Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<PakMenuItem>> rets = new NDbResult<List<PakMenuItem>>();

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
                    SELECT RegionId, RegionName
                      FROM MRegion 
                  ORDER BY RegionId
                ";

                rets.Value = cnn.Query<PakMenuItem>(query).ToList();
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
                rets.Value = new List<PakMenuItem>();
            }

            return rets;
        }
    }

    public class ProvinceMenuItem
    {
        public string RegionId { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceNameTH { get; set; }

        public static NDbResult<List<ProvinceMenuItem>> Gets(string regionId)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<ProvinceMenuItem>> rets = new NDbResult<List<ProvinceMenuItem>>();

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
                    SELECT RegionId, ProvinceId, ProvinceNameTH 
                      FROM MProvince 
                     WHERE RegionId = @RegionId 
                     ORDER BY RegionId
                ";

                rets.Value = cnn.Query<ProvinceMenuItem>(query, new { RegionId = regionId }).ToList();
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
                rets.Value = new List<ProvinceMenuItem>();
            }

            return rets;
        }
    }
}
