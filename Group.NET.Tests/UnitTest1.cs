using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Group.NET.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var group = new ConcurrentGroup<int, int>();

            group.ClearFields();
            group.ClearChildGroups();
        }



    }
}