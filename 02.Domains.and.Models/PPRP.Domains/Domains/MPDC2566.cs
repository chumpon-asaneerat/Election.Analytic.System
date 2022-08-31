#region Using

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
	public class MPDC2566
	{
		#region Public Properties

		public string ProvinceName { get; set; }
		public int PollingUnitNo { get; set; }
		public int CandidateNo { get; set; }
		public string FullName { get; set; }
		public string PrevPartyName { get; set; }
		public string EducationLevel { get; set; }
		public string Remark { get; set; }

		#endregion

		#region Static Methods

		public static NDbResult<List<MPDC2566>> Gets()
		{
			MethodBase med = MethodBase.GetCurrentMethod();

			NDbResult<List<MPDC2566>> rets = new NDbResult<List<MPDC2566>>();

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
					  FROM MPDC2566
				";

				rets.Value = cnn.Query<MPDC2566>(query).ToList();
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
				rets.Value = new List<MPDC2566>();
			}

			return rets;
		}

		public static void Save(MPDC2566 value)
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
			p.Add("@CandidateNo", value.CandidateNo);
			p.Add("@FullName", value.FullName);
			p.Add("@PrevPartyName", value.PrevPartyName);
			p.Add("@EducationLevel", value.EducationLevel);
			p.Add("@Remark", value.Remark);

			p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
			p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

			try
			{
				cnn.Execute("SaveMPDC2566", p, commandType: CommandType.StoredProcedure);

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
