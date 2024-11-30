using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{

    public interface IGroupFieldsReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        IEnumerable<TKey> GetKeysField();

        T GetField<T>(TKey key)
            where T : TValue;

        public bool TryGetField<T>(TKey key, out T? value)
            where T : TValue;

        bool ExistsField(TKey key);
    }
}
