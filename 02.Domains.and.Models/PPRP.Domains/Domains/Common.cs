#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    public class NDbResult<T>
        where T: class, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base() 
        {
            ErrNum = 0;
            ErrMsg = string.Empty;
        }

        #endregion

        #region Public Properties

        public T Value { get; set; }

        public int ErrNum { get; set; }
        public string ErrMsg { get; set; }
        public bool HasError 
        { 
            get { return ErrNum != 0; } 
            set { } 
        }

        #endregion
    }
}
