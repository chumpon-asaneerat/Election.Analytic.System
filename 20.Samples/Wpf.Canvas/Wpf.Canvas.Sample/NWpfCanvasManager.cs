#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Shapes;

#endregion

namespace Wpf.Canvas.Sample
{
    public class NWpfCanvasManager
    {
        #region Internal Variables

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        private NWpfCanvasManager() : base() { }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="canvas">The target canvas.</param>
        public NWpfCanvasManager(System.Windows.Controls.Canvas canvas) : this()
        {
            this.Canvas = canvas;
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NWpfCanvasManager()
        {
            this.Canvas = null;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Target Canvas.
        /// </summary>
        public System.Windows.Controls.Canvas Canvas { get; protected set; }

        #endregion
    }
}
