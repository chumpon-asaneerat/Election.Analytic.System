#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Linq;

using System.Windows;
using System.Windows.Forms;

using Microsoft.Win32;

using NLib;
using OfficeOpenXml;
using EPPlus;
using EPPlus.DataExtractor;

#endregion

namespace PPRP.Imports.Excel
{
    #region NExcelAnalyzeResult

    #endregion

    #region NExcelMapProperty

    /// <summary>
    /// The NExcelMapProperty class.
    /// </summary>
    public class NExcelMapProperty : NInpc
    {
        private string _ColumnLetter = string.Empty;

        #region Public Properties

        /// <summary>Gets or sets Target property name.</summary>
        public string PropertyName { get; set; }
        /// <summary>Gets or sets Excel column'name like 'A', 'B', 'C', etc.</summary>
        public string ColumnLetter 
        { 
            get { return _ColumnLetter; }
            set
            {
                if (_ColumnLetter != value)
                {
                    _ColumnLetter = value;
                    this.Raise(() => this.ColumnLetter);
                    this.Raise(() => this.Summary);
                }
            }
        }

        public string Summary 
        {
            get { return string.Format("'{0}' => '{1}'", PropertyName, _ColumnLetter); }
            set { }
        }


        private NExcelColumn _SelectedColumn;
        public NExcelColumn SelectedColumn
        {
            get { return _SelectedColumn; }
            set
            {
                _SelectedColumn = value;
                this.ColumnLetter = (null != _SelectedColumn) ? _SelectedColumn.ColumnLetter : string.Empty;
            }
        }

        /// <summary>
        /// For lookup bindings.
        /// </summary>
        public List<NExcelColumn> Columns { get; set; }

        #endregion
    }

    #endregion

    #region NExcelColumn

    /// <summary>
    /// The NExcelColumn class.
    /// </summary>
    public class NExcelColumn
    {
        #region Public Properties

        /// <summary>Gets or sets excel worksheet's column name.</summary>
        public string ColumnName { get; set; }
        /// <summary>Gets or sets excel worksheet's column index. This index is start with 1.</summary>
        public int ColumnIndex { get; set; }

        /// <summary>Gets or sets excel worksheet's column letter like 'A', 'B', ..., 'AA', etc.</summary>
        public string ColumnLetter { get; set; }

        #endregion
    }

    #endregion
}

namespace PPRP.Imports.Excel
{
    #region NExcelWorksheet

    /// <summary>
    /// The NExcelWorksheet class.
    /// </summary>
    public class NExcelWorksheet
    {
        #region Private Mehods

        private List<NExcelColumn> _columns = new List<NExcelColumn>();

        #endregion

        #region Override Methods

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">The target object instance.</param>
        /// <returns>Returns true if target instance is equal to current instance</returns>
        public override bool Equals(object obj)
        {
            if (null == obj) return false;
            var curr = this.GetHashCode();
            var target = obj.GetHashCode();
            return curr.Equals(target);
        }
        /// <summary>
        /// GetHashCode.
        /// </summary>
        /// <returns>Returns hash code of object instance.</returns>
        public override int GetHashCode()
        {
            string sVal = this.ToString();
            return sVal.GetHashCode();
        }
        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns>Returns string that represents object instance.</returns>
        public override string ToString()
        {
            //int colCnt = (null == this.Columns) ? -1 : this.Columns.Count;
            //string code = string.Format("{0}_{1}", this.SheetName, colCnt);

            string code = string.Format("{0}", this.SheetName);
            return code.ToString();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets excel worksheet name.</summary>
        public string SheetName { get; set; }

        /// <summary>Gets or sets excel worksheet columns.</summary>
        public List<NExcelColumn> Columns
        {
            get { return _columns; }
            set { }
        }

        #endregion
    }

    #endregion

    #region NExcelImportWizard

    /// <summary>
    /// The NExcelImportWizard class.
    /// </summary>
    public class NExcelImportWizard : NInpc
    {
        #region Internal Variables

        private ObservableCollection<string> _steps = null;

        private int _maxStep = 0;
        private int _progress = 0;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NExcelImportWizard() : base() 
        {
            _steps = new ObservableCollection<string>();
            //_steps.CollectionChanged += _steps_CollectionChanged;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NExcelImportWizard()
        {
            if (null != _steps)
            {
                //_steps.CollectionChanged -= _steps_CollectionChanged;
                _steps.Clear();
            }
            _steps = null;
        }

        #endregion

        #region Private Methods

        private void _steps_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Reset to first step.
            //FirstStep();
        }

        private void RaiseProgressEvents()
        {
            // Raise ProeprtyChanged Events
            this.Raise(() => this.Progress);

            this.Raise(() => this.CanGoPrevious);
            this.Raise(() => this.CanGoNext);
            this.Raise(() => this.IsFinished);

            this.Raise(() => this.GoPreviousVisibility);
            this.Raise(() => this.GoNextVisibility);
            this.Raise(() => this.FinishedStepVisibility);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Move to First steps.
        /// </summary>
        public void FirstStep()
        {
            _progress = 0;
            _maxStep = 0;
            _maxStep = (null == _steps || _steps.Count <= 0) ? 0 : _steps.Count - 1;

            // Raise ProeprtyChanged Events
            this.RaiseProgressEvents();
        }
        /// <summary>
        /// Move to previous wizard step.
        /// </summary>
        public void PreviousStep()
        {
            --_progress;
            if (_progress < 0) _progress = 0;

            // Raise ProeprtyChanged Events
            this.RaiseProgressEvents();
        }
        /// <summary>
        /// Move to next wizard step.
        /// </summary>
        public void NextStep()
        {
            ++_progress;
            if (_progress > _maxStep) _progress = _maxStep;

            // Raise ProeprtyChanged Events
            this.RaiseProgressEvents();
        }

        #endregion

        #region Public Properties

        #region Wizard Steps and Progress

        /// <summary>Gets steps collection.</summary>
        public ObservableCollection<string> Steps
        {
            get { return _steps; }
            set { }
        }
        /// <summary>Gets current step.</summary>
        public int Progress
        {
            get { return _progress; }
            set { }
        }

        #endregion

        #region For Runtime IsEnabled, Visibility Bindings

        /// <summary>Checks can go to previous step.</summary>
        public bool CanGoPrevious
        {
            get
            {
                if (_maxStep == 0)
                    return false;
                bool isFirstStep = _progress <= 0;
                return !isFirstStep;
            }
            set { }
        }
        /// <summary>Gets or sets GoPrevious Visibility.</summary>
        public Visibility GoPreviousVisibility
        {
            get
            {
                var ret = (CanGoPrevious) ? Visibility.Visible : Visibility.Collapsed;
                return ret;
            }
            set { }
        }
        /// <summary>Checks can go to next step.</summary>
        public bool CanGoNext
        {
            get
            {
                if (_maxStep == 0)
                    return false;
                bool isLastStep = _progress >= _maxStep;
                return !isLastStep;
            }
            set { }
        }
        /// <summary>Gets or sets GoNext Visibility.</summary>
        public Visibility GoNextVisibility
        {
            get
            {
                var ret = (CanGoNext) ? Visibility.Visible : Visibility.Collapsed;
                return ret;
            }
            set { }
        }
        /// <summary>Checks is finished step.</summary>
        public bool IsFinished
        {
            get
            {
                if (_maxStep == 0)
                    return false;
                bool isFinishedStep = _progress == _maxStep;
                return isFinishedStep;
            }
            set { }
        }
        /// <summary>Gets or sets Finished Step Visibility.</summary>
        public Visibility FinishedStepVisibility
        {
            get
            {
                var ret = (IsFinished) ? Visibility.Visible : Visibility.Collapsed;
                return ret;
            }
            set { }
        }

        #endregion

        #endregion
    }

    #endregion

    #region NExcelImport

    /// <summary>
    /// The NExcelImport class.
    /// </summary>
    public class NExcelImport : NInpc, IDisposable
    {
        #region Static Methods - Register License

        /// <summary>
        /// Register License.
        /// </summary>
        public static void RegisterLicense()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        #endregion

        #region Static Variables

        /// <summary>The DESKTOP path.</summary>
        public static string DESKTOP_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        #endregion

        #region Internal Variables

        private bool disposedValue; // flag for dispose

        private string _fileName = string.Empty;
        private List<NExcelWorksheet> _worksheets;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NExcelImport() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NExcelImport()
        {
            // Do not change this code.
            // Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);

            _worksheets = null;
        }

        #endregion

        #region Private Methods

        private void LoadWorksheets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (string.IsNullOrWhiteSpace(_fileName))
            {
                return;
            }

            if (null == _worksheets) _worksheets = new List<NExcelWorksheet>();
            _worksheets.Clear(); // clear exists worksheets

            using (var package = new ExcelPackage(_fileName))
            {
                try
                {
                    _worksheets = new List<NExcelWorksheet>();
                    var worksheets = package.Workbook.Worksheets;
                    foreach (var worksheet in worksheets)
                    {
                        // Create new NExcelWorksheet.
                        var nSheet = new NExcelWorksheet() { SheetName = worksheet.Name };
                        // Extract columns into new NExcelWorksheet.
                        if (null != worksheet &&
                            null != worksheet.Dimension &&
                            null != worksheet.Dimension.End &&
                            worksheet.Dimension.End.Column > 0)
                        {
                            // row always 1
                            int iRow = 1;
                            for (int iCol = 1; iCol <= worksheet.Dimension.End.Column; iCol++)
                            {
                                // get column value
                                var oVal = worksheet.Cells[iRow, iCol].Value;
                                if (null != oVal)
                                {
                                    var nColumn = new NExcelColumn()
                                    {
                                        ColumnName = oVal.ToString(),
                                        ColumnIndex = iCol,
                                        ColumnLetter = ExcelCellAddress.GetColumnLetter(iCol)
                                    };
                                    // Add to column list
                                    nSheet.Columns.Add(nColumn);
                                }
                            }
                        }
                        // Append to list.
                        _worksheets.Add(nSheet);
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                    try
                    {
                        if (null != package) package.Dispose();
                    }
                    catch
                    {
                        Console.WriteLine("package dispose error.");
                    }
                }
            }
        }

        #endregion

        #region protected Methods

        #region Dispose

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">True when in disposing process.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    if (null != _worksheets)
                    {
                        _worksheets.Clear();
                    }
                }
                // Free unmanaged resources (unmanaged objects) and override finalizer

                // Set large fields to null
                disposedValue = true;
            }
        }

        #endregion

        #endregion

        #region Public Methods

        #region Dispose

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Show Open Excel Dialog

        public bool ShowDialog(string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null)
        {
            return ShowDialog(null, title, initDir);
        }
        public bool ShowDialog(Window owner,
            string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null)
        {
            bool ret = false;

            // setup dialog options
            var od = new Microsoft.Win32.OpenFileDialog();
            od.Multiselect = false;
            od.InitialDirectory = initDir;
            od.Title = string.IsNullOrEmpty(title) ? "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล" : title;
            od.Filter = "Excel Files(*.xls, *.xlsx)|*.xls;*.xlsx";

            ret = od.ShowDialog(owner) == true;
            if (ret)
            {
                // assigned to FileName
                this.FileName = od.FileName;
            }
            od = null;

            return ret;
        }

        #endregion

        #endregion

        #region Public Properties

        #region For excel information

        /// <summary>
        /// Gets or sets Exel File Name (xls, xlsx).
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    LoadWorksheets();
                    this.Raise(() => this.FileName); // Raise ProeprtyChanged Event
                }
            }
        }
        /// <summary>
        /// Gets all worksheets.
        /// </summary>
        public List<NExcelWorksheet> Worksheets
        {
            get { return _worksheets; }
            set { }
        }


        #endregion

        #endregion
    }

    #endregion
}