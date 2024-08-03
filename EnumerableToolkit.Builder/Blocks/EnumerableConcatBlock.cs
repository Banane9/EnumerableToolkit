using System;
using System.Collections.Generic;
using System.Text;

namespace EnumerableToolkit.Builder.Blocks
{
    public sealed class EnumerableConcatBlock<T> : EnumerableAddingBlock<T>
    {
        public EnumerableConcatBlock(IEnumerable<T> sequence) : base(sequence)
        { }

        /// <inheritdoc/>
        public override IEnumerable<T> Apply(IEnumerable<T> current)
            => current.Concat(sequence);
    }
}