using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using UiPath.PowerShell.Util;

namespace UiPath.PowerShell.Tests.Cmdlets
{
    [TestClass]
    public class MergeAddRemoveTests
    {

        [TestMethod]
        public void MergeAllEmpty()
        {
            var existing = new int[] {};
            var merged = existing.MergeAddRemove(Enumerable.Empty<int>(), Enumerable.Empty<int>(), i => i);
            CollectionAssert.AreEqual(existing, merged.ToArray());
        }

        public void MergeAddNull()
        {
            var existing = new int[] {1, 2, 3};
            var merged = existing.MergeAddRemove(null, new int[] { 3 }, i => i);
            CollectionAssert.AreEqual(new int[] { 1, 2 }, merged.ToArray());
        }

        public void MergeRemoveNull()
        {
            var existing = new int[] { 1, 2, 3 };
            var merged = existing.MergeAddRemove(new int[] { 4 }, null, i => i);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4}, merged.ToArray());
        }

        [TestMethod]
        public void MergeAddRemoveEmpty()
        {
            var existing = new int[] { 1, 2, 3 };
            var merged = existing.MergeAddRemove(Enumerable.Empty<int>(), Enumerable.Empty<int>(), i => i);
            CollectionAssert.AreEqual(existing, merged.ToArray());
        }


        [TestMethod]
        public void MergeAddEntry()
        {
            var existing = new int[] { 1, 2, 3 };
            var merged = existing.MergeAddRemove(new int[] { 4 }, Enumerable.Empty<int>(), i => i);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4 }, merged.ToArray());
        }

        [TestMethod]
        public void MergeEmptyAddEntry()
        {
            var existing = Enumerable.Empty<int>();
            var merged = existing.MergeAddRemove(new int[] { 4 }, Enumerable.Empty<int>(), i => i);
            CollectionAssert.AreEqual(new int[] { 4 }, merged.ToArray());
        }


        [TestMethod]
        public void MergeRemoveEntry()
        {
            var existing = new int[] { 1, 2, 3 };
            var merged = existing.MergeAddRemove(Enumerable.Empty<int>(), new int[] { 2 }, i => i);
            CollectionAssert.AreEqual(new int[] { 1, 3}, merged.ToArray());
        }


        [TestMethod]
        public void MergeAddRemoveEntry()
        {
            var existing = new int[] { 1, 2, 3 };
            var merged = existing.MergeAddRemove(new int[] { 4 }, new int[] { 2 }, i => i);
            CollectionAssert.AreEqual(new int[] { 1, 3, 4 }, merged.ToArray());
        }

        [TestMethod]
        public void MergeAddRemoveExistingEntry()
        {
            var existing = new int[] { 1, 2, 3 };
            var merged = existing.MergeAddRemove(new int[] { 2, 4 }, new int[] { 2, 3 }, i => i);
            CollectionAssert.AreEqual(new int[] { 1, 2, 4 }, merged.ToArray());
        }

        [TestMethod]
        public void MergeAddRemoveNewEntry()
        {
            var existing = new int[] { 1, 2, 3 };
            var merged = existing.MergeAddRemove(new int[] { 4 }, new int[] { 4 }, i => i);
            CollectionAssert.AreEqual(new int[] { 1, 2, 3, 4 }, merged.ToArray());
        }
    }
}
