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
    #region ADM1

    /// <summary>
    /// The ADM1 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("ADM1")]
    public class ADM1 : NTable<ADM1>
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

        #region ADM1 (ADM1Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        [Indexed]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets Province Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string ProvinceNameEN { get; set; }
        /// <summary>
        /// Gets or sets Province Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string ProvinceNameTH { get; set; }

        #endregion

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion
}
