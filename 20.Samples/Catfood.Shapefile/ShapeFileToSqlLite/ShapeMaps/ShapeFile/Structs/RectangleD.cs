﻿/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

#region Using

using System;

#endregion

namespace ShapeFileToSqlLite.ShapeFiles
{
    #region RectangleD Struct

    /// <summary>
    /// A simple double precision rectangle
    /// </summary>
    public struct RectangleD
    {
        #region Public Fields

        /// <summary>Gets or sets the left value</summary>
        public double Left;
        /// <summary>Gets or sets the top value</summary>
        public double Top;
        /// <summary>Gets or sets the right value</summary>
        public double Right;
        /// <summary>Gets or sets the bottom value</summary>
        public double Bottom;

        #endregion

        #region Constructor

        /// <summary>
        /// A simple double precision rectangle
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="top">Top</param>
        /// <param name="right">Right</param>
        /// <param name="bottom">Bottom</param>
        public RectangleD(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        #endregion
    }

    #endregion
}
