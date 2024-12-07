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

        #region IGroupHierarchyReadOnly<TKey, TValue>

        public Group<TKey, TValue>? ParentGroup => _parentGroup;

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
            => _childrenGroups.Clear();

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
            if (!_childrenGroups.Remove(key))
            {
                throw new KeyNotFoundException($"No child group found with key '{key}'.");
            }
        }

        public bool TryRemoveChildGroup(TKey key)
            => _childrenGroups.Remove(key);

        #endregion
    }
}
