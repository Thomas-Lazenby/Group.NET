using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Group.NET.Tests
{
    [TestClass]
    public class UnitTest1
    {
        // TODO: Make tests that do same share same tests so for example CountChildGroups can be tested together with
        // IGroup etc.


        [TestMethod]
        public void TestMethod1()
        {
            var commands = new ConcurrentGroup<string, object>();
            {
                var categoryFun = commands.CreateChildGroup("fun");
                {
                    var banCommand = categoryFun.CreateChildGroup("banCommand");
                    banCommand.AddField("permission.perm", 2);
                }
            }

            Assert.IsTrue(commands.GetChildGroup("fun").GetChildGroup("banCommand") != null);
        }

        [TestMethod]
        public void ConcurrentAccess_ShouldBeThreadSafe()
        {
            var commands = new ConcurrentGroup<string, object>();

            Parallel.For(0, 10000, i =>
            {
                var category = commands.CreateChildGroup($"category{i}");
                category.AddField("key", i);
            });

            Assert.AreEqual(10000, commands.CountChildGroups());
        }

        [TestMethod]
        public void SequentialAccess_ShouldBeConsistent()
        {
            var commands = new ConcurrentGroup<string, object>();

            for (int i = 0; i < 10000; i++)
            {
                var category = commands.CreateChildGroup($"category{i}");
                category.AddField("key", i);
            }

            Assert.AreEqual(10000, commands.CountChildGroups());
        }




    }
}