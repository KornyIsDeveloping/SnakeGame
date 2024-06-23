using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameState
    {
        //current state of the game
        public int Rows { get; }
        public int Columns { get; }
        public GridValue[,] Grid { get; }

        public DirectionGrid Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        //list with positions currently occupied by the snake
        private readonly LinkedList<PositionGrid> snakePositions = new LinkedList<PositionGrid>();
        //where the food should spawn
        private readonly Random random = new Random();

        public GameState(int rows, int cols) 
        { 
            Rows = rows;
            Columns = cols;
            Grid = new GridValue[rows, cols];
            Dir = DirectionGrid.Right;

            AddSnake();
        }

        private void AddSnake()
        {
            int r = Rows / 2;

            for(int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                snakePositions.AddFirst(new PositionGrid(r, c));
            }
        }

        //method that returns all empty positions for food
        private IEnumerable<PositionGrid> EmptyPositions()
        {
            for(int r = 0; r < Rows; r++)
            {
                for(int c=0; c < Columns; c++)
                {
                    yield return new PositionGrid(r, c);
                }
            }
        }
    }
}
