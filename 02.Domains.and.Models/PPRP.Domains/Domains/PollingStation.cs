﻿#region Using

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
    public class PollingStation
    {
        #region Public Properties
        public int YearThai { get; set; }
        public string RegionName { get; set; }
        public string GeoSubGroup { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceNameTH { get; set; }
        public string DistrictId { get; set; }
        public string DistrictNameTH { get; set; }
        public string SubdistrictId { get; set; }
        public string SubdistrictNameTH { get; set; }
        public int PollingUnitNo { get; set; }
        public int PollingSubUnitNo { get; set; }
        public int VillageCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<PollingStation>> Gets(string regionName = null, string provinceNameTH = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<PollingStation>> rets = new NDbResult<List<PollingStation>>();

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

            string sRegionName = regionName;
            if (string.IsNullOrWhiteSpace(sRegionName) || sRegionName.Contains("ทุกภาค"))
            {
                sRegionName = null;
            }
            string sProvinceNameTH = provinceNameTH;
            if (string.IsNullOrWhiteSpace(sProvinceNameTH) || sProvinceNameTH.Contains("ทุกจังหวัด"))
            {
                sProvinceNameTH = null;
            }

            try
            {
                string query = string.Empty;
                query += @"
                    SELECT * 
                      FROM PollingStationView 
                     WHERE UPPER(LTRIM(RTRIM(RegionName))) = UPPER(LTRIM(RTRIM(COALESCE(@RegionName, RegionName))))
                       AND UPPER(LTRIM(RTRIM(ProvinceNameTH))) = UPPER(LTRIM(RTRIM(COALESCE(@ProvinceNameTH, ProvinceNameTH))))
                     ORDER BY RegionId 
                            , ProvinceNameTH 
                            , DistrictNameTH 
                            , SubdistrictNameTH 
                ";

                rets.Value = cnn.Query<PollingStation>(query, 
                    new { RegionName = sRegionName, ProvinceNameTH = sProvinceNameTH }).ToList();
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
                rets.Value = new List<PollingStation>();
            }

            return rets;
        }

        public static void Save(PollingStation value)
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
            p.Add("@YearThai", value.YearThai);
            p.Add("@RegionName", value.RegionName);
            p.Add("@GeoSubGroup", value.GeoSubGroup);
            p.Add("@ProvinceId", value.ProvinceId);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@DistrictId", value.DistrictId);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@SubdistrictId", value.SubdistrictId);
            p.Add("@SubdistrictNameTH", value.SubdistrictNameTH);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@PollingSubUnitNo", value.PollingSubUnitNo);
            p.Add("@VillageCount", value.VillageCount);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);


            if (value.ProvinceId == "67" &&
                value.PollingUnitNo > 1)
            {
                Console.WriteLine("detected");
            }

            try
            {
                cnn.Execute("SavePollingStation", p, commandType: CommandType.StoredProcedure);

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

        public static void Import(PollingStation value)
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
            p.Add("@YearThai", value.YearThai);
            p.Add("@RegionName", value.RegionName);
            p.Add("@GeoSubGroup", value.GeoSubGroup);
            p.Add("@ProvinceId", value.ProvinceId);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@DistrictId", value.DistrictId);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@SubdistrictId", value.SubdistrictId);
            p.Add("@SubdistrictNameTH", value.SubdistrictNameTH);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@PollingSubUnitNo", value.PollingSubUnitNo);
            p.Add("@VillageCount", value.VillageCount);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);


            if (value.ProvinceId == "67" && 
                value.PollingUnitNo > 1)
            {
                Console.WriteLine("detected");
            }

            try
            {
                cnn.Execute("ImportPollingStation", p, commandType: CommandType.StoredProcedure);

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
