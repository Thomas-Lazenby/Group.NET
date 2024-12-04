using Group.NET.Group.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Group.NET
{
    /// <summary> For Keys or Values to be flexiable I recommend using <see cref="object"/> </summary>
    public partial class Group<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        public bool ContainsKey(TKey key)
            => _fields.ContainsKey(key) || _childrenGroups.ContainsKey(key);

        public Group(IDictionary<TKey, TValue>? fields = null, IDictionary<TKey, Group<TKey,TValue>>? childrenGroups = null)
        {
            _fields = fields ?? new Dictionary<TKey, TValue>();
            _childrenGroups = childrenGroups ?? new Dictionary<TKey, Group<TKey, TValue>>();
        }

        /*

        // DESIGN CHOICE REGARDING FLEXIBILITY:
        // Groups can use `object` for keys and values if require different key/values types at different groups as
        // this is to avoid complex generics, which this can be handled by `object`.



        // TODO: Add events.
        // Maybe things aswell such as: ChildFieldAdded etc.


        protected readonly IDictionary<TKey, Group<TKey, TValue>> _childGroups;
        protected readonly IDictionary<TKey, TValue> _fields;

        public Group() : this(new Dictionary<TKey, TValue>(), new Dictionary<TKey, Group<TKey, TValue>>()) { }

        // Protected constructor for custom dictionary implementations
        protected Group(IDictionary<TKey, TValue> fields, IDictionary<TKey, Group<TKey, TValue>> childGroups)
        {
            _fields = fields;
            _childGroups = childGroups;
        }


        public Group<TKey, TValue>? ParentGroup { get; private set; }

        public bool IsRootGroup
            => ParentGroup == null;

        public Group<TKey, TValue> GetRootGroup()
        {
            var currentGroup = this;
            do
            {
                if (currentGroup.ParentGroup == null)
                    return currentGroup;

                currentGroup = currentGroup.ParentGroup;
            } while (true);
        }

        public bool ContainsKey(TKey key)
            => _fields.ContainsKey(key) || _childGroups.ContainsKey(key);



        #region Field Methods

        public IEnumerable<TKey> GetKeysField()
            => _fields.Keys;

        public T GetField<T>(TKey key)
            where T : TValue
        {
            if (!_fields.TryGetValue(key, out TValue? value))
                throw new KeyNotFoundException($"Field with key '{key}' does not exist.");

            if (value is T tValue)
            {
                return tValue;
            }
            else
            {
                throw new InvalidCastException($"Field with key '{key}' is not of type '{typeof(T).Name}' it is '{value?.GetType().Name}'.");
            }
        }


        public bool TryGetField<T>(TKey key, out T? value)
            where T : TValue
        {
            if (_fields.TryGetValue(key, out TValue? fieldValue) && fieldValue is T tValue)
            {
                value = tValue;
                return true;
            }

            value = default;
            return false;
        }


        public void AddField(TKey key, TValue value)
        {
            if (!_fields.TryAdd(key, value)) // ConcurrentDictionary's TryAdd ensures atomicity
            {
                throw new InvalidOperationException($"Key '{key}' already exists in field or subgroups");
            }
        }

        /// <summary> Adds a field to this group. </summary>
        public bool TryAddField(TKey key, TValue value)
            => _fields.TryAdd(key, value);

        public void UpdateField(TKey key, TValue value)
            => _fields[key] = value;

        /// <summary> Finds a field by key. </summary>
        public bool TryUpdateField(TKey key, TValue value)
        {
            if (!_fields.ContainsKey(key))
                return false;

            _fields[key] = value;
            return true;
        }

        public bool RemoveField(TKey key)
            => _fields.Remove(key);

        public bool TryRemoveField(TKey key)
        {
            // TODO Turn this into tenary operation.
            if (!_fields.ContainsKey(key))
                return false;
            else
                return _fields.Remove(key);
        }

        public bool ExistsField(TKey key)
            => _fields.ContainsKey(key);

        #endregion

        #region Child Methods

        public IEnumerable<TKey> GetKeysForChildrenGroups()
            => _childGroups.Keys;

        public Group<TKey, TValue> CreateChildGroup(TKey key)
            => InsertChildGroup(key, new Group<TKey, TValue>());

        public bool TryCreateChildGroup(TKey key, out Group<TKey, TValue> group)
        {
            if (ContainsKey(key))
            {
                group = null!;
                return false;
            }

            group = new Group<TKey, TValue> { ParentGroup = this };
            _childGroups[key] = group;
            return true;
        }

        public Group<TKey, TValue> InsertChildGroup(TKey key, Group<TKey, TValue> group)
        {
            if (ContainsKey(key))
                throw new InvalidOperationException($"Key '{key}' is already in use by a subgroup or field.");

            group.ParentGroup = this;
            _childGroups[key] = group;

            return group;
        }



        public bool TryInsertChildGroup(TKey key, Group<TKey, TValue> group)
        {
            if (ContainsKey(key))
                return false;

            group.ParentGroup = this;
            _childGroups[key] = group;
            return true;
        }

        public Group<TKey, TValue> GetChildGroup(TKey key)
        {
            if (!_childGroups.TryGetValue(key, out var group))
                throw new KeyNotFoundException($"Subgroup with key '{key}' does not exist.");

            return group;
        }

        public bool TryGetChildGroup(TKey key, out Group<TKey, TValue>? group)
            => _childGroups.TryGetValue(key, out group);

        public bool RemoveChildGroup(TKey key)
            => _childGroups.Remove(key);

        public bool TryRemoveChildGroup(TKey key, out Group<TKey, TValue>? group)
            => _childGroups.Remove(key, out group);

        public bool ExistsChildGroup(TKey key)
            => _childGroups.ContainsKey(key);

    #endregion

*/
    }



}
