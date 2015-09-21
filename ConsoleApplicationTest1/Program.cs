using Newtonsoft.Json;
using RedisHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = TestHelper.GetTestData(1000000);

            var exp = new TimeSpan(0, 10, 0);

            //for (int i = 0; i < data.Length; i++)
            //{
            //    var item = data[i];
            //    RedisGroup.RdRedis.Set(item.gid.ToString(), JsonConvert.SerializeObject(item), exp);
            //}

            var model = data.First();
            var tt = JsonConvert.SerializeObject(model);

            var i9 = StringCompressor.CompressString(tt);
            var i8 = StringCompressor.DecompressString(i9);

            var fomatter = new BinaryFormatter();

            var ms = new MemoryStream();
            fomatter.Serialize(ms, model);
            var bytes = ms.ToArray();
            ms.Position = 0;
            var ddadfadf = (PartabModel)fomatter.Deserialize(ms);

            var sss1 = System.Text.Encoding.ASCII.GetString(ms.ToArray());
            string hex = BitConverter.ToString(bytes);
            var dd = Convert.ToBase64String(bytes);


            var sw = System.Diagnostics.Stopwatch.StartNew();
            //RedisGroup.RdRedis.HashSet<PartabModel>(data, (i) => i.gid.ToString(), exp);
            RedisGroup.RdRedis.Set<PartabModel>(data, (i) => i.gid.ToString(), exp);
            Console.WriteLine("time spent:" + sw.ElapsedMilliseconds);
        }
    }
    internal static class StringCompressor
    {
        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}

