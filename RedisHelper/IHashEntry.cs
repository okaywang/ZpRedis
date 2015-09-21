using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisHelper
{


    public interface IHashEntry
    {
        HashEntry[] GetHashEntries();

    }
}
