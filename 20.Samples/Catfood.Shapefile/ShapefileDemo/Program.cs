#define USE_ARRAY

/* ------------------------------------------------------------------------
 * (c)copyright 2009-2019 Robert Ellison and contributors - https://github.com/abfo/shapefile
 * Provided under the ms-PL license, see LICENSE.txt
 * ------------------------------------------------------------------------ */

using System;
using System.IO;
using PPRP;
using PPRP.Models.Maps;
using PPRP.Imports.ShapeFiles;
using PPRP.Exports.Maps;

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

                //shapefile.Export(@"./output");
                shapefile.Export(@"D:\Maps");
            }

            Console.WriteLine("Done");
            Console.WriteLine();
        }
    }
}
