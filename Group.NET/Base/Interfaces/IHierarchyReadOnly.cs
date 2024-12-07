namespace Group.NET
{
    public interface IHierarchyReadOnly<TKey, TValue> : IReadOnly<TKey, TValue>
        where TKey: IEquatable<TKey>
    {
        bool IsRootGroup { get; }

        bool IsChildGroupsEmpty();

        int CountChildGroups();

        IEnumerable<TKey> GetKeysChildrenGroups();

        bool ExistsChildGroup(TKey key);
    }
}
