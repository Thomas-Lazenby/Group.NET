namespace Group.NET
{
    /// <summary> For Keys or Values to be flexiable I recommend using <see cref="object"/> </summary>
    public class Group<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        // DESIGN CHOICE REGARDING FLEXIBILITY:
        // Groups can use `object` for keys and values if require different key/values types at different groups as
        // this is to avoid complex generics, which this can be handled by `object`.



        // TODO: Add events.
        // Maybe things aswell such as: ChildFieldAdded etc.


        private readonly Dictionary<TKey, Group<TKey, TValue>> _subGroups = new();
        private readonly Dictionary<TKey, TValue> _fields = new();

        public Group<TKey, TValue>? Parent { get; private set; }

        public bool IsRootGroup
            => Parent == null;

        public Group<TKey, TValue> GetRootGroup()
        {
            var currentGroup = this;
            do
            {
                if (currentGroup.Parent == null)
                    return currentGroup;

                currentGroup = currentGroup.Parent;
            } while (true);
        }

        public bool ContainsKey(TKey key)
            => _fields.ContainsKey(key) || _subGroups.ContainsKey(key);



        #region Field Methods

        /// <summary> Gets all fields. </summary>
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
            if (ContainsKey(key))
                throw new Exception("Key already exists in field or subgroups");

            _fields[key] = value;
        }

        /// <summary> Adds a field to this group. </summary>
        public bool TryAddField(TKey key, TValue value)
        {
            if (ContainsKey(key))
                return false;
            else
                return _fields.TryAdd(key, value);
        }

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

        #region SubGroup Methods

        public IEnumerable<TKey> GetKeysSubGroup()
            => _subGroups.Keys;

        /// <summary> Adds an existing group as a child. </summary>
        public Group<TKey, TValue> CreateSubGroup(TKey key)
            => InsertSubGroup(key, new Group<TKey, TValue>());

        /// <summary> Adds an existing group as a child. </summary>
        public Group<TKey, TValue> InsertSubGroup(TKey key, Group<TKey, TValue> group)
        {
            if (ContainsKey(key))
                throw new InvalidOperationException($"Key '{key}' is already in use by a subgroup or field.");

            group.Parent = this;
            _subGroups[key] = group;

            return group;
        }

        /// <summary> Attempts to add a new subgroup with the specified key. Returns false if the key already exists. </summary>
        public bool TryCreateSubGroup(TKey key, out Group<TKey, TValue> group)
        {
            if (ContainsKey(key))
            {
                group = null!;
                return false;
            }

            group = new Group<TKey, TValue> { Parent = this };
            _subGroups[key] = group;
            return true;
        }

        /// <summary> Attempts to insert an existing subgroup. Returns false if the key already exists. </summary>
        public bool TryInsertSubGroup(TKey key, Group<TKey, TValue> group)
        {
            if (ContainsKey(key))
                return false;

            group.Parent = this;
            _subGroups[key] = group;
            return true;
        }

        /// <summary> Retrieves a subgroup by key. Throws an exception if the subgroup does not exist. </summary>
        public Group<TKey, TValue> GetSubGroup(TKey key)
        {
            if (!_subGroups.TryGetValue(key, out var group))
                throw new KeyNotFoundException($"Subgroup with key '{key}' does not exist.");

            return group;
        }

        /// <summary> Attempts to retrieve a subgroup by key. </summary>
        public bool TryGetSubGroup(TKey key, out Group<TKey, TValue>? group)
            => _subGroups.TryGetValue(key, out group);

        /// <summary> Removes a subgroup by key. Returns true if successful. </summary>
        public bool RemoveSubGroup(TKey key)
            => _subGroups.Remove(key);

        /// <summary> Attempts to remove a subgroup and retrieve its instance. </summary>
        public bool TryRemoveSubGroup(TKey key, out Group<TKey, TValue>? group)
            => _subGroups.Remove(key, out group);

        /// <summary> Checks if a subgroup with the given key exists. </summary>
        public bool ExistsSubGroup(TKey key)
            => _subGroups.ContainsKey(key);

        #endregion


    }

}
