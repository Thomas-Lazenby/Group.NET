using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroupField<TKey, TValue> : IGroupFieldsReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        void AddField(TKey key, TValue value);

        bool TryAddField(TKey key, TValue value);

        void UpdateField(TKey key, TValue value);

        bool TryUpdateField(TKey key, TValue value);

        bool RemoveField(TKey key);

        bool TryRemoveField(TKey key);
    }
}
