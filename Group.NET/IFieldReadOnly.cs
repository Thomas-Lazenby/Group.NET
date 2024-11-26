using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET
{

    public interface IFieldReadOnly<TKey, TValue>
        where TKey : IEquatable<TKey>
    {

        /// <summary> Gets all fields. </summary>
        IEnumerable<TKey> GetKeysField();

        T GetField<T>(TKey key)
            where T : TValue;


        public bool TryGetField<T>(TKey key, out T? value)
            where T : TValue;

        bool ExistsField(TKey key);
    }
}
