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
        #region IConcurrentGroupField<TKey,TValue>

        public TValue GetOrAddField(TKey key, TValue value)
        {
            IValueType result = _values.GetOrAdd(
                key,
                k => new Value<TValue>
                {
                    Type = ValueType.Field,
                    Data = value
                }
            );

            if (result.Type != ValueType.Field)
            {
                throw new InvalidOperationException($"Key '{key}' already exists as a {result.Type}.");
            }

            if (result is Value<TValue> { Data: var data })
            {
                return data;
            }
            else
                throw new InvalidCastException($"Failed to cast Data for key '{key}'. Expected type: {typeof(TValue).Name}, Actual type: {result.GetType().Name}.");
        }


        public TValue GetOrAddField(TKey key, Func<TKey, TValue> valueFactory)
        {
            throw new NotImplementedException();
        }

        public TValue AddOrUpdateField(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            IValueType result = _values.AddOrUpdate(
                key,
                k => new Value<TValue>
                {
                    Type = ValueType.Field,
                    Data = addValue
                },
                (k, existing) =>
                {
                    if (existing.Type != ValueType.Field)
                    {
                        throw new InvalidOperationException($"Key '{key}' already exists as a {existing.Type}.");
                    }

                    var existingValue = (Value<TValue>)existing;
                    existingValue.Data = updateValueFactory(k, existingValue.Data);
                    return existingValue;
                }
            );

            // Do as a implicit cast instead.
            return ((Value<TValue>)result).Data;
        }


        public bool TryAddOrUpdateField(TKey key, TValue value, Func<TKey, TValue, TValue> updateValueFactory)
        {
            try
            {
                AddOrUpdateField(key, value, updateValueFactory);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool TryUpdateField(TKey key, TValue value, TValue comparisonValue)
        {
            if (_values.TryGetValue(key, out var existing) && existing.Type == ValueType.Field)
            {
                var existingValue = (Value<TValue>)existing;

                if (EqualityComparer<TValue>.Default.Equals(existingValue.Data, comparisonValue))
                {
                    var newValue = new Value<TValue>
                    {
                        Type = ValueType.Field,
                        Data = value
                    };

                    return _values.TryUpdate(key, newValue, existing);
                }
            }

            return false;
        }


        public ConcurrentGroup<TKey, TValue> GetFieldsSnapshot()
        {
            var snapshot = new ConcurrentGroup<TKey, TValue>();

            foreach (var kvp in _values.Where(kvp => kvp.Value.Type == ValueType.Field))
            {
                var field = (Value<TValue>)kvp.Value;
                snapshot.AddField(kvp.Key, field.Data);
            }

            return snapshot;
        }



        #endregion

        #region IGroupField<TKey, TValue> 

        public void ClearFields()
        {
            foreach (var key in _values.Where(kvp => kvp.Value.Type == ValueType.Field).Select(kvp => kvp.Key).ToList())
            {
                _values.TryRemove(key, out _);
            }
        }


        public void AddField(TKey key, TValue value)
        {
            var newValue = new Value<TValue>
            {
                Type = ValueType.Field,
                Data = value
            };

            if (!_values.TryAdd(key, newValue))
            {
                throw new InvalidOperationException($"Key '{key}' already exists as a {_values[key].Type}.");
            }
        }



        public bool TryAddField(TKey key, TValue value)
        {
            var newValue = new Value<TValue>
            {
                Type = ValueType.Field,
                Data = value
            };

            return _values.TryAdd(key, newValue);
        }


        public void UpdateField(TKey key, TValue value)
        {
            if (_values.TryGetValue(key, out var existing) && existing.Type == ValueType.Field)
            {
                var updatedValue = new Value<TValue>
                {
                    Type = ValueType.Field,
                    Data = value
                };

                if (!_values.TryUpdate(key, updatedValue, existing))
                {
                    throw new InvalidOperationException($"Failed to update field with key '{key}'.");
                }
            }
            else
            {
                throw new KeyNotFoundException($"No field found with key '{key}'.");
            }
        }


        public bool TryUpdateField(TKey key, TValue value)
        {
            if (_values.TryGetValue(key, out var existing) && existing.Type == ValueType.Field)
            {
                var updatedValue = new Value<TValue>
                {
                    Type = ValueType.Field,
                    Data = value
                };

                return _values.TryUpdate(key, updatedValue, existing);
            }

            return false;
        }


        public void RemoveField(TKey key)
        {
            if (!_values.TryRemove(key, out var existing) || existing.Type != ValueType.Field)
            {
                throw new KeyNotFoundException($"No field found with key '{key}'.");
            }
        }


        public bool TryRemoveField(TKey key)
            => _values.TryRemove(key, out var existing) && existing.Type == ValueType.Field;

        #endregion

        #region IGroupFieldReadOnly<TKey,TValue>

        public bool IsFieldsEmpty()
            => !_values.Any(kvp => kvp.Value.Type == ValueType.Field);

        public int CountFields()
            => _values.Count(kvp => kvp.Value.Type == ValueType.Field);

        public IEnumerable<TKey> GetKeysField()
            => _values.Where(kvp => kvp.Value.Type == ValueType.Field).Select(kvp => kvp.Key);

        public bool ExistsField(TKey key)
            => _values.TryGetValue(key, out var existing) && existing.Type == ValueType.Field;

        public T GetField<T>(TKey key)
            where T : TValue
        {
            if (_values.TryGetValue(key, out var existing) && existing is Value<TValue> field && field.Type == ValueType.Field)
            {
                if (field.Data is T typedValue)
                {
                    return typedValue;
                }

                throw new InvalidCastException($"Field with key '{key}' exists but cannot be cast to type {typeof(T).Name}. Actual type: {field.Data?.GetType().Name ?? "null"}.");
            }

            throw new KeyNotFoundException($"No field found with key '{key}'.");
        }

        public bool TryGetField<T>(TKey key, out T? value)
            where T : TValue
        {
            if (_values.TryGetValue(key, out var existing) && existing is Value<TValue> field && field.Type == ValueType.Field)
            {
                if (field.Data is T typedValue)
                {
                    value = typedValue;
                    return true;
                }
            }

            value = default;
            return false;
        }


        #endregion
    }
}
