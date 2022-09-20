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
    #region MPD2562VoteSummary

    public class MPD2562VoteSummary
    {
        #region Public Properties

        public int RowNo { get; set; }
        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public string FullName { get; set; }
        public int VoteNo { get; set; }
        public string PartyName { get; set; }
        public int VoteCount { get; set; }
        public int RevoteNo { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2562VoteSummary>>  Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPD2562VoteSummary>> rets = new NDbResult<List<MPD2562VoteSummary>>();

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
                    ;WITH VoteSum62
                    AS
                    -- Define the Vote Summary by Province and PollingUnit query.
                    (
                        SELECT ROW_NUMBER() OVER(ORDER BY VoteCount DESC) AS RowNo
                             , * 
                          FROM MPD2562VoteSummary
                    )
                    SELECT * FROM VoteSum62 
                     ORDER BY ProvinceName, PollingUnitNo, VoteCount DESC
                ";

                rets.Value = cnn.Query<MPD2562VoteSummary>(query).ToList();
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
                rets.Value = new List<MPD2562VoteSummary>();
            }

            return rets;
        }

        public static NDbResult<List<MPD2562VoteSummary>> Gets(string provinceName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

            NDbResult<List<MPD2562VoteSummary>> rets = new NDbResult<List<MPD2562VoteSummary>>();

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
                    ;WITH VoteSum62
                    AS
                    -- Define the Vote Summary by Province and PollingUnit query.
                    (
                        SELECT ROW_NUMBER() OVER(ORDER BY ProvinceName, PollingUnitNo, VoteCount DESC) AS RowNo
                             , * 
                          FROM MPD2562VoteSummary
                         WHERE UPPER(LTRIM(RTRIM(ProvinceName))) = UPPER(LTRIM(RTRIM(COALESCE(@ProvinceName, ProvinceName))))
                    )
                    SELECT * FROM VoteSum62 
                     ORDER BY ProvinceName, PollingUnitNo, VoteCount DESC
                ";

                rets.Value = cnn.Query<MPD2562VoteSummary>(query, new { ProvinceName = sProvinceName }).ToList();
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
                rets.Value = new List<MPD2562VoteSummary>();
            }

            return rets;
        }

        public static NDbResult<MPD2562VoteSummary> GetPrevYearInfoByFullName(string fullName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                sFullName = null;
            }

            NDbResult<MPD2562VoteSummary> rets = new NDbResult<MPD2562VoteSummary>();

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
                    ;WITH VoteSum62_A
                    AS
                    (
                        SELECT ProvinceName, PollingUnitNo 
                          FROM MPD2562VoteSummary 
                         WHERE UPPER(LTRIM(RTRIM(FullName))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@FullName1, FullName)))) + '%' 
                    ),
                    VoteSum62_B
                    AS
                    -- Find the Vote Summary by Province and PollingUnit query.
                    (
                        SELECT ROW_NUMBER() OVER(ORDER BY A.VoteCount DESC) AS RowNo
                             , A.* 
                          FROM MPD2562VoteSummary A JOIN VoteSum62_A B
                           ON (
                                   UPPER(LTRIM(RTRIM(A.ProvinceName))) = UPPER(LTRIM(RTRIM(B.ProvinceName)))
                               AND A.PollingUnitNo = B.PollingUnitNo
                              )
                    )
                    SELECT * FROM VoteSum62_B
                     WHERE UPPER(LTRIM(RTRIM(FullName))) LIKE '%' + UPPER(LTRIM(RTRIM(COALESCE(@FullName2, FullName)))) + '%' 
                    ORDER BY ProvinceName, PollingUnitNo, VoteCount DESC
                ";

                rets.Value = cnn.Query<MPD2562VoteSummary>(query, 
                    new { FullName1 = sFullName, FullName2 = sFullName }).FirstOrDefault();
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
                rets.Value = null;
            }

            return rets;
        }

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

        #endregion
    }

    #endregion

    #region MPD2562PrintVoteSummary

    public class MPD2562PrintVoteSummary
    {
        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public string FullName { get; set; }
        public int VoteNo { get; set; }
        public string PartyName { get; set; }
        public int VoteCount { get; set; }
        public int RevoteNo { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2562PrintVoteSummary>> Gets(string provinceName)
        {
            string sProvinceName = provinceName;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPD2562PrintVoteSummary>> rets = new NDbResult<List<MPD2562PrintVoteSummary>>();

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
                      FROM MPD2562VoteSummary
                     WHERE UPPER(LTRIM(RTRIM(ProvinceName))) = UPPER(LTRIM(RTRIM(COALESCE(@ProvinceName, ProvinceName))))
                     ORDER BY ProvinceName, PollingUnitNo, VoteCount DESC
                ";

                rets.Value = cnn.Query<MPD2562PrintVoteSummary>(query, new { ProvinceName = sProvinceName }).ToList();
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
                rets.Value = new List<MPD2562PrintVoteSummary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MPD2562PersonalVoteSummary

    public class MPD2562PersonalVoteSummary : NInpc
    {
        #region Internal Variables

        // for party logo
        private byte[] _LogoData = null;
        private bool _PartyLogoLoading = false;
        private ImageSource _PartyLogo = null;
        // for person image
        private byte[] _PersonImageData = null;
        private bool _PersonImageLoading = false;
        private ImageSource _PersonImage = null;

        #endregion

        #region Public Properties

        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }

        public string FullName { get; set; }

        public byte[] PersonImageData
        {
            get { return _PersonImageData; }
            set
            {
                _PersonImageData = value;
                if (null == _PersonImageData)
                {
                    _PersonImage = null;
                }
            }
        }

        public ImageSource PersonImage 
        { 
            get 
            {
                if (null == _PersonImage && !_PersonImageLoading)
                {
                    _PersonImageLoading = true;
                    ImageSource imgSrc;
                    if (null == _PersonImageData)
                    {
                        imgSrc = Defaults.Person;
                    }
                    else
                    {
                        imgSrc = ByteUtils.GetImageSource(PersonImageData);
                    }

                    _PersonImage = imgSrc;
                    _PersonImageLoading = false;
                    Raise(() => PersonImage);
                }

                return _PersonImage;
            }
            set { } 
        }

        public string PartyId { get; set; }
        public string PartyName { get; set; }

        public byte[] LogoData 
        { 
            get { return _LogoData; }
            set
            {
                _LogoData = value;
                if (null == _LogoData)
                {
                    _PartyLogo = null;
                }
            }
        }
        public ImageSource PartyLogo 
        { 
            get 
            {
                if (null == _PartyLogo && null != _LogoData && !_PartyLogoLoading)
                {
                    _PartyLogoLoading = true;
                    _PartyLogo = ByteUtils.GetImageSource(LogoData);
                    _PartyLogoLoading = false;
                    Raise(() => PartyLogo);
                }
                return _PartyLogo;
            }
            set { } 
        }

        public int VoteNo { get; set; }
        public int VoteCount { get; set; }

        #endregion

        #region Static Methods

        /*
        public static NDbResult<List<MPD2562PersonalVoteSummary>> Gets(string provinceId, int pollingUnitNo)
        {
            return Gets(6, provinceId, pollingUnitNo);
        }
        */
        public static NDbResult<List<MPD2562PersonalVoteSummary>> Gets(int top, string provinceId, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPD2562PersonalVoteSummary>> rets = new NDbResult<List<MPD2562PersonalVoteSummary>>();

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
                query += "SELECT TOP " + top.ToString() + " ";
                query += @"
                         B.ProvinceId
                       , B.ProvinceNameTH
                       , A.PollingUnitNo
                       , A.FullName
                       , IMG.Data AS PersonImageData
                       , C.PartyId
                       , A.PartyName
                       , C.Data AS LogoData
                       , A.VoteNo
                       , A.VoteCount
                   FROM MPD2562VoteSummary A
                            LEFT OUTER JOIN (SELECT P.PartyId
                                                  , P.PartyName  
                                                  , CT.Data
                                               FROM MParty P LEFT OUTER JOIN MContent CT 
                                                 ON P.ContentId = CT.ContentId) C 
                                         ON (
                                              UPPER(LTRIM(RTRIM(A.PartyName))) = UPPER(LTRIM(RTRIM(C.PartyName)))
                                            )
                            LEFT OUTER JOIN PersonImage IMG 
                                         ON (   
                                                 (IMG.FullName = A.FullName)
                                              OR (IMG.FullName LIKE '%' + A.FullName + '%')
                                              OR (A.FullName LIKE '%' + IMG.FullName + '%')
                                            )
                      , MProvince B 
                  WHERE UPPER(LTRIM(RTRIM(A.ProvinceName))) = UPPER(LTRIM(RTRIM(B.ProvinceNameTH)))
                    AND B.ProvinceId = @ProvinceId
                    AND A.PollingUnitNo = @PollingUnitNo
                  ORDER BY A.VoteCount DESC
                ";

                rets.Value = cnn.Query<MPD2562PersonalVoteSummary>(query,
                    new { ProvinceId = provinceId, PollingUnitNo = pollingUnitNo }).ToList();
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
                rets.Value = new List<MPD2562PersonalVoteSummary>();
            }

            return rets;
        }

        public static int GetTotalVotes(string provinceId, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            int ret = 0;

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                return ret;
            }

            try
            {
                string query = string.Empty;
                query += @"
                    SELECT SUM(A.VoteCount) AS TotalVotes
                     FROM MPD2562VoteSummary A
                        , MProvince B 
                     WHERE UPPER(LTRIM(RTRIM(A.ProvinceName))) = UPPER(LTRIM(RTRIM(B.ProvinceNameTH)))
                    AND B.ProvinceId = @ProvinceId
                    AND A.PollingUnitNo = @PollingUnitNo
                ";

                ret = cnn.ExecuteScalar<int>(query,
                    new { ProvinceId = provinceId, PollingUnitNo = pollingUnitNo });
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return ret;
        }

        #endregion
    }

    #endregion
}
