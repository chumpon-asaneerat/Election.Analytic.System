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

        #region Primary Key

        /// <summary>
        /// Gets or sets Id (Primary Key auto generate).
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        #endregion

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

        #region ADM2

        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [MaxLength(20)]
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
}
