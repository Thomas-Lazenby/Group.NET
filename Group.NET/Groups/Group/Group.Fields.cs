using Group.NET.Group.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Group.NET
{
    public partial class Group<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        private readonly IDictionary<TKey, TValue> _fields;

    }
}
