#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

#endregion

namespace ShapeFileToSqlLite.Models
{
    #region ADM2

    /// <summary>
    /// The ADM2 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("ADM2")]
    public class ADM2 : NTable<ADM2>
    {
        #region Public Properties

        #region ADM0

        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [MaxLength(20)]
        [Indexed]
        public string ADM0Code { get; set; }

        #endregion

        #region ADM1

        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [MaxLength(20)]
        [Indexed]
        public string ADM1Code { get; set; }

        #endregion

        #region ADM2 (ADM2Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        [Indexed]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets District Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string DistrictNameEN { get; set; }
        /// <summary>
        /// Gets or sets District Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string DistrictNameTH { get; set; }

        #endregion

        #region Bounds

        /// <summary>
        /// Gets Bound Left position.
        /// </summary>
        public double BoundLeft { get; set; }
        /// <summary>
        /// Gets Bound Top position.
        /// </summary>
        public double BoundTop { get; set; }
        /// <summary>
        /// Gets Bound Right position.
        /// </summary>
        public double BoundRight { get; set; }
        /// <summary>
        /// Gets Bound Bottom position.
        /// </summary>
        public double BoundBottom { get; set; }

        #endregion

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion

    #region ADM2Part

    public class ADM2Part : NTable<ADM2Part>
    {
        #region Public Properties

        /// <summary>
        /// Gets or set Id.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM0Code { get; set; }
        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets ADM3 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM3Code { get; set; }
        /// <summary>
        /// Gets or sets RecordId.
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets Point count.
        /// </summary>
        public int PointCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<ADM2Part> Get(
            string ADM0Code, string ADM1Code, string ADM2Code, 
            int recordId)
        {
            NDbResult<ADM2Part> ret = new NDbResult<ADM2Part>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM ADM2Part ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND ADM2Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    var results = NQuery.Query<ADM2Part>(cmd,
                        ADM0Code, ADM1Code, ADM2Code, 
                        recordId).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }

        #endregion
    }

    #endregion

    #region ADM2Point

    public class ADM2Point : NTable<ADM2Point>
    {
        #region Public Properties
        /// <summary>
        /// Gets or set Id.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM0Code { get; set; }
        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets ADM3 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM3Code { get; set; }
        /// <summary>
        /// Gets or sets RecordId.
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets Point Id.
        /// </summary>
        public int PointId { get; set; }
        /// <summary>
        /// Gets or sets Point X position.
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Gets or sets Point Y position.
        /// </summary>
        public double Y { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<ADM2Point> Get(
            string ADM0Code, string ADM1Code, string ADM2Code, 
            int recordId, int pointId)
        {
            NDbResult<ADM2Point> ret = new NDbResult<ADM2Point>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM ADM2Point ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND ADM2Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PointId = ? ";
                    var results = NQuery.Query<ADM2Point>(cmd,
                        ADM0Code, ADM1Code, ADM2Code, 
                        recordId, pointId).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }

        #endregion
    }

    #endregion
}
