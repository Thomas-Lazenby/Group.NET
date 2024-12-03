namespace Group.NET.Group
{
    public interface IGroupGroupHierarchyReadOnly<TKey, TValue> : IGroupHierarchy<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        Group<TKey, TValue>? ParentGroup { get; }
        Group<TKey, TValue> GetRootGroup();
        


        Group<TKey, TValue> GetChildGroup(TKey key);
        /// <summary> Attempts to retrieve a subgroup by key. </summary>
        bool TryGetChildGroup(TKey key, out Group<TKey, TValue>? group);


        /// <summary> Checks if a subgroup with the given key exists. </summary>
        bool ExistsChildGroup(TKey key);

    }
}
