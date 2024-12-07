using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IHierarchy<TKey, TValue> : IHierarchyReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        void ClearChildGroups();

        void RemoveChildGroup(TKey key);
        bool TryRemoveChildGroup(TKey key);
    }
}
