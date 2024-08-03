using System;
using System.Collections.Generic;
using System.Text;

namespace EnumerableToolkit.Builder.Blocks
{
    public abstract class EnumerableAddingBlock<T> : EnumerableBuildingBlock<T>
    {
        protected readonly IEnumerable<T> sequence;

        public EnumerableAddingBlock(IEnumerable<T> sequence)
        {
            this.sequence = sequence;
        }
    }
}