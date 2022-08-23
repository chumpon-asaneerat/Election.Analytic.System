#define USE_ARRAY

/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

using System;
using System.Collections.Generic;
using System.IO;
using PPRP;
using PPRP.Models.Maps;
using PPRP.Imports.ShapeFiles;
using Newtonsoft.Json;

namespace ShapefileDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pass the path to the shapefile in as the command line argument
            if ((args.Length == 0) || (!File.Exists(args[0])))
            {
                Console.WriteLine("Usage: ShapefileDemo <shapefile.shp>");
                return;
            }

            // construct shapefile with the path to the .shp file
            using (Shapefile shapefile = new Shapefile(args[0]))
            {
                Console.WriteLine("ShapefileDemo Dumping {0}", args[0]);
                Console.WriteLine();

                // enumerate all shapes
                foreach (Shape shape in shapefile)
                {
                    JsonShapeFile file = new JsonShapeFile();
                    file.Type = (MapShapeType)shapefile.Type;
                    file.Count = shapefile.Count;
                    file.Bound.Left = shapefile.BoundingBox.Left;
                    file.Bound.Top = shapefile.BoundingBox.Top;
                    file.Bound.Right = shapefile.BoundingBox.Right;
                    file.Bound.Bottom = shapefile.BoundingBox.Bottom;

                    JsonShape jshape = new JsonShape();
                    jshape.RecordNo = shape.RecordNumber;
                    jshape.ShapeType = (MapShapeType)shape.Type;
                    jshape.Area = Convert.ToDouble(shape.GetMetadata("SHAPE_AREA"));
                    jshape.Length = Convert.ToDouble(shape.GetMetadata("SHAPE_LENG"));

                    jshape.ADM0_EN = shape.GetMetadata("ADM0_EN");
                    jshape.ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
                    jshape.ADM1_EN = shape.GetMetadata("ADM1_EN");
                    jshape.ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
                    jshape.ADM2_EN = shape.GetMetadata("ADM2_EN");
                    jshape.ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
                    jshape.ADM3_EN = shape.GetMetadata("ADM3_EN");
                    jshape.ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");

                    // cast shape based on the type
                    switch (shape.Type)
                    {
                        case ShapeType.Point:
                            // a point is just a single x/y point
                            ShapePoint shapePoint = shape as ShapePoint;
                            {
                                //Console.WriteLine("Point={0},{1}", shapePoint.Point.X, shapePoint.Point.Y);
                                var jPart = new JsonShapePart();
                                jPart.Count = 1;
                                jPart.Type = MapShapeType.Point;
                                jshape.Parts.Add(jPart);

                                jPart.Points = new double[1, 2];
                                jPart.Points[0, 0] = shapePoint.Point.X;
                                jPart.Points[0, 1] = shapePoint.Point.Y;
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
                                    var jPart = new JsonShapePart();
                                    jPart.Count = part.Length;
                                    jPart.Type = MapShapeType.Polygon;
                                    jshape.Parts.Add(jPart);

                                    //Console.WriteLine("Polygon part:");
                                    int iCnt = 0;
                                    jPart.Points = new double[part.Length, 2];
                                    foreach (PointD point in part)
                                    {
                                        //Console.WriteLine("{0}, {1}", point.X, point.Y);
                                        jPart.Points[iCnt, 0] = point.X;
                                        jPart.Points[iCnt, 1] = point.Y;
                                        iCnt++;
                                    }
                                    //Console.WriteLine();
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    file.Shapes.Add(jshape);

                    string path = "./output";
                    if (!Directory.Exists(path))
                    {
                        try 
                        {
                            Directory.CreateDirectory(path);
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine(ex);
                        }                        
                    }

                    string fileName = string.Empty;
                    fileName += jshape.ADM0_EN;
                    fileName += string.IsNullOrWhiteSpace(jshape.ADM1_EN) ? string.Empty : "." + jshape.ADM1_EN;
                    fileName += string.IsNullOrWhiteSpace(jshape.ADM2_EN) ? string.Empty : "." + jshape.ADM2_EN;
                    fileName += string.IsNullOrWhiteSpace(jshape.ADM3_EN) ? string.Empty : "." + jshape.ADM3_EN;

                    file.SaveToFile(path + "/" + fileName + ".json", true);
                }
            }

            Console.WriteLine("Done");
            Console.WriteLine();
        }
    }
}
