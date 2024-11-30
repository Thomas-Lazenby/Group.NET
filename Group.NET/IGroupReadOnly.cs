using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroupReadOnly<TKey, TValue> : IGroupHierarchyGroupsReadOnly<TKey, TValue>, IGroupFieldReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

    }
}
