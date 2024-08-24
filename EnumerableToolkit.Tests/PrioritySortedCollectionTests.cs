namespace EnumerableToolkit.Tests
{
    [TestClass]
    public class PrioritySortedCollectionTests
    {
        [TestMethod]
        public void WrappedPrioritizable()
        {
            var collection = new PrioritySortedCollection<Prioritizable<int>>();
            Assert.IsNotNull(collection);

            collection.Add(3);
            collection.Add((1, 1));
            collection.Add((2, 1));
            collection.Add(4);
            collection.Add((5, -1));

            Assert.IsTrue(collection.Unwrap().SequenceEqual([1, 2, 3, 4, 5]));
        }
    }
}