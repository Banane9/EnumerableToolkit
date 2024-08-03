using EnumerableToolkit.Builder.Blocks;
using System.Collections;

namespace EnumerableToolkit.Builder
{
    public sealed class EnumerableBuilder<T> : IEnumerable<T>
    {
        private readonly List<EnumerableBuildingBlock<T>> _buildingBlocks = [];

        public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}