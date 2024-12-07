using Group.NET.ConcurrentGroup;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public partial class ConcurrentGroup<TKey, TValue> : IConcurrentGroupHierarchy<TKey, TValue>
    {

        #region IConcurrentGroupHierarchyGroups<TKey, TValue>

        public ConcurrentGroup<TKey, TValue> CreateChildGroup(TKey key)
        {
            // lock!
            // groupMapping.Add(currentContextId, key)
            // _fields.Add(etc.)
            // make new concurrent group with:
            // new Concurrent { ParentGroupID = currentCOntextId } etc. so gets parent.



            var newGroup = new ConcurrentGroup<TKey, TValue>
            {
                ParentGroup = this
            };

            var newValue = new Value<ConcurrentGroup<TKey, TValue>>
            {
                Type = ValueType.ChildGroup,
                Data = newGroup
            };

            if (!_values.TryAdd(key, newValue))
            {
                throw new InvalidOperationException($"A key '{key}' already exists as a {_values[key].Type}.");
            }

            return newGroup;
        }

        public bool TryCreateChildGroup(TKey key, out ConcurrentGroup<TKey, TValue> group)
        {
            group = new ConcurrentGroup<TKey, TValue>
            {
                ParentGroup = this
            };

            var newValue = new Value<ConcurrentGroup<TKey, TValue>>
            {
                Type = ValueType.ChildGroup,
                Data = group
            };

            return _values.TryAdd(key, newValue);
        }


        public void InsertChildGroup(TKey key, ConcurrentGroup<TKey, TValue> group)
        {
            var newValue = new Value<ConcurrentGroup<TKey, TValue>>
            {
                Type = ValueType.ChildGroup,
                Data = group
            };

            if (!_values.TryAdd(key, newValue))
            {
                throw new InvalidOperationException($"A key '{key}' already exists as a {_values[key].Type}.");
            }

            group.ParentGroup = this;
        }

        public bool TryInsertChildGroup(TKey key, ConcurrentGroup<TKey, TValue> group)
        {
            var newValue = new Value<ConcurrentGroup<TKey, TValue>>
            {
                Type = ValueType.ChildGroup,
                Data = group
            };

            group.ParentGroup = this;

            return _values.TryAdd(key, newValue);
        }

        #endregion

        #region IGroupHierarchy<TKey, TValue>

        public void ClearChildGroups()
        {
            foreach (var key in _values
                .Where(kvp => kvp.Value.Type == ValueType.ChildGroup)
                .Select(kvp => kvp.Key)
                .ToList())
            {
                _values.TryRemove(key, out _);
            }
        }


        public void RemoveChildGroup(TKey key)
        {
            if (!TryRemoveChildGroup(key))
            {
                throw new KeyNotFoundException($"No child group found with key {key}.");
            }
        }

        public bool TryRemoveChildGroup(TKey key)
            => _values.TryRemove(key, out var existing) && existing.Type == ValueType.ChildGroup;

        #endregion



        #region IConcurrentGroupHierarchyGroupsReadOnly<TKey, TValue>

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



        public ConcurrentGroup<TKey, TValue> GetRootGroup()
        {
            var currentGroup = this;

            while (currentGroup.ParentGroup != null)
            {
                currentGroup = currentGroup.ParentGroup;
            }

            return currentGroup;
        }



        public ConcurrentGroup<TKey, TValue> GetChildGroup(TKey key)
        {
            if (!_values.TryGetValue(key, out var existing) || existing.Type != ValueType.ChildGroup)
            {
                throw new KeyNotFoundException($"No child group found with key '{key}'.");
            }

            return ((Value<ConcurrentGroup<TKey, TValue>>)existing).Data;
        }

        public bool TryGetChildGroup(TKey key, out ConcurrentGroup<TKey, TValue>? group)
        {
            if (_values.TryGetValue(key, out var existing) && existing.Type == ValueType.ChildGroup)
            {
                group = ((Value<ConcurrentGroup<TKey, TValue>>)existing).Data;
                return true;
            }

            group = null;
            return false;
        }

        public bool ExistsChildGroup(TKey key)
            => _values.TryGetValue(key, out var existing) && existing.Type == ValueType.ChildGroup;

        #endregion

        #region IGroupHierarchyReadOnly<TKey, TValue>

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

        public bool IsChildGroupsEmpty()
            => !_values.Any(kvp => kvp.Value.Type == ValueType.ChildGroup);

        public int CountChildGroups()
            => _values.Count(kvp => kvp.Value.Type == ValueType.ChildGroup);

        public IEnumerable<TKey> GetKeysChildrenGroups()
            => _values.Where(kvp => kvp.Value.Type == ValueType.ChildGroup).Select(kvp => kvp.Key);

        #endregion

    }
}


