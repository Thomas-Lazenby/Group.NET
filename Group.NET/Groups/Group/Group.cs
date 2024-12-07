using Group.NET.Group.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Group.NET
{
    /// <summary> For Keys or Values to be flexiable I recommend using <see cref="object"/> </summary>
    public partial class Group<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

        // TODO: Test for key conflictions in unit tests.
        public bool ContainsKey(TKey key)
            => _fields.ContainsKey(key) || _childrenGroups.ContainsKey(key);

        public Group(IDictionary<TKey, TValue>? fields = null, IDictionary<TKey, Group<TKey,TValue>>? childrenGroups = null)
        {
            _fields = fields ?? new Dictionary<TKey, TValue>();
            _childrenGroups = childrenGroups ?? new Dictionary<TKey, Group<TKey, TValue>>();
        }

    }



}
