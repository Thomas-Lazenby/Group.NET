using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET.Group
{
    public interface IGroupGroupHierarchy<TKey, TValue> : IGroupGroupHierarchyReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

        Group<TKey, TValue> CreateChildGroup(TKey key);
        bool TryCreateChildGroup(TKey key, out Group<TKey, TValue> group);

        void InsertChildGroup(TKey key, Group<TKey, TValue> group);
        bool TryInsertChildGroup(TKey key, Group<TKey, TValue> group);
    }
}
