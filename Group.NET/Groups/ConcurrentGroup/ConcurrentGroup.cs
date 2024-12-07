using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group.NET.ConcurrentGroup;


namespace Group.NET
{
    [Obsolete("Not implemented yet")]
    public partial class ConcurrentGroup<TKey, TValue> : IConcurrentGroupField<TKey, TValue>, IConcurrentGroupHierarchy<TKey, TValue>
        where TKey : notnull, IEquatable<TKey>
    {
        

        protected ConcurrentDictionary<TKey, ConcurrentGroup<TKey, TValue>> _childGroups = new();
        protected ConcurrentDictionary<TKey, TValue> _fields = new();

        protected object lockObject = new();



        [Obsolete]
        protected ConcurrentDictionary<TKey, IValueType> _values = new();

        public bool ContainsKey(TKey key)
            => _values.ContainsKey(key);

        // TODO: Add to interface?
        public ValueType GetKeyType(TKey key)
        {
            if(!_values.TryGetValue(key, out var valueType))
            {
                throw new KeyNotFoundException($"No key '{key}' exists in ConcurrentGroup");
            }

            return valueType.Type;
        }

        // TODO: Add to interface?
        public void Clear()
            => _values.Clear();

    }
}
