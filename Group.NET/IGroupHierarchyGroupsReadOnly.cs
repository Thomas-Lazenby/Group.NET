namespace Group.NET
{
    public interface IGroupHierarchyGroupsReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        Group<TKey, TValue>? ParentGroup { get; }

        bool IsRootGroup { get; }

        Group<TKey, TValue> GetRootGroup();

        IEnumerable<TKey> GetKeysForChildrenGroups();

        Group<TKey, TValue> GetChildGroup(TKey key);

        /// <summary> Attempts to retrieve a subgroup by key. </summary>
        bool TryGetChildGroup(TKey key, out Group<TKey, TValue>? group);

        /// <summary> Checks if a subgroup with the given key exists. </summary>
        bool ExistsChildGroup(TKey key);

    }
}
