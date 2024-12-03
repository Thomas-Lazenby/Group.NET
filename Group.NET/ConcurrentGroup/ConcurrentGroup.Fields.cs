using Group.NET.ConcurrentGroup;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public partial class ConcurrentGroup<TKey, TValue> : IConcurrentGroupField<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, TValue> _fields = new();

        #region IConcurrentGroupField<TKey,TValue>

        public TValue GetOrAddField(TKey key, TValue value)
            => _fields.GetOrAdd(key, value);

        public TValue GetOrAddField(TKey key, Func<TKey, TValue> valueFactory)
            => _fields.GetOrAdd(key, valueFactory);

        public TValue AddOrUpdateField(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
            => _fields.AddOrUpdate(key, addValue, updateValueFactory);

        public bool TryAddOrUpdateField(TKey key, TValue value, Func<TKey, TValue, TValue> updateValueFactory)
            => _fields.AddOrUpdate(key, value, updateValueFactory) != null;

        public bool TryUpdateField(TKey key, TValue value, TValue comparisonValue)
            => _fields.TryUpdate(key, value, comparisonValue);

        public ConcurrentGroup<TKey, TValue> GetFieldsSnapshot()
        {
            var snapshot = new ConcurrentGroup<TKey, TValue>();

            foreach (var kvp in _fields)
            {
                snapshot.AddField(kvp.Key, kvp.Value);
            }

            return snapshot;
        }


        #endregion

        #region IGroupField<TKey, TValue> 

        public void ClearFields()
            => _fields.Clear();

        public void AddField(TKey key, TValue value)
        {
            if (!_fields.TryAdd(key, value))
            {
                throw new InvalidOperationException($"Key: {key} already exists in fields.");
            }
        }

        public bool TryAddField(TKey key, TValue value)
            => _fields.TryAdd(key, value);

        public void UpdateField(TKey key, TValue value)
        {
            if (!_fields.ContainsKey(key))
            {
                throw new KeyNotFoundException($"No field found with key {key}.");
            }
            _fields[key] = value;
        }

        public bool TryUpdateField(TKey key, TValue value)
        {
            if (_fields.TryGetValue(key, out TValue existingValue))
            {
                return _fields.TryUpdate(key, value, existingValue);
            }
            return false;
        }

        public void RemoveField(TKey key)
        {
            if (!_fields.TryRemove(key, out _))
            {
                throw new KeyNotFoundException($"No field found with key {key}.");
            }
        }

        public bool TryRemoveField(TKey key)
            => _fields.TryRemove(key, out _);

        #endregion

        #region IGroupFieldReadOnly<TKey,TValue>

        public bool IsFieldsEmpty()
            => _fields.IsEmpty;

        public int CountFields()
            => _fields.Count;

        public IEnumerable<TKey> GetKeysField()
            => _fields.Keys;

        public bool ExistsField(TKey key)
            => _fields.ContainsKey(key);

        public T GetField<T>(TKey key)
            where T : TValue
        {
            if (_fields.TryGetValue(key, out var untypedValue))
            {
                if (untypedValue is T typedValue)
                {
                    return typedValue;
                }
                throw new InvalidCastException($"Field with key {key} exists but cannot be cast to type {typeof(T).Name}. Actual type: {untypedValue?.GetType().Name ?? "null"}.");
            }
            throw new KeyNotFoundException($"No field found with key {key}.");
        }


        public bool TryGetField<T>(TKey key, out T? value)
            where T : TValue
        {
            if (_fields.TryGetValue(key, out var untypedValue) && untypedValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        #endregion
    }
}
