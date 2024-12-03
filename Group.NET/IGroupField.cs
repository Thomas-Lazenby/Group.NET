using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroupField<TKey, TValue> : IGroupFieldReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        bool IsFieldsEmpty();

        int CountFields();

        void ClearFields();


        void AddField(TKey key, TValue value);

        bool TryAddField(TKey key, TValue value);

        void UpdateField(TKey key, TValue value);

        bool TryUpdateField(TKey key, TValue value);

        void RemoveField(TKey key);

        bool TryRemoveField(TKey key);
    }
}
