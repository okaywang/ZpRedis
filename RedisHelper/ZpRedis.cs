using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelper
{
    public class RedisGroup
    {
        public static ZpRedis RdRedis = new ZpRedis("172.17.1.70:11111");
        //public static ZpRedis Resume = new ZpRedis("172.17.1.70:10001,172.17.1.70:10002,172.17.1.70:10003,172.17.1.70:10004,172.17.1.70:10005,172.17.1.70:10006");
    }

    public class ZpRedis
    {
        private ConnectionMultiplexer _multiplexer;
        private IDatabase _db;
        private string _configuration;
        public ZpRedis(string configuration)
        {
            _configuration = configuration;
            Configure();
        }

        public void Set(string key, string value, TimeSpan? timespan = null)
        {
            _db.StringSet(key, value, timespan);
        }

        public void Set<T>(T[] items, Func<T, string> keySelector, TimeSpan? timespan = null) where T : class
        {
            var tasks = new Task<bool>[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                var key = keySelector(item);
                tasks[i] = _db.StringSetAsync(key, JsonConvert.SerializeObject(item), timespan);
            }
            Task.WaitAll(tasks);
        }

        public void HashSet(string key, HashEntry[] hashFields, TimeSpan? timespan = null)
        {
            _db.HashSet(key, hashFields);
            if (timespan.HasValue)
            {
                _db.KeyExpire(key, timespan);
            }
        }



        public void set2(KeyValuePair<string, string>[] pairs, TimeSpan? timespan = null)
        {
            var tasks = new Task[pairs.Length];
            for (int i = 0; i < pairs.Length; i++)
            {
                tasks[i] = _db.StringSetAsync(pairs[i].Key, pairs[i].Value, timespan).ContinueWith(test);
            }
            try
            {
                Task.WaitAll(tasks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void test(Task<bool> task)
        {
            var flg = task.Result;
            if (flg == false)
            {

            }
        }

        public string get(string key)
        {
            return _db.StringGet(key);
        }

        private void Configure()
        {
            _multiplexer = ConnectionMultiplexer.Connect(_configuration);
            _db = _multiplexer.GetDatabase();
        }


    }
}
