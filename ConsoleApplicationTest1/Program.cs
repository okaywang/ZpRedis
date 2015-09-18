using Newtonsoft.Json;
using RedisHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest1
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = TestHelper.GetTestData(10000);

            var exp = new TimeSpan(0, 10, 0);
            //for (int i = 0; i < data.Length; i++)
            //{
            //    var item = data[i];
            //    RedisGroup.RdRedis.Set(item.gid.ToString(), JsonConvert.SerializeObject(item), exp);
            //}

            RedisGroup.RdRedis.Set<PartabModel>(data, (i) => i.gid.ToString(), exp);

        }
    }
}
