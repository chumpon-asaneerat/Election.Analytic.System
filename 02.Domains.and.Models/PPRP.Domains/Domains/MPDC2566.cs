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
        public string SubGroup { get; set; }
        public string Remark { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPDC2566>> Gets(string provinceName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

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
                     WHERE UPPER(LTRIM(RTRIM(ProvinceName))) = UPPER(LTRIM(RTRIM(COALESCE(@ProvinceName, ProvinceName))))
                     ORDER BY ProvinceName, PollingUnitNo
                ";

                rets.Value = cnn.Query<MPDC2566>(query, new { ProvinceName = sProvinceName }).ToList();
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

        public static NDbResult Save(MPDC2566 value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult ret = new NDbResult();

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

            var p = new DynamicParameters();
            p.Add("@ProvinceName", value.ProvinceName);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@CandidateNo", value.CandidateNo);
            p.Add("@FullName", value.FullName);
            p.Add("@PrevPartyName", value.PrevPartyName);
            p.Add("@EducationLevel", value.EducationLevel);
            p.Add("@SubGroup", value.SubGroup);
            p.Add("@Remark", value.Remark);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMPDC2566", p, commandType: CommandType.StoredProcedure);
                // Set error number/message
                ret.ErrNum = p.Get<int>("@errNum");
                ret.ErrMsg = p.Get<string>("@errMsg");
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

        public static NDbResult Import(MPDC2566 value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult ret = new NDbResult();

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

            var p = new DynamicParameters();
            p.Add("@ProvinceName", value.ProvinceName);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@CandidateNo", value.CandidateNo);
            p.Add("@FullName", value.FullName);
            p.Add("@PrevPartyName", value.PrevPartyName);
            p.Add("@EducationLevel", value.EducationLevel);
            p.Add("@SubGroup", value.SubGroup);
            p.Add("@Remark", value.Remark);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPDC2566", p, commandType: CommandType.StoredProcedure);
                // Set error number/message
                ret.ErrNum = p.Get<int>("@errNum");
                ret.ErrMsg = p.Get<string>("@errMsg");
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

        #endregion
    }

    public class MPDC2566Summary : NInpc
    {
        #region Internal Variables

        // for person image
        private byte[] _PersonImageData = null;
        private bool _PersonImageLoading = false;
        private ImageSource _PersonImage = null;

        #endregion

        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int CandidateNo { get; set; }
        public string FullName { get; set; }
        public string PrevPartyName { get; set; }
        public string EducationLevel { get; set; }
        public string SubGroup { get; set; }
        public string Remark { get; set; }

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

                    Defaults.RunInBackground(() =>
                    {
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
                    });
                }

                return _PersonImage;
            }
            set { }
        }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPDC2566Summary>> Gets(string provinceId, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPDC2566Summary>> rets = new NDbResult<List<MPDC2566Summary>>();

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
                    SELECT TOP 4
                          B.ProvinceId
                        , B.ProvinceNameTH AS ProvinceName
                        , A.PollingUnitNo
                        , A.FullName
                        , IMG.Data AS PersonImageData
                        , C.PartyId
                        , A.PrevPartyName
                        , C.Data AS LogoData
                        , A.CandidateNo
                        , A.EducationLevel
                        , A.SubGroup
                        , A.Remark
                     FROM MPDC2566 A
                            LEFT OUTER JOIN (SELECT P.PartyId
                                                  , P.PartyName  
                                                  , CT.Data
                                                FROM MParty P LEFT OUTER JOIN MContent CT 
                                                    ON P.ContentId = CT.ContentId) C 
                                            ON (
                                                UPPER(LTRIM(RTRIM(A.PrevPartyName))) = UPPER(LTRIM(RTRIM(C.PartyName)))
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
                    ORDER BY A.CandidateNo
                ";

                rets.Value = cnn.Query<MPDC2566Summary>(query,
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
                rets.Value = new List<MPDC2566Summary>();
            }

            return rets;
        }

        #endregion
    }
}
