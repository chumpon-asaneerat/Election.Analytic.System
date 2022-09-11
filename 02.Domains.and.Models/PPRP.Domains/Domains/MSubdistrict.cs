#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

using System.Windows.Media;

using NLib;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    /// <summary>
    /// The MSubdistrict class.
    /// </summary>
    public class MSubdistrict
    {
        #region Public Properties

        public string SubdistrictId { get; set; }
        public string SubdistrictNameTH { get; set; }
        public string SubdistrictNameEN { get; set; }
        public string ADM3Code { get; set; }
        public decimal SubdistrictAreaM2 { get; set; }
        public Guid SubdistrictContentId { get; set; }

        public string DistrictId { get; set; }
        public string DistrictNameTH { get; set; }
        public string DistrictNameEN { get; set; }
        public string ADM2Code { get; set; }

        public string ProvinceId { get; set; }
        public string ProvinceNameTH { get; set; }
        public string ProvinceNameEN { get; set; }
        public string ADM1Code { get; set; }

        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string GeoGroup { get; set; }
        public string GeoSubGroup { get; set; }

        #endregion

        #region Static Methods

        #endregion
    }
}
