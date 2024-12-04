using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET.ConcurrentGroup
{
    public interface IConcurrentGroupHierarchy<TKey, TValue> : IConcurrentGroupHierarchyReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        ConcurrentGroup<TKey, TValue> CreateChildGroup(TKey key);
        bool TryCreateChildGroup(TKey key, out ConcurrentGroup<TKey, TValue> group);

        void InsertChildGroup(TKey key, ConcurrentGroup<TKey, TValue> group);
        bool TryInsertChildGroup(TKey key, ConcurrentGroup<TKey, TValue> group);

    }
}
