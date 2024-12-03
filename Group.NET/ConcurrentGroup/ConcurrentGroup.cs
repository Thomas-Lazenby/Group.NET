using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group.NET.ConcurrentGroup;


namespace Group.NET
{
    public sealed class ConcurrentGroup<TKey, TValue> : IConcurrentGroupField<TKey, TValue>, IConcurrentGroupHierarchyGroups<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        #region Hierarchy

        private readonly ConcurrentDictionary<TKey, ConcurrentGroup<TKey, TValue>> _childrenGroups = new();

        private readonly object _parentGroupLock = new();
        private ConcurrentGroup<TKey, TValue>? _parentGroup;

        public ConcurrentGroup<TKey, TValue>? ParentGroup
        {
            get
            {
                lock (_parentGroupLock)
                {
                    return _parentGroup;
                }
            }
            private set
            {
                lock (_parentGroupLock)
                {
                    _parentGroup = value;
                }
            }
        }

        public bool IsRootGroup
        {
            get
            {
                lock (_parentGroupLock)
                {
                    return _parentGroup == null;
                }
            }
        }

        public ConcurrentGroup<TKey, TValue> GetRootGroup()
        {
            var currentGroup = this;

            while (currentGroup.ParentGroup != null)
            {
                currentGroup = currentGroup.ParentGroup;
            }

            return currentGroup;
        }

        public bool IsChildGroupsEmpty()
            => _childrenGroups.IsEmpty;

        public int CountChildGroups()
            => _childrenGroups.Count;

        public void ClearChildGroups()
            => _childrenGroups.Clear();

        public ConcurrentGroup<TKey, TValue> CreateChildGroup(TKey key)
        {
            if (!TryCreateChildGroup(key, out var newGroup))
            {
                throw new InvalidOperationException($"A child group with key {key} already exists.");
            }
            return newGroup;
        }

        public bool TryCreateChildGroup(TKey key, out ConcurrentGroup<TKey, TValue> group)
        {
            group = new ConcurrentGroup<TKey, TValue>
            {
                ParentGroup = this
            };

            return _childrenGroups.TryAdd(key, group);
        }


        public void InsertChildGroup(TKey key, ConcurrentGroup<TKey, TValue> group)
        {

            if (_childrenGroups.TryAdd(key, group))
            {
                group.ParentGroup = this;
            }
            else
            {
                throw new InvalidOperationException($"A child group with key {key} already exists.");
            }
        }

        public bool TryInsertChildGroup(TKey key, ConcurrentGroup<TKey, TValue> group)
        {
            // Make this not set to parentgroup unless adeded
            group.ParentGroup = this;
            return _childrenGroups.TryAdd(key, group);
        }

        public void RemoveChildGroup(TKey key)
        {
            if (!TryRemoveChildGroup(key))
            {
                throw new KeyNotFoundException($"No child group found with key {key}.");
            }
        }



        public bool TryRemoveChildGroup(TKey key)
            => _childrenGroups.TryRemove(key, out _);

        public IEnumerable<TKey> GetKeysForChildrenGroups()
            => _childrenGroups.Keys;

        public ConcurrentGroup<TKey, TValue> GetChildGroup(TKey key)
        {
            if (!_childrenGroups.TryGetValue(key, out var group))
            {
                throw new KeyNotFoundException($"No child group found with key {key}.");
            }

            return group;
        }

        public bool TryGetChildGroup(TKey key, out ConcurrentGroup<TKey, TValue>? group)
            => _childrenGroups.TryGetValue(key, out group);

        public bool ExistsChildGroup(TKey key)
            => _childrenGroups.ContainsKey(key);

        #endregion

        #region Fields

        private readonly ConcurrentDictionary<TKey, TValue> _fields = new();

        #region IGroupField<TKey, TValue>

        public bool IsFieldsEmpty()
            => _fields.IsEmpty;

        public int CountFields()
            => _fields.Count;

        public void ClearFields()
            => _fields.Clear();



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

        public IEnumerable<TKey> GetKeysField()
            => _fields.Keys;

        public void RemoveField(TKey key)
        {
            if (!_fields.TryRemove(key, out _))
            {
                throw new KeyNotFoundException($"No field found with key {key}.");
            }
        }

        public bool TryRemoveField(TKey key)
            => _fields.TryRemove(key, out _);

        public void AddField(TKey key, TValue value)
        {
            if (!_fields.TryAdd(key, value))
            {
                throw new InvalidOperationException($"Key: {key} already exists in fields.");
            }
        }


        public bool TryAddField(TKey key, TValue value)
            => _fields.TryAdd(key, value);

        public bool TryUpdateField(TKey key, TValue value)
        {
            if (_fields.TryGetValue(key, out TValue existingValue))
            {
                return _fields.TryUpdate(key, value, existingValue);
            }
            return false;
        }


        public void UpdateField(TKey key, TValue value)
        {
            if (!_fields.ContainsKey(key))
            {
                throw new KeyNotFoundException($"No field found with key {key}.");
            }
            _fields[key] = value;
        }


        #endregion

        #region IConcurrentGroupHierarchyGroups

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

        #endregion
    }
}
