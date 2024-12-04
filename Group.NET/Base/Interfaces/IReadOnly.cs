using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        bool ContainsKey(TKey key);
    }
}
