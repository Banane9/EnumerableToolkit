namespace EnumerableToolkit.Builder.Blocks
{
    public abstract class EnumerableBuildingBlock<T>
    {
        public abstract IEnumerable<T> Apply(IEnumerable<T> current);
    }
}