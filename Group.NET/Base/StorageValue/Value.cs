namespace Group.NET
{
    public class Value<TValue> : IValueType
    {
        public required ValueType Type { get; init; }

        public required TValue Data { get; set; }
    }
}
