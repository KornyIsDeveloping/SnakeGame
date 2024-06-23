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
    }
}
