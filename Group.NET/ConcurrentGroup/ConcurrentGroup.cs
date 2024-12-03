using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group.NET.ConcurrentGroup;


namespace Group.NET
{
    public partial class ConcurrentGroup<TKey, TValue> : IConcurrentGroupField<TKey, TValue>, IConcurrentGroupHierarchy<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        public bool ContainsKey(TKey key)
            => _fields.ContainsKey(key) || _childrenGroups.ContainsKey(key);

    }
}
