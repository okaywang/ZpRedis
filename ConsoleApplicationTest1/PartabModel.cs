using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest1
{
    public class PartabModel
    {
        public const string PROPERTYNAME_GID = "gid";
        public const string PROPERTYNAME_PNUMBER = "pnumber";
        public const string PROPERTYNAME_MDATE = "mdate";

        public long gid { get; set; }
        public string pnumber { get; set; }
        public DateTime mdate { get; set; }
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
