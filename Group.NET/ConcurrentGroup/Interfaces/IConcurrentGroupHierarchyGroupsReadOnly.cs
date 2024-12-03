using Group.NET;

namespace Group.NET.ConcurrentGroup
{
    public interface IConcurrentGroupHierarchyGroupsReadOnly<TKey, TValue> : IGroupHierarchy<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        ConcurrentGroup<TKey, TValue>? ParentGroup { get; }

        bool IsRootGroup { get; }

        ConcurrentGroup<TKey, TValue> GetRootGroup();

        IEnumerable<TKey> GetKeysForChildrenGroups();

        ConcurrentGroup<TKey, TValue> GetChildGroup(TKey key);

        /// <summary> Attempts to retrieve a subgroup by key. </summary>
        bool TryGetChildGroup(TKey key, out ConcurrentGroup<TKey, TValue>? group);

        /// <summary> Checks if a subgroup with the given key exists. </summary>
        bool ExistsChildGroup(TKey key);

    }
}
