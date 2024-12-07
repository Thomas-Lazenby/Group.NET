using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group.NET.Tests.Group.Hierarchy
{
    [TestClass]
    public class GroupHierarchyUnitTests
    {
        // TODO: Unit tests on deleting middle groups and handled all correctly.

        [TestMethod]
        public void CreateChildGroup_ValidKey_CreatesChildGroup()
        {
            // Arrange
            var rootGroup = new Group<string, int>();

            // Act
            var childGroup = rootGroup.CreateChildGroup("child1");

            // Assert
            Assert.IsNotNull(childGroup);
            Assert.AreEqual(rootGroup, childGroup.ParentGroup);
            Assert.IsTrue(rootGroup.ExistsChildGroup("child1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateChildGroup_DuplicateKey_ThrowsException()
        {
            var rootGroup = new Group<string, int>();
            rootGroup.CreateChildGroup("child1");

            rootGroup.CreateChildGroup("child1"); // Should throw
        }

        [TestMethod]
        public void AddChildGroup_ValidKey_AddsChildGroup()
        {
            var rootGroup = new Group<string, int>();
            var childGroup = new Group<string, int>();

            rootGroup.AddChildGroup("child1", childGroup);

            Assert.AreEqual(rootGroup, childGroup.ParentGroup);
            Assert.IsTrue(rootGroup.ExistsChildGroup("child1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddChildGroup_DuplicateKey_ThrowsException()
        {
            var rootGroup = new Group<string, int>();
            var childGroup1 = new Group<string, int>();
            var childGroup2 = new Group<string, int>();

            rootGroup.AddChildGroup("child1", childGroup1);

            rootGroup.AddChildGroup("child1", childGroup2); // Should throw
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddChildGroup_ChildAlreadyHasParent_ThrowsException()
        {
            // Arrange
            var rootGroup1 = new Group<string, int>();
            var rootGroup2 = new Group<string, int>();
            var childGroup = new Group<string, int>();

            rootGroup1.AddChildGroup("child1", childGroup);

            // Act
            rootGroup2.AddChildGroup("child2", childGroup); // Should throw
        }

        [TestMethod]
        public void AddChildGroup_ChildGroupReferencesParent()
        {
            // Arrange
            var rootGroup = new Group<string, int>();
            var childGroup = new Group<string, int>();

            // Act
            rootGroup.AddChildGroup("child1", childGroup);

            // Assert
            Assert.AreEqual(rootGroup, childGroup.ParentGroup);
        }

        [TestMethod]
        public void AddChildGroup_ChildGroupIsInChildrenCollection()
        {
            // Arrange
            var rootGroup = new Group<string, int>();
            var childGroup = new Group<string, int>();

            // Act
            rootGroup.AddChildGroup("child1", childGroup);

            // Assert
            Assert.IsTrue(rootGroup.ExistsChildGroup("child1"));
        }

        [TestMethod]
        public void CreateChildGroup_MultipleChildren_AllChildGroupsExist()
        {
            // Arrange
            var rootGroup = new Group<string, int>();

            // Act
            var child1 = rootGroup.CreateChildGroup("child1");
            var child2 = rootGroup.CreateChildGroup("child2");

            // Assert
            Assert.IsTrue(rootGroup.ExistsChildGroup("child1"));
            Assert.IsTrue(rootGroup.ExistsChildGroup("child2"));
        }
    }
}
