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
        }

        private static void ImportADM1(Shape shape) 
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");

            var ADM0_EN = shape.GetMetadata("ADM0_EN");
            var ADM1_EN = shape.GetMetadata("ADM1_EN");
        }

        private static void ImportADM2(Shape shape) 
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");

            var ADM0_EN = shape.GetMetadata("ADM0_EN");
            var ADM1_EN = shape.GetMetadata("ADM1_EN");
            var ADM2_EN = shape.GetMetadata("ADM2_EN");
        }

        private static void ImportADM3(Shape shape) 
        {
            if (null == shape) return;
            var ADM0_PCODE = shape.GetMetadata("ADM0_PCODE");
            var ADM1_PCODE = shape.GetMetadata("ADM1_PCODE");
            var ADM2_PCODE = shape.GetMetadata("ADM2_PCODE");
            var ADM3_PCODE = shape.GetMetadata("ADM3_PCODE");

            var ADM0_EN = shape.GetMetadata("ADM0_EN");
            var ADM1_EN = shape.GetMetadata("ADM1_EN");
            var ADM2_EN = shape.GetMetadata("ADM2_EN");
            var ADM3_EN = shape.GetMetadata("ADM3_EN");
        }
    }
}
