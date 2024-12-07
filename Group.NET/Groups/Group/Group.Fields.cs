using Group.NET.Group.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Group.NET
{
    public partial class Group<TKey, TValue> : IField<TKey, TValue>, IGroupFieldReadOnly<TKey,TValue>
        where TKey : IEquatable<TKey>
    {
        private readonly IDictionary<TKey, TValue> _fields;

        #region IGroupFieldReadOnly<TKey,TValue>

        [Obsolete("Not implemented")]
        public IDictionary<TKey, TValue> GetFieldsSnapshot()
            => throw new NotImplementedException();

        #endregion

        #region IField<TKey, TValue> 

        public void ClearFields()
            => _fields.Clear();


        public void AddField(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new InvalidOperationException($"Key '{key}' already exists.");
            }

            _fields[key] = value;
        }



        public bool TryAddField(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                return false;
            }

            _fields[key] = value;
            return true;
        }


        public void UpdateField(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                throw new KeyNotFoundException($"No field found with key '{key}'.");
            }

            _fields[key] = value;
        }


        public bool TryUpdateField(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                return false;
            }

            _fields[key] = value;
            return true;
        }


        public void RemoveField(TKey key)
        {
            if (!_fields.Remove(key))
            {
                throw new KeyNotFoundException($"No field found with key '{key}'.");
            }
        }


        public bool TryRemoveField(TKey key)
            => _fields.Remove(key);

        #endregion

        #region IFieldReadOnly<TKey,TValue>

        public bool IsFieldsEmpty()
            => !_fields.Any();

        public int CountFields()
            => _fields.Count;

        public IEnumerable<TKey> GetKeysField()
            => _fields.Keys;

        public bool ExistsField(TKey key)
            => _fields.ContainsKey(key);

        public T GetField<T>(TKey key)
            where T : TValue
        {
            if (_fields.TryGetValue(key, out var value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }

                throw new InvalidCastException($"Field with key '{key}' exists but cannot be cast to type {typeof(T).Name}. Actual type: {value?.GetType().Name ?? "null"}.");
            }

            throw new KeyNotFoundException($"No field found with key '{key}'.");
        }

        public bool TryGetField<T>(TKey key, out T? value)
            where T : TValue
        {
            if (_fields.TryGetValue(key, out var fieldValue) && fieldValue is T typedValue)
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
