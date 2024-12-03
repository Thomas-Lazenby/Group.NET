namespace Group.NET
{
    public interface IGroupHierarchyReadOnly<TKey, TValue>
        where TKey: IEquatable<TKey>
    {
        IEnumerable<TKey> GetKeysForChildrenGroups();
        bool IsRootGroup { get; }
    }
}
