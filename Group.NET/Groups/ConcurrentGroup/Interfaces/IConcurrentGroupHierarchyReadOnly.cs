using Group.NET;
using Group.NET.Group;

namespace Group.NET.ConcurrentGroup
{
    public interface IConcurrentGroupHierarchyReadOnly<TKey, TValue> : IHierarchyReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        ConcurrentGroup<TKey, TValue>? ParentGroup { get; }
        ConcurrentGroup<TKey, TValue> GetRootGroup();

        ConcurrentGroup<TKey, TValue> GetChildGroup(TKey key);

        /// <summary> Attempts to retrieve a subgroup by key. </summary>
        bool TryGetChildGroup(TKey key, out ConcurrentGroup<TKey, TValue>? group);
    }
}
