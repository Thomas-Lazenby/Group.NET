using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroupParentChildrenGroups<TKey, TValue> : IGroupParentChildrenGroupsReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

        Group<TKey, TValue> CreateChildGroup(TKey key);

        bool TryCreateChildGroup(TKey key, out Group<TKey, TValue> group);

        Group<TKey, TValue> InsertChildGroup(TKey key, Group<TKey, TValue> group);

        bool TryInsertChildGroup(TKey key, Group<TKey, TValue> group);

        bool RemoveChildGroup(TKey key);

        bool TryRemoveChildGroup(TKey key, out Group<TKey, TValue>? group);

    }
}
