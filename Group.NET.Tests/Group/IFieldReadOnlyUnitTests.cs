using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Group.NET.Tests
{
    [TestClass]
    public class IFieldReadOnlyUnitTests
    {
        // Data source for test cases
        public static IEnumerable<object[]> GetFieldReadOnlyImplementations()
        {
            yield return new object[] { CreateGroupInstance() };
            // yield return new object[] { CreateConcurrentGroupInstance() };
        }

        private static IFieldReadOnly<string, object> CreateGroupInstance()
        {
            var group = new Group<string, object>();
            PopulateTestData(group);
            return group;
        }

        private static IFieldReadOnly<string, object> CreateConcurrentGroupInstance()
        {
            var concurrentGroup = new ConcurrentGroup<string, object>();
            PopulateTestData(concurrentGroup);
            return concurrentGroup;
        }

        private static void PopulateTestData(IField<string, object> field)
        {
            field.AddField("key1", 42);
            field.AddField("ke5", "Test String");
            field.AddField("key3", 99.99);
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void IsFieldsEmpty_ReturnsFalseWhenFieldsExist(IFieldReadOnly<string, object> field)
        {
            Assert.IsFalse(field.IsFieldsEmpty());
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void CountFields_ReturnsCorrectCount(IFieldReadOnly<string, object> field)
        {
            Assert.AreEqual(3, field.CountFields());
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void GetKeysField_ReturnsAllKeys(IFieldReadOnly<string, object> field)
        {
            var keys = field.GetKeysField().ToList();

            CollectionAssert.AreEquivalent(new List<string> { "key1", "ke5", "key3" }, keys);
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void ExistsField_ReturnsTrueForExistingKey(IFieldReadOnly<string, object> field)
        {
            Assert.IsTrue(field.ExistsField("key1"));
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void ExistsField_ReturnsFalseForNonExistentKey(IFieldReadOnly<string, object> field)
        {
            Assert.IsFalse(field.ExistsField("nonExistentKey"));
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void GetField_ValidKey_ReturnsValue(IFieldReadOnly<string, object> field)
        {
            Assert.AreEqual(42, field.GetField<int>("key1"));
            Assert.AreEqual("Test String", field.GetField<string>("ke5"));
            Assert.AreEqual(99.99, field.GetField<double>("key3"));
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetField_InvalidKey_ThrowsException(IFieldReadOnly<string, object> field)
        {
            field.GetField<int>("nonExistentKey");
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetField_InvalidCast_ThrowsException(IFieldReadOnly<string, object> field)
        {
            field.GetField<string>("key1"); // Attempting to cast an int to string
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void TryGetField_ValidKey_ReturnsTrueAndValue(IFieldReadOnly<string, object> field)
        {
            var result = field.TryGetField<int>("key1", out var value);

            Assert.IsTrue(result);
            Assert.AreEqual(42, value);
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void TryGetField_InvalidKey_ReturnsFalse(IFieldReadOnly<string, object> field)
        {
            var result = field.TryGetField<int>("nonExistentKey", out var value);

            Assert.IsFalse(result);
            Assert.IsInstanceOfType(value, typeof(int));
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldReadOnlyImplementations), DynamicDataSourceType.Method)]
        public void TryGetField_InvalidCast_ReturnsFalse(IFieldReadOnly<string, object> field)
        {
            var result = field.TryGetField<string>("key1", out var value);

            Assert.IsFalse(result);
            Assert.IsNull(value);
        }
    }
}
