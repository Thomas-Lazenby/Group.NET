using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET.ConcurrentGroup
{
    public interface IConcurrentGroupField<TKey, TValue> : IGroupField<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        TValue GetOrAddField(TKey key, TValue value);
        TValue GetOrAddField(TKey key, Func<TKey, TValue> valueFactory);

        TValue AddOrUpdateField(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory);
        bool TryAddOrUpdateField(TKey key, TValue value, Func<TKey, TValue, TValue> updateValueFactory);

        bool TryUpdateField(TKey key, TValue value, TValue comparisonValue);

        ConcurrentGroup<TKey, TValue> GetFieldsSnapshot();
    }
}
