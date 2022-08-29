#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Reflection;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using NLib;
using Dapper;
using System.Windows.Media.Imaging;

using Newtonsoft.Json;

#endregion

namespace PPRP
{
    public class ByteUtils
    {
        public static byte[] GetFileBuffer(string fileName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            byte[] buffers = null;

            if (string.IsNullOrEmpty(fileName))
            {
                med.Err("No File Name assigned.");
                return buffers;
            }
            if (!File.Exists(fileName))
            {
                med.Err("File Not Found. File Name: {0}", fileName);
                return buffers;
            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        buffers = reader.ReadBytes((int)stream.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return buffers;
        }

        public static ImageSource GetImageSource(byte[] buffers)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            ImageSource ret = null;
            if (null == buffers)
            {
                med.Err("Buffer is null.");
                return ret;
            }
            if (buffers.Length <= 0)
            {
                med.Err("Buffer length is zero.");
                return ret;
            }

            try
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

                using (Stream stream = new MemoryStream(buffers))
                {
                    BitmapImage image = new BitmapImage();
                    stream.Position = 0;
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    image.Freeze();

                    ret = image;
                }
            }
            catch (Exception ex)
            {
                ret = null;
                //throw ex;
                med.Err(ex);
            }

            return ret;
        }

        public static string GetJsonString(byte[] buffers)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string ret = null;
            try
            {
                ret = System.Text.Encoding.UTF8.GetString(buffers);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return ret;
        }

        public static string FormatJson(string json)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string ret = string.Empty;
            try
            {
                // formatting json
                dynamic dJson = JsonConvert.DeserializeObject(json);
                ret = JsonConvert.SerializeObject(dJson, Formatting.Indented);
            }
            catch (Exception ex)
            {
                med.Err(ex);
                ret = json;
            }
            return ret;
        }
    }
}
