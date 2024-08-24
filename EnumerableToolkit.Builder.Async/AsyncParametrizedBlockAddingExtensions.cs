using EnumerableToolkit.Builder.AsyncBlocks;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Contains extension methods for the <see cref="IAsyncParametrizedEnumerableBuilder{T, TParameters}"/>
    /// that make it easy to add the predefined <see cref="IAsyncBuildingBlock{T}">async building blocks</see>.
    /// </summary>
    public static class AsyncParametrizedBlockAddingExtensions
    {
        /// <summary>
        /// Adds a building block with default priority that concatenates the given items to the end of the current constructed async enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="firstItem">The first item to concatenate.</param>
        /// <param name="following">Any other items to concatenate.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> Concat<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, T firstItem, params T[] following)
            => builder.Concat(firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that concatenates the given async sequence to the end of the current constructed async enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="sequence">The async sequence to concatenate.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> Concat<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new(new AsyncConcatBlock<T>(sequence).ToParametrized<T, TParameters>()));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that concatenates the given async sequence to the end of the current constructed async enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="sequence">The async sequence to concatenate.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> Concat<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, IEnumerable<T> sequence)
            => builder.Concat(sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterEveryItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new(new AsyncInsertAfterEveryItemLambdaBlock<T>(predicate, sequence).ToParametrized<T, TParameters>()));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterEveryItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterFirstItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new(new AsyncInsertAfterFirstItemLambdaBlock<T>(predicate, sequence).ToParametrized<T, TParameters>()));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterFirstItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterFirstItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertAfterFirstItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertAfterFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterFirstItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeEveryItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new(new AsyncInsertBeforeEveryItemLambdaBlock<T>(predicate, sequence).ToParametrized<T, TParameters>()));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeEveryItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeEveryItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeFirstItem(predicate, firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IAsyncEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new(new AsyncInsertBeforeFirstItemLambdaBlock<T>(predicate, sequence).ToParametrized<T, TParameters>()));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate, sequence.ToAsyncEnumerable());

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), firstItem.YieldAsync().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IAsyncEnumerable<T> sequence)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given async sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">An async sequence of items that should be added.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> InsertBeforeFirstItem<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), sequence.ToAsyncEnumerable());

        /// <summary>
        /// Casts a basic <see cref="IAsyncBuildingBlock{T}">async building block</see> to a
        /// <see cref="IAsyncParametrizedBuildingBlock{T, TParameters}">parametrized</see> one.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="block">The basic building block to cast to a parametrized one.</param>
        /// <returns>The casted <paramref name="block"/>.</returns>
        public static IAsyncParametrizedBuildingBlock<T, TParameters> ToParametrized<T, TParameters>(this IAsyncBuildingBlock<T> block)
            => (IAsyncParametrizedBuildingBlock<T, TParameters>)(IAsyncParametrizedBuildingBlock<T, object>)block;

        /// <summary>
        /// Adds a building block with default priority that filters the current async sequence based on the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the current item should be returned.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> Where<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, int, bool> predicate)
        {
            builder.AddBuildingBlock(new(new AsyncWhereItemLambdaBlock<T>(predicate).ToParametrized<T, TParameters>()));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that filters the current async sequence based on the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated async sequence.</typeparam>
        /// <typeparam name="TParameters">The type of the parameters for generating an async sequence.</typeparam>
        /// <param name="builder">Theasync enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the current item should be returned.</param>
        public static IAsyncParametrizedEnumerableBuilder<T, TParameters> Where<T, TParameters>(this IAsyncParametrizedEnumerableBuilder<T, TParameters> builder, Func<T, bool> predicate)
            => builder.Where(predicate.Wrap());
    }
}