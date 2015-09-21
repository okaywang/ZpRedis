using RedisHelper;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest1
{
    [Serializable]
    public class PartabModel : IHashEntry
    {
        public const string PROPERTYNAME_GID = "gid";
        public const string PROPERTYNAME_PNUMBER = "pnumber";
        public const string PROPERTYNAME_MDATE = "mdate";

        public long gid { get; set; }
        public string pnumber { get; set; }
        public DateTime mdate { get; set; }

        public HashEntry[] GetHashEntries()
        {
            return new HashEntry[]
            {
             new HashEntry(PartabModel.PROPERTYNAME_GID,this.gid ),
             new HashEntry(PartabModel.PROPERTYNAME_PNUMBER,this.pnumber ),
             new HashEntry(PartabModel.PROPERTYNAME_MDATE,this.mdate.ToString() )
            };
        }
    }

    public class TestHelper
    {
        public static PartabModel[] GetTestData(int count)
        {
            var items = new List<PartabModel>();
            for (int i = 0; i < count; i++)
            {
                items.Add(new PartabModel
                {
                    gid = i,
                    mdate = DateTime.Now,
                    pnumber = "cc12007036"
                });
            }
            return items.ToArray();
        }
    }
}
