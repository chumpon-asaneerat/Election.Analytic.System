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
    #region ADM0

    /// <summary>
    /// The ADM0 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("ADM0")]
    public class ADM0 : NTable<ADM0>
    {
        #region Public Properties

        #region ADM0 (ADM0Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        public string ADM0Code { get; set; }
        /// <summary>
        /// Gets or sets Country Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string CountryNameEN { get; set; }
        /// <summary>
        /// Gets or sets Country Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string CountryNameTH { get; set; }

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

    #region ADM0Part

    public class ADM0Part : NTable<ADM0Part>
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
        /// Gets or sets RecordId.
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets Point count.
        /// </summary>
        public int PointCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<ADM0Part> Get(string ADM0Code, int recordId)
        {
            NDbResult<ADM0Part> ret = new NDbResult<ADM0Part>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try 
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM ADM0Part ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    var results = NQuery.Query<ADM0Part>(cmd, ADM0Code, recordId).FirstOrDefault();
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

    #region ADM0Point

    public class ADM0Point : NTable<ADM0Point>
    {
        #region Public Properties

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion
}
