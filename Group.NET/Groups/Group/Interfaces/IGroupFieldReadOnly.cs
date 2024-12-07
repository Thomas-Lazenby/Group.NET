using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET.Group.Interfaces
{
    public interface IGroupFieldReadOnly<TKey, TValue> : IFieldReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        IDictionary<TKey, TValue> GetFieldsSnapshot();
    }
}
