#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

using NLib;
using ShapeFileToSqlLite.ShapeFiles;
using ShapeFileToSqlLite.Models;

#endregion

namespace ShapeFileToSqlLite.Services
{
    /// <summary>
    /// JsonMapFiles Extension Methods class.
    /// </summary>
    public static class ShapeFileDbImport
    {
        public static void Import(this Shapefile shapefile)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == shapefile)
                return;

            foreach (Shape shape in shapefile)
            {
                var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
                var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
                var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
                var ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");

                if (null != ADM3_PCODE)
                {
                    ImportADM3(shape);
                }
                else if (null != ADM2_PCODE)
                {
                    ImportADM2(shape);
                }
                else if (null != ADM1_PCODE)
                {
                    ImportADM1(shape);
                }
                else if (null != ADM0_PCODE)
                {
                    ImportADM0(shape);
                }
                else
                {
                    Console.WriteLine("Invalid.");
                }
            }
        }

        private static void ImportADM0(Shape shape) 
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM0_EN = shape.GetMetadata("ADM0_EN");
            
            var row = new ADM0();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.CountryNameEN = ADM0_EN;

            var recordId = shape.RecordNumber; // shape number
            RectangleD boundRect = new RectangleD();

            #region Extract and convert shape part's points

            // cast shape based on the type
            switch (shape.Type)
            {
                case ShapeType.Point:
                    // a point is just a single x/y point
                    ShapePoint shapePoint = shape as ShapePoint;
                    {
                        ADM0Part admPart = ADM0Part.Get(row.ADM0Code, recordId).Value();
                        if (null == admPart) admPart = new ADM0Part();
                        admPart.ADM0Code = row.ADM0Code;
                        admPart.RecordId = recordId;
                        admPart.PointCount = 1;

                        ADM0Part.Save(admPart);

                        /*
                        // create new part
                        var jPart = new JsonShapePart();
                        jPart.Count = 1;
                        jPart.Type = MapShapeType.Point;
                        jshape.Parts.Add(jPart);

                        // create point.
                        jPart.Points = new double[1, 2];
                        jPart.Points[0, 0] = shapePoint.Point.X;
                        jPart.Points[0, 1] = shapePoint.Point.Y;
                        */

                        // Set Bound Rect
                        boundRect.Left = shapePoint.Point.X;
                        boundRect.Top = shapePoint.Point.Y;
                        boundRect.Right = shapePoint.Point.X;
                        boundRect.Bottom = shapePoint.Point.Y;
                    }
                    break;
                case ShapeType.Polygon:
                    // a polygon contains one or more parts - each part is a list of points which
                    // are clockwise for boundaries and anti-clockwise for holes 
                    // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                    ShapePolygon shapePolygon = shape as ShapePolygon;
                    {
                        foreach (PointD[] part in shapePolygon.Parts)
                        {
                            ADM0Part admPart = ADM0Part.Get(row.ADM0Code, recordId).Value();
                            if (null == admPart) admPart = new ADM0Part();
                            admPart.ADM0Code = row.ADM0Code;
                            admPart.RecordId = recordId;
                            admPart.PointCount = part.Length;

                            ADM0Part.Save(admPart);

                            foreach (PointD point in part)
                            {

                            }
                            /*
                            // create new part
                            var jPart = new JsonShapePart();
                            jPart.Count = part.Length;
                            jPart.Type = MapShapeType.Polygon;
                            jshape.Parts.Add(jPart);

                            // create points.
                            int iCnt = 0;
                            jPart.Points = new double[part.Length, 2];
                            foreach (PointD point in part)
                            {
                                // assign each point.
                                jPart.Points[iCnt, 0] = point.X;
                                jPart.Points[iCnt, 1] = point.Y;
                                iCnt++;
                            }
                            */
                        }
                    }
                    break;
                default:
                    // other not supports.
                    break;
            }
            // append to shape list.
            //file.Shapes.Add(jshape);

            // Set Bound Rect
            row.BoundLeft = boundRect.Left;
            row.BoundTop = boundRect.Top;
            row.BoundRight = boundRect.Right;
            row.BoundBottom = boundRect.Bottom;
            // Save General Information.
            ADM0.Save(row);

            #endregion
        }

        private static void ImportADM1(Shape shape) 
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM1_EN = shape.GetMetadata("ADM1_EN");

            var row = new ADM1();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ProvinceNameEN = ADM1_EN;

            var recordId = shape.RecordNumber; // shape number
            RectangleD boundRect = new RectangleD();


            // Set Bound Rect
            row.BoundLeft = boundRect.Left;
            row.BoundTop = boundRect.Top;
            row.BoundRight = boundRect.Right;
            row.BoundBottom = boundRect.Bottom;
            // Save General Information.
            ADM1.Save(row);
        }

        private static void ImportADM2(Shape shape) 
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
            var ADM2_EN = shape.GetMetadata("ADM2_EN");

            var row = new ADM2();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ADM2Code = ADM2_PCODE;
            row.DistrictNameEN = ADM2_EN;

            var recordId = shape.RecordNumber; // shape number
            RectangleD boundRect = new RectangleD();


            // Set Bound Rect
            row.BoundLeft = boundRect.Left;
            row.BoundTop = boundRect.Top;
            row.BoundRight = boundRect.Right;
            row.BoundBottom = boundRect.Bottom;
            // Save General Information.
            ADM2.Save(row);
        }

        private static void ImportADM3(Shape shape) 
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
            var ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");
            var ADM3_EN = shape.GetMetadata("ADM3_EN");

            var row = new ADM3();
            // Set Code
            row.ADM0Code = ADM0_PCODE;
            row.ADM1Code = ADM1_PCODE;
            row.ADM2Code = ADM2_PCODE;
            row.ADM3Code = ADM3_PCODE;
            row.SubdistrictNameEN = ADM3_EN;

            var recordId = shape.RecordNumber; // shape number
            RectangleD boundRect = new RectangleD();


            // Set Bound Rect
            row.BoundLeft = boundRect.Left;
            row.BoundTop = boundRect.Top;
            row.BoundRight = boundRect.Right;
            row.BoundBottom = boundRect.Bottom;
            // Save General Information.
            ADM3.Save(row);
        }
    }
}
