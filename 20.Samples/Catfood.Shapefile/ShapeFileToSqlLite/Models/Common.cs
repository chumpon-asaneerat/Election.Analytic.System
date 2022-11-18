#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using System.ComponentModel;
// required for JsonIgnore.
using Newtonsoft.Json;
using NLib;
using NLib.Reflection;
using System.Reflection;

#endregion

namespace ShapeFileToSqlLite.Models
{
    #region Error Consts

    public enum ErrNums : int
    {
        Success = 0,
        // Local Database Connection (100-149)
        DbConenctFailed = 100,
        // Web Service Connection (150-199)
        RestConenctFailed = 150,
        // Note. remove err num and used http status code instead.
        //RestResponseError = 151,
        RestInvalidConfig = 152,
        // Models - Common (200-210)
        ParameterIsNull = 200,
        // Common Exception
        Exception = 900,
        // Unknown (999)
        UnknownError = 999
    }

    public class ErrConsts
    {
        private static Dictionary<ErrNums, string> _msgs;

        // TODO: Required to add more error message.
        static ErrConsts()
        {
            _msgs = new Dictionary<ErrNums, string>();
            _msgs.Add(ErrNums.Success, "Success.");
            // Local Database
            _msgs.Add(ErrNums.DbConenctFailed, "Database connection failed.");

            // Web Service Connection
            _msgs.Add(ErrNums.RestConenctFailed, "Web Service connection failed.");
            // Note. remove err message and used http error message instead. 
            //_msgs.Add(ErrNums.RestResponseError, "Web Service response error.");

            // Models - common
            _msgs.Add(ErrNums.ParameterIsNull, "Parameter is null.");
            // Common Exception
            _msgs.Add(ErrNums.Exception, "Exception detected.");
            // Unknown
            _msgs.Add(ErrNums.UnknownError, "Unknown error.");
        }

        public static string ErrMsg(ErrNums value)
        {
            if (_msgs.ContainsKey(value))
                return _msgs[value];
            else return _msgs[ErrNums.UnknownError];
        }
    }

    #endregion

    #region NDbError

    /// <summary>
    /// The NDbError class.
    /// </summary>
    public class NDbError
    {
        #region Public Properties

        /// <summary>
        /// Checks has errors.
        /// </summary>
        public bool hasError
        {
            get { return (errNum != 0); }
            set { }
        }
        /// <summary>
        /// Gets or sets error number.
        /// </summary>
        public int errNum { get; set; }
        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        public string errMsg { get; set; }

        #endregion
    }

    #endregion

    #region NDbResult

    /// <summary>
    /// The NDbResult class.
    /// </summary>
    public class NDbResult
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base()
        {
            this.errors = new NDbError();
            UnknownError();
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Set Database (local) Connection Error.
        /// </summary>
        public virtual void DbConenctFailed()
        {
            var err = ErrNums.DbConenctFailed;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Unknown Error.
        /// </summary>
        public virtual void UnknownError()
        {
            var err = ErrNums.UnknownError;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Parameter Is Null Error.
        /// </summary>
        public virtual void ParameterIsNull()
        {
            var err = ErrNums.ParameterIsNull;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Success.
        /// </summary>
        public virtual void Success()
        {
            var err = ErrNums.Success;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ErrConsts.ErrMsg(err);
        }
        /// <summary>
        /// Set Error.
        /// </summary>
        /// <param name="ex">The exception instance.</param>
        public virtual void Error(Exception ex)
        {
            var err = ErrNums.Exception;
            this.errors.errNum = (int)err;
            this.errors.errMsg = ex.Message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets error instance.
        /// </summary>
        public NDbError errors { get; set; }
        /// <summary>
        /// Checks if operation is success.
        /// </summary>
        public virtual bool Ok
        {
            get { return !this.errors.hasError; }
            set { }
        }
        /// <summary>
        /// Checks if operation is failed.
        /// </summary>
        public virtual bool Failed
        {
            get { return this.errors.hasError; }
            set { }
        }

        #endregion
    }

    #endregion

    #region NDbResult<T>

    /// <summary>
    /// The NDbResult class.
    /// </summary>
    /// <typeparam name="T">The Type of data.</typeparam>
    public class NDbResult<T> : NDbResult
        where T : new()
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base()
        {
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Set Database (local) Connection Error.
        /// </summary>
        public override void DbConenctFailed()
        {
            base.DbConenctFailed();
            this.data = Default();
        }
        /// <summary>
        /// Set Unknown Error.
        /// </summary>
        public override void UnknownError()
        {
            base.UnknownError();
            this.data = Default();
        }
        /// <summary>
        /// Set Parameter Is Null Error.
        /// </summary>
        public override void ParameterIsNull()
        {
            base.ParameterIsNull();
            this.data = Default();
        }
        /// <summary>
        /// Set Success.
        /// </summary>
        /// <param name="data">The Data instance.</param>
        public void Success(T data)
        {
            base.Success();
            this.data = (null != data) ? data : Default();
        }
        /// <summary>
        /// Set Error.
        /// </summary>
        /// <param name="ex">The exception instance.</param>
        public override void Error(Exception ex)
        {
            base.Error(ex);
            this.data = Default();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Data Instance.
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// Checks if has data (not null).
        /// </summary>
        public bool HasData
        {
            get { return (null != this.data); }
            set { }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets default instance.
        /// </summary>
        /// <returns>Returns default instance. If T is IList return new instance.</returns>
        public static T Default()
        {
            return (typeof(T) == typeof(IList)) ? new T() : default(T);
        }

        #endregion
    }

    #endregion

    #region NDbResult<T, O>

    /// <summary>
    /// The NDbResult class.
    /// </summary>
    /// <typeparam name="T">The Type of data.</typeparam>
    /// <typeparam name="O">The Type of output.</typeparam>
    public class NDbResult<T, O> : NDbResult
        where T : new()
        where O : new()
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base()
        {
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Set Database (local) Connection Error.
        /// </summary>
        public override void DbConenctFailed()
        {
            base.DbConenctFailed();
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }
        /// <summary>
        /// Set Unknown Error.
        /// </summary>
        public override void UnknownError()
        {
            base.UnknownError();
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }
        /// <summary>
        /// Set Parameter Is Null Error.
        /// </summary>
        public override void ParameterIsNull()
        {
            base.ParameterIsNull();
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }
        /// <summary>
        /// Set Success.
        /// </summary>
        /// <param name="data">The Data instance.</param>
        /// <param name="output">The Output instance.</param>
        public void Success(T data, O output)
        {
            base.Success();
            this.data = (null != data) ? data : DefaultData();
            this.Output = (null != output) ? output : DefaultOutput();
        }
        /// <summary>
        /// Set Error.
        /// </summary>
        /// <param name="ex">The exception instance.</param>
        public override void Error(Exception ex)
        {
            base.Error(ex);
            this.data = DefaultData();
            this.Output = DefaultOutput();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Data Instance.
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// Checks if has data (not null).
        /// </summary>
        public bool HasData
        {
            get { return (null != this.data); }
            set { }
        }
        /// <summary>
        /// Gets Output Instance (for serialized).
        /// </summary>
        public O @out { get; set; }
        /// <summary>
        /// Gets Output Instance.
        /// </summary>
        public O Output
        {
            get { return this.@out; }
            set { this.@out = value; }
        }
        /// <summary>
        /// Checks if has ouput (not null)
        /// </summary>
        public bool HasOutput
        {
            get { return (null != this.Output); }
            set { }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets default instance.
        /// </summary>
        /// <returns>Returns default instance. If T is IList return new instance.</returns>
        public static T DefaultData()
        {
            return (typeof(T) == typeof(IList)) ? new T() : default(T);
        }
        /// <summary>
        /// Gets default instance.
        /// </summary>
        /// <returns>Returns default instance. If T is IList return new instance.</returns>
        public static O DefaultOutput()
        {
            return (typeof(O) == typeof(IList)) ? new O() : default(O);
        }

        #endregion
    }

    #endregion

    #region NDbResult Extension Methods

    /// <summary>
    /// The NDbResult Extension Methods class.
    /// </summary>
    public static class NDbResultExtensionMethods
    {
        #region Public Methods (static)

        /// <summary>
        /// Gets defult value of data.
        /// </summary>
        /// <typeparam name="T">Tht data type.</typeparam>
        /// <returns>Returns default instance. If type is IList new instance returns.</returns>
        public static T DefaultData<T>()
            where T : new()
        {
            return (typeof(T) == typeof(IList)) ? new T() : default(T);
        }
        /// <summary>
        /// Gets defult value of output.
        /// </summary>
        /// <typeparam name="O">The output type.</typeparam>
        /// <returns>Returns default instance. If type is IList new instance returns.</returns>
        public static O DefaultOutput<O>()
            where O : new()
        {
            return (typeof(O) == typeof(IList)) ? new O() : default(O);
        }

        #endregion

        #region Value

        /// <summary>
        /// Gets Data instance.
        /// </summary>
        /// <typeparam name="T">The type for data instance.</typeparam>
        /// <param name="value">The instance to get data.</param>
        /// <returns>Returns data instance.</returns>
        public static T Value<T>(this NDbResult<T> value)
            where T : new()
        {
            T ret = (null != value && !value.errors.hasError && null != value.data) ?
                value.data : DefaultData<T>();
            return ret;
        }
        /// <summary>
        /// Gets Data instance.
        /// </summary>
        /// <typeparam name="T">The type for data instance.</typeparam>
        /// <typeparam name="O">The type for output instance.</typeparam>
        /// <param name="value">The instance to get data.</param>
        /// <returns>Returns data instance.</returns>
        public static T Value<T, O>(this NDbResult<T, O> value)
            where T : new()
            where O : new()
        {
            T ret = (null != value && !value.errors.hasError && null != value.data) ?
                value.data : DefaultData<T>();
            return ret;
        }
        /// <summary>
        /// Gets Output instance.
        /// </summary>
        /// <typeparam name="T">The type for data instance.</typeparam>
        /// <typeparam name="O">The type for output instance.</typeparam>
        /// <param name="value">The instance to get output.</param>
        /// <returns>Returns output instance.</returns>
        public static O Output<T, O>(this NDbResult<T, O> value)
            where T : new()
            where O : new()
        {
            O ret = (null != value && !value.errors.hasError && null != value.Output) ?
                value.Output : DefaultOutput<O>();
            return ret;
        }

        #endregion
    }

    #endregion

    #region ModelBase (abstract)

    /// <summary>
    /// The ModelBase abstract class.
    /// Provide basic implementation of INotifyPropertyChanged interface.
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged
    {
        #region Internal Variables

        private bool _lock = false;

        #endregion

        #region Private Methods

        /// <summary>
        /// Raise Property Changed Event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected void RaiseChanged(string propertyName)
        {
            if (!_lock)
            {
                PropertyChanged.Call(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Enable Notify Change Event.
        /// </summary>
        public void EnableNotify()
        {
            _lock = true;
        }
        /// <summary>
        /// Disable Notify Change Event.
        /// </summary>
        public void DisableNotify()
        {
            _lock = false;
        }
        /// <summary>
        /// Checks is notifiy enabled or disable.
        /// </summary>
        /// <returns>Returns true if enabled.</returns>
        public bool Notified() { return _lock; }

        #endregion

        #region Public Events

        /// <summary>
        /// The PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    #endregion

    // Note:
    // - The Default connection should seperate by Domain class but can initialize with
    //   value assigned in NTable class.
    // - Query static methods (in NTable<T> class) required for custom search/filter.

    #region NTable

    /// <summary>
    /// The NTable abstract class.
    /// </summary>
    public abstract class NTable : ModelBase
    {
        #region Static Variables and Properties

        /// <summary>
        /// sync object used for lock concurrent access.
        /// </summary>
        protected static object sync = new object();
        /// <summary>
        /// Gets default Connection.
        /// </summary>
        public static SQLiteConnection Default { get; set; }

        #endregion
    }

    #endregion

    #region NTable<T>

    /// <summary>
    /// The NTable (Generic) abstract class.
    /// </summary>
    /// <typeparam name="T">The Target Class.</typeparam>
    public abstract class NTable<T> : NTable
        where T : NTable, new()
    {
        #region Static Resources

        /// <summary>The Red Foreground Brush</summary>
        public static SolidColorBrush RedForeground = new SolidColorBrush(Colors.Red);
        /// <summary>The Black Foreground Brush</summary>
        public static SolidColorBrush BlackForeground = new SolidColorBrush(Colors.Black);

        #endregion

        #region Static Methods

        #region Create

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <returns>Returns new instance.</returns>
        public static T Create()
        {
            return new T();
        }

        #endregion

        #region Used Custom Connection

        /// <summary>
        /// Checks is item is already exists in database.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <param name="value">The item to checks.</param>
        /// <returns>Returns true if item is already in database.</returns>
        public static bool Exists(SQLiteConnection db, T value)
        {
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    if (null == db || null == value)
                        return false;
                    // read mapping information.
                    var map = db.GetMapping<T>(CreateFlags.None);
                    if (null == map) return false;

                    string tableName = map.TableName;
                    string columnName = map.PK.Name;
                    string propertyName = map.PK.PropertyName;
                    // get pk id.
                    object Id = PropertyAccess.GetValue(value, propertyName);
                    // init query string.
                    string cmd = string.Empty;
                    cmd += string.Format("SELECT * FROM {0} WHERE {1} = ?", tableName, columnName);
                    // execute query.
                    var item = db.Query<T>(cmd, Id).FirstOrDefault();
                    return (null != item);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    return false;
                }
            }
        }
        /// <summary>
        /// Save.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <param name="value">The item to save to database.</param>
        public static NDbResult<T> Save(SQLiteConnection db, T value)
        {
            NDbResult<T> result = new NDbResult<T>();
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            if (null == db)
            {
                result.ParameterIsNull();
                return result;
            }

            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                if (!Exists(db, value))
                {
                    try
                    {
                        db.Insert(value);
                        result.Success(value);
                    }
                    catch (Exception ex)
                    {
                        med.Err(ex);
                        result.Error(ex);
                    }
                }
                else
                {
                    try
                    {
                        db.Update(value);
                        result.Success(value);
                    }
                    catch (Exception ex)
                    {
                        med.Err(ex);
                        result.Error(ex);
                    }
                }

                return result;
            }
        }
        /// <summary>
        /// Update relationship with children that assigned with relationship attribute.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <param name="value">The item to load children.</param>
        public static NDbResult UpdateWithChildren(SQLiteConnection db, T value)
        {
            NDbResult result = new NDbResult();
            lock (sync)
            {
                if (null == db || null == value)
                {
                    result.ParameterIsNull();
                    return result;
                }
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    db.UpdateWithChildren(value);
                    result.Success();
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }
        /// <summary>
        /// Get All with children.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <param name="recursive">True for load related nested children.</param>
        /// <returns>Returns List of all records</returns>
        public static NDbResult<List<T>> GetAllWithChildren(SQLiteConnection db,
            bool recursive = false)
        {
            var result = new NDbResult<List<T>>();
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    var results = db.GetAllWithChildren<T>(recursive: recursive);
                    result.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }
        /// <summary>
        /// Gets by Id with children.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <param name="Id">The Id (primary key).</param>
        /// <param name="recursive">True for load related nested children.</param>
        /// <returns>Returns found record.</returns>
        public static NDbResult<T> GetWithChildren(SQLiteConnection db,
            object Id, bool recursive = false)
        {
            var result = new NDbResult<T>();
            if (null == db)
            {
                result.DbConenctFailed();
                return result;
            }
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    // read mapping information.
                    var map = db.GetMapping<T>(CreateFlags.None);
                    if (null == map) return null;

                    string tableName = map.TableName;
                    string columnName = map.PK.Name;
                    string propertyName = map.PK.PropertyName;
                    // init query string.
                    string cmd = string.Empty;
                    cmd += string.Format("SELECT * FROM {0} WHERE {1} = ?", tableName, columnName);
                    // execute query.
                    T item = db.Query<T>(cmd, Id).FirstOrDefault();
                    if (null != item)
                    {
                        // read children.
                        db.GetChildren(item, recursive);
                    }
                    result.Success(item);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    result.Error(ex);
                }
                return result;
            }
        }
        /// <summary>
        /// Delete All.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <returns>Returns number of rows deleted.</returns>
        public static int DeleteAll(SQLiteConnection db)
        {
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                int cnt = 0;
                try
                {
                    if (null != db)
                    {
                        cnt = db.DeleteAll<T>();
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    cnt = 0;
                }
                return cnt;
            }
        }
        /// <summary>
        /// Delete by Id with children.
        /// </summary>
        /// <param name="db">The connection.</param>
        /// <param name="Id">The Id (primary key).</param>
        /// <param name="recursive">True for load related nested children.</param>
        public static void DeleteWithChildren(SQLiteConnection db, object Id, bool recursive = false)
        {
            lock (sync)
            {
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    if (null == db || null == Id) return;
                    var ret = GetWithChildren(db, Id, recursive);
                    if (ret.Ok && ret.HasData)
                    {
                        T inst = ret.Value();
                        db.Delete(inst, recursive);
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }
            }
        }

        #endregion

        #region Used Default Connection

        /// <summary>
        /// Checks is item is already exists in database.
        /// </summary>
        /// <param name="value">The item to checks.</param>
        /// <returns>Returns true if item is already in database.</returns>
        public static bool Exists(T value)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return Exists(db, value);
            }
        }
        /// <summary>
        /// Save.
        /// </summary>
        /// <param name="value">The item to save to database.</param>
        public static NDbResult<T> Save(T value)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return Save(db, value);
            }
        }
        /// <summary>
        /// Update relationship with children that assigned with relationship attribute.
        /// </summary>
        /// <param name="value">The item to load children.</param>
        public static void UpdateWithChildren(T value)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                UpdateWithChildren(db, value);
            }
        }
        /// <summary>
        /// Gets All with children.
        /// </summary>
        /// <param name="recursive">True for load related nested children.</param>
        /// <returns>Returns List of all records</returns>
        public static NDbResult<List<T>> GetAllWithChildren(bool recursive = false)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return GetAllWithChildren(db, recursive);
            }
        }
        /// <summary>
        /// Gets by Id with children.
        /// </summary>
        /// <param name="Id">The Id (primary key).</param>
        /// <param name="recursive">True for load related nested children.</param>
        /// <returns>Returns found record.</returns>
        public static NDbResult<T> GetWithChildren(object Id, bool recursive = false)
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return GetWithChildren(db, Id, recursive);
            }
        }
        /// <summary>
        /// Delete All.
        /// </summary>
        /// <returns>Returns number of rows deleted.</returns>
        public static int DeleteAll()
        {
            lock (sync)
            {
                SQLiteConnection db = Default;
                return DeleteAll(db);
            }
        }
        /// <summary>
        /// Delete by Id with children.
        /// </summary>
        /// <param name="Id">The Id (primary key).</param>
        /// <param name="recursive">True for load related nested children.</param>
        public static void DeleteWithChildren(object Id, bool recursive = false)
        {
            lock (sync)
            {
                // TODO: Need Implements Delete With Id.
                SQLiteConnection db = Default;
                DeleteWithChildren(db, recursive);
            }
        }

        #endregion

        #endregion
    }

    #endregion

    #region IFKs interface

    /// <summary>
    /// The IFKs interface of T.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    public interface IFKs<T>
        where T : NTable, new()
    {
    }

    #endregion

    #region NTable Extension Methods

    /// <summary>
    /// The NTable Extension Methods.
    /// </summary>
    public static class NTableExtensionMethods
    {
        /// <summary>
        /// Convert instance of IFKs To target Model and assigned match properties.
        /// </summary>
        /// <typeparam name="T">The target instance type.</typeparam>
        /// <param name="value">The source to assign properties into new instance.</param>
        /// <returns>Returns new instance of T model.</returns>
        public static T ToModel<T>(this IFKs<T> value)
            where T : NTable, new()
        {
            T inst = new T();
            if (null != value) value.AssignTo(inst);
            return inst;
        }
        /// <summary>
        /// Convert List of instance of IFKs To target Model and assigned match properties.
        /// </summary>
        /// <typeparam name="T">The target instance type.</typeparam>
        /// <param name="values">The source list.</param>
        /// <returns>Returns new List of instance of T model.</returns>
        public static List<T> ToModels<T>(this IEnumerable<IFKs<T>> values)
            where T : NTable, new()
        {
            List<T> insts = new List<T>();
            if (null != values)
            {
                foreach (var value in values)
                {
                    if (null != value)
                    {
                        T inst = new T();
                        value.AssignTo(inst);
                        insts.Add(inst);
                    }
                }
            }
            return insts;
        }
    }

    #endregion
}
