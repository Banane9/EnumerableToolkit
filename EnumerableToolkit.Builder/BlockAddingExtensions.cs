﻿using EnumerableToolkit.Builder.Blocks;

namespace EnumerableToolkit.Builder
{
    /// <summary>
    /// Contains extension methods for the <see cref="IEnumerableBuilder{T}"/>
    /// that make it easy to add the predefined <see cref="BuildingBlock{T}">building blocks</see>.
    /// </summary>
    public static class BlockAddingExtensions
    {
        /// <summary>
        /// Adds a building block with default priority that concatenates the given items to the end of the current constructed enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="firstItem">The first item to concatenate.</param>
        /// <param name="following">Any other items to concatenate.</param>
        public static IEnumerableBuilder<T> Concat<T>(this IEnumerableBuilder<T> builder, T firstItem, params T[] following)
            => builder.Concat(firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that concatenates the given sequence to the end of the current constructed enumerable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="sequence">The sequence to concatenate.</param>
        public static IEnumerableBuilder<T> Concat<T>(this IEnumerableBuilder<T> builder, IEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new ConcatBlock<T>(sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertAfterEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterEveryItem(predicate, firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertAfterEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new InsertAfterEveryItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertAfterEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterEveryItem(predicate.Wrap(), firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence after every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after an item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertAfterEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterEveryItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertAfterFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterFirstItem(predicate, firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertAfterFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new InsertAfterFirstItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given items after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted after the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertAfterFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertAfterFirstItem(predicate.Wrap(), firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence after the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted after that item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertAfterFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertAfterFirstItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeEveryItem(predicate, firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new InsertBeforeEveryItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before an item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence before every item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before an item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertBeforeEveryItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeEveryItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeFirstItem(predicate, firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate, IEnumerable<T> sequence)
        {
            builder.AddBuildingBlock(new InsertBeforeFirstItemLambdaBlock<T>(predicate, sequence));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that inserts the given items before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the items should be inserted before the item.</param>
        /// <param name="firstItem">The first item to add.</param>
        /// <param name="following">Any other items to add.</param>
        public static IEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, T firstItem, params T[] following)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), firstItem.Yield().Concat(following));

        /// <summary>
        /// Adds a building block with default priority that inserts the given sequence before the first item that matches the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the <paramref name="sequence"/> of items should be inserted before that item.</param>
        /// <param name="sequence">A sequence of items that should be added.</param>
        public static IEnumerableBuilder<T> InsertBeforeFirstItem<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate, IEnumerable<T> sequence)
            => builder.InsertBeforeFirstItem(predicate.Wrap(), sequence);

        /// <summary>
        /// Adds a building block with default priority that filters the current sequence based on the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the current item should be returned.</param>
        public static IEnumerableBuilder<T> Where<T>(this IEnumerableBuilder<T> builder, Func<T, int, bool> predicate)
        {
            builder.AddBuildingBlock(new WhereItemLambdaBlock<T>(predicate));

            return builder;
        }

        /// <summary>
        /// Adds a building block with default priority that filters the current sequence based on the given <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the items in the generated sequence.</typeparam>
        /// <param name="builder">The enumerable builder to add the building block to.</param>
        /// <param name="predicate">A predicate determining whether the current item should be returned.</param>
        public static IEnumerableBuilder<T> Where<T>(this IEnumerableBuilder<T> builder, Func<T, bool> predicate)
            => builder.Where(predicate.Wrap());

        internal static Func<T, int, bool> Wrap<T>(this Func<T, bool> predicate)
            => (item, index) => predicate(item);
    }
}