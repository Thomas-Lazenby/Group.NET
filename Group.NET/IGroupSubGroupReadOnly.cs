namespace Group.NET
{
    public interface IGroupSubGroupReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TKey> GetKeysSubGroup();

        Group<TKey, TValue> GetSubGroup(TKey key);

        /// <summary> Attempts to retrieve a subgroup by key. </summary>
        bool TryGetSubGroup(TKey key, out Group<TKey, TValue>? group);

        /// <summary> Checks if a subgroup with the given key exists. </summary>
        bool ExistsSubGroup(TKey key);

    }
}
