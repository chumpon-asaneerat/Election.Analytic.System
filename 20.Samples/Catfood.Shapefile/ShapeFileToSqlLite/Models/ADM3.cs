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
    #region ADM3

    /// <summary>
    /// The ADM3 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("ADM3")]
    public class ADM3 : NTable<ADM3>
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

        #region ADM2

        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [MaxLength(20)]
        [Indexed]
        public string ADM2Code { get; set; }

        #endregion

        #region ADM3 (ADM3Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM3 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        [Indexed]
        public string ADM3Code { get; set; }
        /// <summary>
        /// Gets or sets Subdistrict Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string SubdistrictNameEN { get; set; }
        /// <summary>
        /// Gets or sets Subdistrict Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string SubdistrictNameTH { get; set; }

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
