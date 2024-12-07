using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET.Tests
{
    [TestClass]
    public class IFieldUnitTest
    {
        public static IEnumerable<object[]> GetFieldImplementations()
        {
            yield return new object[] { new Group<string, object>() };
            //yield return new object[] { new ConcurrentGroup<string, object>() };
        }


        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void AddField_ValidKey_AddsField(IField<string, object> field)
        {
            field.AddField("key1", 42);

            Assert.AreEqual(42, field.GetField<int>("key1"));
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddField_DuplicateKey_ThrowsException(IField<string, object> field)
        {
            field.AddField("key1", 42);

            field.AddField("key1", 100);
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void TryAddField_NewKey_ReturnsTrue(IField<string, object> field)
        {
            var result = field.TryAddField("key1", 42);

            Assert.IsTrue(result);
            Assert.AreEqual(42, field.GetField<int>("key1"));
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void RemoveField_ValidKey_RemovesField(IField<string, object> field)
        {
            field.AddField("key1", 42);
            field.RemoveField("key1");

            Assert.IsTrue(field.IsFieldsEmpty());
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void RemoveField_InvalidKey_AttemptsRemovesField(IField<string, object> field)
        {
            field.RemoveField("nonExistentKey");
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void UpdateField_ExistingKey_UpdatesValue(IField<string, object> field)
        {
            field.AddField("key1", 42);
            field.UpdateField("key1", 100);

            Assert.AreEqual(100, field.GetField<int>("key1"));
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void UpdateField_NonExistentKey_ThrowsException(IField<string, object> field)
        {
            field.UpdateField("key1", 100);
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void TryRemoveField_ExistingKey_ReturnsTrue(IField<string, object> field)
        {
            field.AddField("key1", 42);
            var result = field.TryRemoveField("key1");

            Assert.IsTrue(result);
            Assert.IsTrue(field.IsFieldsEmpty());
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void TryRemoveField_NonExistentKey_ReturnsFalse(IField<string, object> field)
        {
            var result = field.TryRemoveField("key1");

            Assert.IsFalse(result);
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void ClearFields_RemovesAllFields(IField<string, object> field)
        {
            field.AddField("key1", 42);
            field.AddField("ke5", 100);

            field.ClearFields();

            field.AddField("key3", 1001);

            Assert.IsTrue(field.CountFields() == 1);
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void TryUpdateField_ExistingKey_ReturnsTrue(IField<string, object> field)
        {
            field.AddField("key1", 42);

            var result = field.TryUpdateField("key1", 100);

            Assert.IsTrue(result, "TryUpdateField should return true when the key exists.");
            Assert.AreEqual(100, field.GetField<int>("key1"), "The value should be updated to 100.");
        }

        [TestMethod]
        [DynamicData(nameof(GetFieldImplementations), DynamicDataSourceType.Method)]
        public void TryUpdateField_NonExistentKey_ReturnsFalse(IField<string, object> field)
        {
            var result = field.TryUpdateField("key1", 100);

            Assert.IsFalse(result, "TryUpdateField should return false when the key does not exist.");
        }

    }
}
