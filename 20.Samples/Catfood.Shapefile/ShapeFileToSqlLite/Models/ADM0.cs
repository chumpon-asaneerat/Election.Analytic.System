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
        /// <summary>
        /// Gets or sets Country EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string CountryEN { get; set; }
        /// <summary>
        /// Gets or sets Country TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string CountryTH { get; set; }

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
