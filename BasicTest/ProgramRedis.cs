using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

[assembly: AssemblyVersion("1.0.0")]

namespace BasicTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test_Partab_Hash();
            //Test_Partab_KV();
            //Test_Task1();
            //SetASync();
            //NewMethod2();
            SetSync();
        }

        private static void SetASync()
        {
            var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();
            var sw = Stopwatch.StartNew();
            var count = 5;
            var tasks = new Task<bool>[count];
            Console.WriteLine("开始处理..." + count);
            for (int i = 0; i < count; i++)
            {
                try
                {
                    db.StringSet("name", "wgj");

                    //tasks[i] = db.StringSetAsync(i.ToString(), i);
                    //tasks[i].ContinueWith((t) =>
                    //{
                    //    if (!t.Result)
                    //    {
                    //        Console.WriteLine(t.Result);
                    //    }
                    //});
                    //System.Threading.Thread.Sleep(1000);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            try
            {
                Task.WaitAll(tasks);
            }
            catch (Exception ex)
            {

                throw;
            }

            var failedTask = tasks.Where(i => i.Result == false);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.ReadLine();
        }

        class KV
        {
            public string K { get; set; }

            public string V { get; set; }
        }

        private static void SetSync()
        {
            var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();

            var span = new TimeSpan(0, 10, 0);
            db.StringSet("age", 10, span);


            var span2 = new TimeSpan(0, 50, 0);
            db.StringSet("age", 10, span2);


        }

        private static void NewMethod4()
        {
            var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                db.StringSet(i.ToString(), i);
            }
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }
        private static void Test_Partab_Hash()
        {
            //var cnn = ConnectionMultiplexer.Connect("172.17.1.70:10001,172.17.1.70:10002,172.17.1.70:10003,172.17.1.70:10004,172.17.1.70:10005,172.17.1.70:10006");
            var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();

            var result = db.HashGetAll("gid_100");

            db.HashSet("gid_100", "rootid", 9999);




            result = db.HashGetAll("gid_100");

            var sw = Stopwatch.StartNew();
            long cid = 1200700000;
            long gid = 146165;

            var COUNT = 100;
            var rnd = new Random();

            var data = new Partab[COUNT];
            for (int i = 0; i < +data.Length; i++)
            {
                var d = new Partab
                {
                    cid = rnd.Next(1000000, int.MaxValue),
                    gid = i,
                    FeedbackType = rnd.Next(1000000, int.MaxValue),
                    foldid = rnd.Next(1000000, int.MaxValue),
                    is_active_for_company = "y",
                    is_temporary = "y",
                    LabelType = i,
                    mdate = DateTime.Now,
                    pid = rnd.Next(1000000, int.MaxValue),
                    pnumber = "CC000043927J90250331000",
                    PostType = i,
                    public_label = i,
                    resume_source_id = i,
                    rid = rnd.Next(1000000, int.MaxValue),
                    rnumber = "JM346841194R90250000000",
                    rootid = 12007000 + i,
                    rversion = i,
                    uid = rnd.Next(1000000, int.MaxValue)
                };

                data[i] = d;
            }

            var tasks = new Task<bool>[COUNT];
            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i];
                var json = JsonConvert.SerializeObject(item);
                tasks[i] = db.HashSetAsync("h_gid", item.gid.ToString(), json);
                //tasks[i] = db.StringSetAsync("gid_:" + item.gid.ToString(), json);
            }
            Task.WaitAll(tasks);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void Test_Partab_KV()
        {
            //var cnn = ConnectionMultiplexer.Connect("172.17.1.70:10001,172.17.1.70:10002,172.17.1.70:10003,172.17.1.70:10004,172.17.1.70:10005,172.17.1.70:10006");

            //var cnn = ConnectionMultiplexer.Connect("172.17.6.10:6379,172.17.6.11:6379,172.17.6.12:6379,172.17.6.13:6379,172.17.6.14:6379,172.17.6.15:6379");
            var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();
            long cid = 1200700000;
            long gid = 146165;

            var START = 0;
            var COUNT = 1000;
            var rnd = new Random();

            var data = new Partab[COUNT];
            for (int i = 0; i < data.Length; i++)
            {
                var d = new Partab
                {
                    cid = rnd.Next(1000000, int.MaxValue),
                    gid = START + i,
                    FeedbackType = rnd.Next(1000000, int.MaxValue),
                    foldid = rnd.Next(1000000, int.MaxValue),
                    is_active_for_company = "y",
                    is_temporary = "y",
                    LabelType = i,
                    mdate = DateTime.Now,
                    pid = rnd.Next(1000000, int.MaxValue),
                    pnumber = "CC000043927J90250331000",
                    PostType = i,
                    public_label = i,
                    resume_source_id = i,
                    rid = rnd.Next(1000000, int.MaxValue),
                    rnumber = "JM346841194R90250000000",
                    rootid = 12007000 + i,
                    rversion = i,
                    uid = rnd.Next(1000000, int.MaxValue)
                };

                data[i] = d;
            }

            var tasks = new Task<bool>[COUNT];
            var sw = Stopwatch.StartNew();
            var expireSpan = new TimeSpan(90, 0, 0, 0);
            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i];
                var json = JsonConvert.SerializeObject(item);
                tasks[i] = db.StringSetAsync("gid_:" + item.gid.ToString(), json, expireSpan);
                //db.StringSet("gid_:" + item.gid.ToString(), json, expireSpan);
            }
            try
            {

                Task.WaitAll(tasks);
                sw.Stop();
                cnn.Close();
            }
            catch (Exception ex)
            {


                var tt = tasks.Where(i => i.IsFaulted == true).ToList();
                foreach (var item in ex.Data.Keys)
                {
                    var o = ex.Data[item];
                    var test = o;
                }
                cnn.Close();
                throw;
            }

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void NewMethod2()
        {
            //var cnn = ConnectionMultiplexer.Connect("172.17.1.70:10001,172.17.1.70:10002,172.17.1.70:10003,172.17.1.70:10004,172.17.1.70:10005,172.17.1.70:10006");
            var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();
            var sw = Stopwatch.StartNew();
            long cid = 1200700000;
            long gid = 146165;

            var COUNT = 200000;
            var data = new Partab[COUNT];
            for (int i = 1200000 + 0; i < 1200000 + data.Length; i++)
            {
                var d = new Partab
                {
                    cid = i,
                    gid = i,
                    FeedbackType = i,
                    foldid = i,
                    is_active_for_company = "y",
                    is_temporary = "y",
                    LabelType = i,
                    mdate = DateTime.Now,
                    pid = i,
                    pnumber = "CC000043927J90250331000",
                    PostType = i,
                    public_label = i,
                    resume_source_id = i,
                    rid = i,
                    rnumber = "JM346841194R90250000000",
                    rootid = 12007000 + i,
                    rversion = i,
                    uid = i
                };

                data[i - 1200000] = d;
            }

            var tasks = new Task<bool>[COUNT];
            for (int i = 0; i < data.Length; i++)
            {
                var item = data[i];
                var json = JsonConvert.SerializeObject(item);
                tasks[i] = db.StringSetAsync(item.gid.ToString(), json);
            }
            Task.WaitAll(tasks);
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void Test_Task1()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse("172.17.1.70:10001,172.17.1.70:10002,172.17.1.70:10003,172.17.1.70:10004,172.17.1.70:10005,172.17.1.70:10006");
            //ConfigurationOptions options = ConfigurationOptions.Parse("172.17.1.88:10001,172.17.1.88:10002,172.17.1.88:10003,172.17.1.88:10004,172.17.1.88:10005,172.17.1.88:10006");
            //options.ConfigCheckSeconds = 3;

            var cnn = ConnectionMultiplexer.Connect(options);
            cnn.ConfigurationChanged += cnn_ConfigurationChanged;
            //var cnn = ConnectionMultiplexer.Connect("172.17.6.10:6379,172.17.6.11:6379,172.17.6.12:6379,172.17.6.13:6379,172.17.6.14:6379,172.17.6.15:6379");

            //var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();
            var sw = Stopwatch.StartNew();
            long cid = 1200700000;
            long gid = 146165;
            var resumeNumber = "JM346841194R90250000000";
            var tasks = new Task<bool>[1000000];

            var expireSpan = new TimeSpan(90, 0, 0, 0);
            for (int i = 0; i < 1000000; i++)
            {
                var compandId = cid + i;
                var key = string.Concat(compandId.ToString(), resumeNumber);
                try
                {
                    db.StringSet("test" + i.ToString(), i.ToString());
                }
                catch (Exception ex)
                {
                    cnn.Configure();
                    Console.WriteLine("ex---------------:" + i.ToString() + ex.Message);
                }

                //tasks[i] = db.StringSetAsync(key, gid + i, expireSpan);
                Console.WriteLine(i);
                System.Threading.Thread.Sleep(1000);
            }
            try
            {

                //Task.WaitAll(tasks);
            }
            catch (Exception ex)
            {

                throw;
            }
            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        static void cnn_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void Test_Task2()
        {
            //var cnn = ConnectionMultiplexer.Connect("172.17.1.70:10001,172.17.1.70:10002,172.17.1.70:10003,172.17.1.70:10004,172.17.1.70:10005,172.17.1.70:10006");
            //var cnn = ConnectionMultiplexer.Connect("172.17.6.10:6379,172.17.6.11:6379,172.17.6.12:6379,172.17.6.13:6379,172.17.6.14:6379,172.17.6.15:6379");
            var cnn = ConnectionMultiplexer.Connect("172.17.1.70:11111");
            var db = cnn.GetDatabase();
            var sw = Stopwatch.StartNew();
            long cid = 12007000;
            long gid = 146165;
            var resumeNumber = "JM346841194R90250000000";
            var tasks = new Task<bool>[5000000];
            int i = 0;
            try
            {
                var expireSpan = new TimeSpan(90, 0, 0, 0);
                for (; i < 5000000; i++)
                {
                    var compandId = cid + i;
                    var key = string.Concat(compandId.ToString(), resumeNumber);
                    //db.StringSet("test" + i.ToString(), i.ToString());
                    tasks[i] = db.StringSetAsync(key, i.ToString(), expireSpan);
                    //tasks[i] = db.KeyDeleteAsync(key);
                    //db.StringSet(key, gid + i, expireSpan);
                }



                Task.WaitAll(tasks);
            }
            catch (Exception ex)
            {
                foreach (var item in ex.Data.Keys)
                {
                    var o = ex.Data[item];
                    var test = o;
                }
                throw;
            }

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }

    public class Partab
    {
        public long gid { get; set; }
        public long rootid { get; set; }
        public long cid { get; set; }
        public long uid { get; set; }
        public long foldid { get; set; }
        public long pid { get; set; }
        public string pnumber { get; set; }
        public long rid { get; set; }
        public string rnumber { get; set; }
        public DateTime mdate { get; set; }
        public int rversion { get; set; }
        public string is_active_for_company { get; set; }
        public int resume_source_id { get; set; }
        public string is_temporary { get; set; }
        public int public_label { get; set; }
        public int PostType { get; set; }
        public int FeedbackType { get; set; }
        public int LabelType { get; set; }
    }


}
