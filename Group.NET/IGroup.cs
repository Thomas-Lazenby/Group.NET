using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{
    public interface IGroup<TKey, TValue> : IGroupField<TKey, TValue>, IGroupParentChildrenGroups<TKey, TValue>, IGroupReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

    }
}
