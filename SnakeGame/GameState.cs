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
            AddFood();
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
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new PositionGrid(r, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<PositionGrid> empty = new List<PositionGrid>(EmptyPositions());

            if (empty.Count == 0)
            {
                return;
            }

            PositionGrid pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Food;
        }

        public PositionGrid HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public PositionGrid TailPosition()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<PositionGrid> SnakePositions()
        {
            return snakePositions;
        }

        private void AddHead(PositionGrid pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            PositionGrid tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        public void ChangeDirection(DirectionGrid dir)
        {
            Dir = dir;
        }

        private bool OutsideGrid(PositionGrid pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Columns;
        }

        private GridValue WillHit(PositionGrid newHeadPos)
        {
            if(OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if(newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public void Move()
        {
            PositionGrid newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            if(hit != GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if(hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
}
