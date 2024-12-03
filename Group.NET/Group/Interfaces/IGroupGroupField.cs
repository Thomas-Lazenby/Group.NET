using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET.Group.Interfaces
{
    public interface IGroupGroupField<TKey, TValue> : IGroupField<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        Group<TKey, TValue> GetFieldsSnapshot();
    }
}
