
namespace SnakeGame
{
    public class DirectionGrid
    {
        public readonly static DirectionGrid Left = new DirectionGrid(0, -1);
        public readonly static DirectionGrid Right = new DirectionGrid(0, 1);
        public readonly static DirectionGrid Up = new DirectionGrid(-1, 0);
        public readonly static DirectionGrid Down = new DirectionGrid(1, 0);

        //grid direction array
        public int RowOffSet {get; }
        public int ColumnOffSet { get; }

        private DirectionGrid(int rowOffSet, int columnOffSet)
        {
            RowOffSet = rowOffSet;
            ColumnOffSet = columnOffSet;
        }

        public DirectionGrid Opposite()
        {
            return new DirectionGrid(-RowOffSet, -ColumnOffSet);
        }

        public override bool Equals(object obj)
        {
            return obj is DirectionGrid grid &&
                   RowOffSet == grid.RowOffSet &&
                   ColumnOffSet == grid.ColumnOffSet;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowOffSet, ColumnOffSet);
        }

        public static bool operator ==(DirectionGrid left, DirectionGrid right)
        {
            return EqualityComparer<DirectionGrid>.Default.Equals(left, right);
        }

        public static bool operator !=(DirectionGrid left, DirectionGrid right)
        {
            return !(left == right);
        }
    }
}
