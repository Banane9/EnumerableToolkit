﻿using EnumerableToolkit;
using EnumerableToolkit.Builder.AsyncBlocks;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Contains extension methods for the <see cref="IAsyncEnumerableBuilder{T}"/>
    /// that make it easy to add the predefined <see cref="AsyncBuildingBlock{T}">building blocks</see>.
    /// </summary>
    public static class AsyncBlockAddingExtensions
    {
        /// <summary>
        /// Adds a building block with default priority that concatenates the given items to the end of the current constructed async enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="firstItem">The first item to concatenate.</param>
        /// <param name="following">Any other items to concatenate.</param>
        public static IAsyncEnumerableBuilder<T> Concat<T>(this IAsyncEnumerableBuilder<T> builder, T firstItem, params T[] following)
            => builder.Concat(firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that concatenates the given async sequence to the end of the current constructed async enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="sequence">The async sequence to concatenate.</param>
        public static IAsyncEnumerableBuilder<T> Concat<T>(this IAsyncEnumerableBuilder<T> builder, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new AsyncConcatBlock<T>(sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that concatenates the given async sequence to the end of the current constructed async enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="sequence">The async sequence to concatenate.</param>
        public static IAsyncEnumerableBuilder<T> Concat<T>(this IAsyncEnumerableBuilder<T> builder, IEnumerable<T> sequence)
            => builder.Concat(sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterEveryItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new AsyncInsertAfterEveryItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterEveryItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterFirstItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new AsyncInsertAfterFirstItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterFirstItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterFirstItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertAfterFirstItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertAfterFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterFirstItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeEveryItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new AsyncInsertBeforeEveryItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeEveryItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeFirstItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new AsyncInsertBeforeFirstItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that filters the current async sequence based on the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the current item should be returned.</param>
        public static IAsyncEnumerableBuilder<T> Where<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, int, bool> predicate)
        {
            builder.AddBuildingBlock(new AsyncWhereItemLambdaBlock<T>(predicate));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that filters the current async sequence based on the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the current item should be returned.</param>
        public static IAsyncEnumerableBuilder<T> Where<T>(this IAsyncEnumerableBuilder<T> builder, Func<T, bool> predicate)
            => builder.Where(predicate.Wrap());

        internal static Func<T, int, bool> Wrap<T>(this Func<T, bool> predicate)
            => (item, index) => predicate(item);
    }
}