using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroupSubGroup<TKey, TValue> : IGroupSubGroupReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

        Group<TKey, TValue> CreateSubGroup(TKey key);

        bool TryCreateSubGroup(TKey key, out Group<TKey, TValue> group);

        Group<TKey, TValue> InsertSubGroup(TKey key, Group<TKey, TValue> group);

        bool TryInsertSubGroup(TKey key, Group<TKey, TValue> group);

        bool RemoveSubGroup(TKey key);

        bool TryRemoveSubGroup(TKey key, out Group<TKey, TValue>? group);

    }
}
