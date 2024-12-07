namespace Group.NET.Group
{
    public interface IGroupHierarchyReadOnly<TKey, TValue> : IHierarchyReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        Group<TKey, TValue>? ParentGroup { get; }
        Group<TKey, TValue> GetRootGroup();
        
        Group<TKey, TValue> GetChildGroup(TKey key);

        /// <summary> Attempts to retrieve a subgroup by key. </summary>
        bool TryGetChildGroup(TKey key, out Group<TKey, TValue>? group);

    }
}
