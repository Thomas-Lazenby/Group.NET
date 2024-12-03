using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroupHierarchy<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

        bool IsChildGroupsEmpty();

        int CountChildGroups();


        void ClearChildGroups();

        void RemoveChildGroup(TKey key);
        bool TryRemoveChildGroup(TKey key);
    }
}
