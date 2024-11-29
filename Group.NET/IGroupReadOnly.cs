using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroupReadOnly<TKey, TValue> : IGroupSubGroupReadOnly<TKey, TValue>, IGroupFieldReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        Group<TKey, TValue>? Parent { get; }

        bool IsRootGroup { get; }
    }
}
