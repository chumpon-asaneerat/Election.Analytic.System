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
    public class MPD2562VoteSummary
    {
        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public string FullName { get; set; }
        public int VoteNo { get; set; }
        public string PartyName { get; set; }
        public int VoteCount { get; set; }
        public int RevoteNo { get; set; }

        public static void Save(MPD2562VoteSummary value)
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
            p.Add("@FullName", value.FullName);
            p.Add("@VoteNo", value.VoteNo);
            p.Add("@PartyName", value.PartyName);
            p.Add("@VoteCount", value.VoteCount);
            p.Add("@RevoteNo", value.RevoteNo);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMPD2562VoteSummary", p, commandType: CommandType.StoredProcedure);

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
    }
}
