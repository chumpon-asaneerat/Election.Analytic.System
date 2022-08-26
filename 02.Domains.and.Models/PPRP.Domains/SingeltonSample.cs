//#define USE_PROGRAM_DATA

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Reflection;

// SQLite
using SQLite;
//using SQLiteNetExtensions.Attributes;
//using SQLiteNetExtensions.Extensions;

using NLib;
using NLib.IO;

using DMT.Models;
using DMT.Views;

#endregion

namespace DMT.Services
{
    #region AccountLocalDbServer

    /// <summary>
    /// Account Local Db Server.
    /// </summary>
    public class AccountLocalDbServer : NSingelton<AccountLocalDbServer>
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        protected AccountLocalDbServer() : base()
        {
            this.FileName = "Account.db";
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~AccountLocalDbServer()
        {
            Shutdown();
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets local json folder path name.
        /// </summary>
        private static string LocalFolder
        {
            get
            {
#if !USE_PROGRAM_DATA
                string localFilder = Folders.Combine(
                    Folders.Assemblies.CurrentExecutingAssembly, "data");
#else
                // Stored in C:\ProgarmData\DMT\Data\ folder
                string localFilder = ApplicationManager.Instance.Environments.Company.Data.FullName;
#endif
                if (!Folders.Exists(localFilder))
                {
                    Folders.Create(localFilder);
                }
                return localFilder;
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods (Start/Shutdown)

        /// <summary>
        /// Start.
        /// </summary>
        public void Start()
        {
            MethodBase med = MethodBase.GetCurrentMethod();
            if (null == Db)
            {
                lock (typeof(AccountLocalDbServer))
                {
                    try
                    {
                        // ---------------------------------------------------------------
                        // NOTE:
                        // ---------------------------------------------------------------
                        // If Exception due to version mismatch here
                        // Please rebuild only this project and try again
                        // VS Should Solve mismatch version properly (maybe)
                        // See: https://nickcraver.com/blog/2020/02/11/binding-redirects/
                        // for more information.
                        // ---------------------------------------------------------------

                        string path = Path.Combine(LocalFolder, FileName);
                        Db = new SQLiteConnection(path,
                            SQLiteOpenFlags.Create |
                            SQLiteOpenFlags.SharedCache |
                            SQLiteOpenFlags.ReadWrite |
                            SQLiteOpenFlags.FullMutex,
                            storeDateTimeAsTicks: false);
                        Db.BusyTimeout = new TimeSpan(0, 0, 5); // set busy timeout.
                    }
                    catch (Exception ex)
                    {
                        med.Err(ex);
                        Db = null;

                        OnConectError.Call(this, EventArgs.Empty);
                    }
                    if (null != Db)
                    {
                        // Set Default connection 
                        // (be careful to make sure that we only has single database
                        // for all domain otherwise call static method with user connnection
                        // in each domain class instead omit connection version).

                        NTable.Default = Db;
                        NQuery.Default = Db;

                        OnConnected.Call(this, EventArgs.Empty);
                    }
                }
            }
        }
        /// <summary>
        /// Shutdown.
        /// </summary>
        public void Shutdown()
        {
            if (null != Db)
            {
                Db.Dispose();
            }
            Db = null;
            OnDisconnected.Call(this, EventArgs.Empty);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets database file name.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets SQLite Connection.
        /// </summary>
        public SQLiteConnection Db { get; private set; }
        /// <summary>
        /// Gets is database connected.
        /// </summary>
        public bool Connected { get { return (null != this.Db); } }

        #endregion

        #region Public Events

        /// <summary>
        /// OnConnected event.
        /// </summary>
        public event EventHandler OnConnected;
        /// <summary>
        /// OnDisconnected event.
        /// </summary>
        public event EventHandler OnDisconnected;
        /// <summary>
        /// OnConectError event.
        /// </summary>
        public event EventHandler OnConectError;

        #endregion
    }

    #endregion
}
