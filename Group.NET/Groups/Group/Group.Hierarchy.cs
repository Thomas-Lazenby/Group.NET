using Group.NET.Group;
using Group.NET.Group.Interfaces;
using System.Collections.Generic;

namespace Group.NET
{
    public partial class Group<TKey, TValue> : IHierarchy<TKey, TValue>, IGroupHierarchyReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        private readonly IDictionary<TKey, Group<TKey, TValue>> _childrenGroups = new Dictionary<TKey, Group<TKey, TValue>>();
        private Group<TKey, TValue>? _parentGroup;

        public Group<TKey, TValue> CreateChildGroup(TKey key)
        {
            if (ContainsKey(key))
            {
                throw new InvalidOperationException($"Key '{key}' already exists.");
            }

            var childGroup = new Group<TKey, TValue>
            {
                _parentGroup = this
            };

            _childrenGroups[key] = childGroup;

            return childGroup;
        }

        public void AddChildGroup(TKey key, Group<TKey, TValue> childGroup)
        {
            if (ContainsKey(key))
            {
                throw new InvalidOperationException($"Key '{key}' already exists.");
            }

            if (childGroup.ParentGroup != null)
            {
                throw new InvalidOperationException($"The child group already belongs to another parent.");
            }

            childGroup._parentGroup = this;
            _childrenGroups[key] = childGroup;
        }

        #region IGroupHierarchyReadOnly<TKey, TValue>

        public Group<TKey, TValue>? ParentGroup
            => _parentGroup;

        public Group<TKey, TValue> GetRootGroup()
        {
            var currentGroup = this;
            while (currentGroup.ParentGroup != null)
            {
                currentGroup = currentGroup.ParentGroup;
            }
            return currentGroup;
        }

        public Group<TKey, TValue> GetChildGroup(TKey key)
        {
            if (!_childrenGroups.TryGetValue(key, out var group))
            {
                throw new KeyNotFoundException($"No child group found with key '{key}'.");
            }
            return group;
        }

        public bool TryGetChildGroup(TKey key, out Group<TKey, TValue>? group)
            => _childrenGroups.TryGetValue(key, out group);

        #endregion

        #region IHierarchy<TKey, TValue>

        public bool IsRootGroup
            => _parentGroup == null;

        public void ClearChildGroups()
        {
            foreach (var childGroup in _childrenGroups.Values)
            {
                childGroup._parentGroup = null;
            }
            _childrenGroups.Clear();
        }

        public int CountChildGroups()
            => _childrenGroups.Count;

        public bool ExistsChildGroup(TKey key)
            => _childrenGroups.ContainsKey(key);

        public IEnumerable<TKey> GetKeysChildrenGroups()
            => _childrenGroups.Keys;

        public bool IsChildGroupsEmpty()
            => !_childrenGroups.Any();

        public void RemoveChildGroup(TKey key)
        {
            if (!_childrenGroups.Remove(key, out var deletedGroup))
            {
                throw new KeyNotFoundException($"No child group found with key '{key}'.");
            }

            deletedGroup._parentGroup = null;
        }

        public bool TryRemoveChildGroup(TKey key)
        {
            if (_childrenGroups.Remove(key, out var deletedGroup))
            {
                deletedGroup._parentGroup = null;
                return true;
            }

            return false;
        }

        #endregion
    }
}
