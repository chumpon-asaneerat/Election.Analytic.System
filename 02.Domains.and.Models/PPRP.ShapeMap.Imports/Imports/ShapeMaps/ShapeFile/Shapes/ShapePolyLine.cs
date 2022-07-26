﻿/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;

#endregion

namespace PPRP.Imports.ShapeFiles
{
    #region ShapePolyLine

    /// <summary>
    /// A Shapefile PolyLine  Shape
    /// </summary>
    public class ShapePolyLine : Shape
    {
        #region Internal Variables

        /// <summary>Bounding Box</summary>
        internal RectangleD _boundingBox;
        /// <summary>List of parts</summary>
        internal List<PointD[]> _parts;

        #endregion

        #region Constructor

        /// <summary>
        /// A Shapefile PolyLine Shape
        /// </summary>
        /// <param name="recordNumber">The record number in the Shapefile</param>
        /// <param name="metadata">Metadata about the shape</param>
        /// <param name="dataRecord">IDataRecord associated with the metadata</param>
        /// <param name="shapeData">The shape record as a byte array</param>
        /// <exception cref="ArgumentNullException">Thrown if shapeData is null</exception>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs parsing shapeData</exception>
        protected internal ShapePolyLine(int recordNumber, StringDictionary metadata
            , IDataRecord dataRecord, byte[] shapeData)
            : base(ShapeType.PolyLine, recordNumber, metadata, dataRecord)
        {
            ParsePolyLineOrPolygon(shapeData, out _boundingBox, out _parts);
        }
        /// <summary>
        /// A Shapefile PolyLine Shape
        /// </summary>
        /// <param name="recordNumber">The record number in the Shapefile</param>
        /// <param name="metadata">Metadata about the shape</param>        
        /// <param name="dataRecord">IDataRecord associated with the metadata</param>
        protected internal ShapePolyLine(int recordNumber, StringDictionary metadata
            , IDataRecord dataRecord)
            : base(ShapeType.PolyLine, recordNumber, metadata, dataRecord) { }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the bounding box
        /// </summary>
        public RectangleD BoundingBox { get { return _boundingBox; } }

        /// <summary>
        /// Gets a list of parts (segments) for the PolyLine. Each part
        /// is an array of double precision points
        /// </summary>
        public List<PointD[]> Parts { get { return _parts; } }

        #endregion
    }

    #endregion
}
