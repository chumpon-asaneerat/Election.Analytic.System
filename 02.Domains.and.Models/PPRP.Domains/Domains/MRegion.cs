#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

using NLib;
using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    public class MRegion
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MRegion() : base()
        {

        }

        #endregion

        #region Public Properties

        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string GeoGroup { get; set; }
        public string GeoSubGroup { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MRegion>> Gets(string regionId = null, 
            string regionName = null, string geoGroup = null, string geoSubGroup = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MRegion>> rets = new NDbResult<List<MRegion>>();

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

            var p = new DynamicParameters();
            p.Add("@RegionId", regionId);
            p.Add("@RegionName", regionName);
            p.Add("@GeoGroup", geoGroup);
            p.Add("@GeoSubGroup", geoSubGroup);

            try
            {
                rets.Value = cnn.Query<MRegion>("GetMRegions", p,
                    commandType: CommandType.StoredProcedure).AsList();
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
                rets.Value = new List<MRegion>();
            }

            return rets;
        }

        #endregion
    }
}
