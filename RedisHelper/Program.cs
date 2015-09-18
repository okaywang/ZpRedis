using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            NewMethod();
        }

        private static void NewMethod12()
        {
            long cid = 12007000;
            long gid = 146165;
            var resumeNumber = "JM346841194R90250000000";
            var tasks = new Task<bool>[1000000];
            var kvs = new KeyValuePair<string, string>[1000000];
            int i = 0;

            var expireSpan = new TimeSpan(90, 0, 0, 0);
            for (; i < 1000000; i++)
            {
                var compandId = cid + i;
                var key = string.Concat(compandId.ToString(), resumeNumber);

                try
                {
                    System.Threading.Thread.Sleep(1000);
                    RedisGroup.RdRedis.Set(key, i.ToString());

                    Console.WriteLine("success:" + i.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("____________________________________________" + i.ToString());
                }
                 
            }
        }

        private static void NewMethod()
        {
            long cid = 12007000;
            long gid = 146165;
            var resumeNumber = "JM346841194R90250000000";
            var tasks = new Task<bool>[500000];
            var kvs = new KeyValuePair<string, string>[100000];
            int i = 0;

            var expireSpan = new TimeSpan(90, 0, 0, 0);
            for (; i < 100000; i++)
            {
                var compandId = cid + i;
                var key = string.Concat(compandId.ToString(), resumeNumber);
                kvs[i] = new KeyValuePair<string, string>(key, i.ToString());
            }


            RedisGroup.RdRedis.set2(kvs);
        }
    }
}
