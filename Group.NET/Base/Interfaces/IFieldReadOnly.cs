using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{

    public interface IFieldReadOnly<TKey, TValue> : IReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        bool IsFieldsEmpty();

        int CountFields();

        IEnumerable<TKey> GetKeysField();

        bool ExistsField(TKey key);

        T GetField<T>(TKey key)
            where T : TValue;

        public bool TryGetField<T>(TKey key, out T? value)
            where T : TValue;
    }
}
