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
        private readonly ConcurrentDictionary<TKey, ConcurrentGroup<TKey, TValue>> _childrenGroups = new();

        #region IConcurrentGroupHierarchyGroups<TKey, TValue>

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

        #endregion

        #region IGroupHierarchy<TKey, TValue>
        
        public void ClearChildGroups()
            => _childrenGroups.Clear();

        public void RemoveChildGroup(TKey key)
        {
            if (!TryRemoveChildGroup(key))
            {
                throw new KeyNotFoundException($"No child group found with key {key}.");
            }
        }

        public bool TryRemoveChildGroup(TKey key)
            => _childrenGroups.TryRemove(key, out _);

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
            => _childrenGroups.IsEmpty;

        public int CountChildGroups()
            => _childrenGroups.Count;

        public IEnumerable<TKey> GetKeysChildrenGroups()
            => _childrenGroups.Keys;

        #endregion

    }
}
