
namespace SnakeGame
{
    public class PositionGrid
    {
        public int Row { get; }
        public int Col {  get; }

        public PositionGrid(int Row, int Col)
        {
            Row = Row;
            Col = Col;
        }

        public PositionGrid Translate(DirectionGrid dir)
        {
            //moving one step in the given direction
            return new PositionGrid(Row + dir.RowOffSet, Col + dir.ColumnOffSet);
        }

        public override bool Equals(object obj)
        {
            return obj is PositionGrid grid &&
                   Row == grid.Row &&
                   Col == grid.Col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Col);
        }

        public static bool operator ==(PositionGrid left, PositionGrid right)
        {
            return EqualityComparer<PositionGrid>.Default.Equals(left, right);
        }

        public static bool operator !=(PositionGrid left, PositionGrid right)
        {
            return !(left == right);
        }
    }
}
