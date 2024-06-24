using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //mapping grid values to image sources
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body },
            { GridValue.Food, Images.Food }
        };

        private readonly Dictionary<DirectionGrid, int> dirToRotation = new()
        {
            { DirectionGrid.Up, 0 },
            { DirectionGrid.Right, 90 },
            { DirectionGrid.Down, 180 },
            { DirectionGrid.Left, 270 }
        };

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private readonly object directionLock = new();
        private bool gameRunning;
        private ImageSource source;

        public MainWindow()
        {
            InitializeComponent();
            this.gridImages = SetUpGrid();
            this.gameState = new GameState(rows, cols);
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if(!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;  
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameState.GameOver)
            {
                return;
            }

            lock (directionLock)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        gameState.ChangeDirection(DirectionGrid.Up);
                        break;

                    case Key.Right:
                        gameState.ChangeDirection(DirectionGrid.Right);
                        break;

                    case Key.Down:
                        gameState.ChangeDirection(DirectionGrid.Down);
                        break;

                    case Key.Left:
                        gameState.ChangeDirection(DirectionGrid.Left);
                        break;
                }
            }
        }

        private async Task GameLoop()
        {
            while(!gameState.GameOver)
            {
                await Task.Delay(150);
                lock (directionLock)
                {
                    gameState.Move();
                }
                Draw();
            }
        }

        private Image[,] SetUpGrid()
        {
            Image[,] images = new Image[rows, cols];    
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for(int r = 0; r < rows; r++)
            {
                for(int c = 0; c < cols; c++)
                {
                    Image image = new()
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                } 
            }

            return images;
        }

        private void Draw()
        {
            Dispatcher.Invoke(() =>
            {
                DrawGrid();
            });
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        private void DrawGrid()
        {
            for(int r = 0; r < rows; r++)
            {
                for(int c = 0; c < cols ; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private void DrawSnakeHead()
        {
            PositionGrid headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnake()
        {
            List<PositionGrid> positionsGrid = new List<PositionGrid>(gameState.SnakePositions());

            for(int i = 0; i < positionsGrid.Count; i++)
            {
                PositionGrid pos = positionsGrid[i];
                ImageSource source = (i == 0) ? Images.SnakeHeadDead : Images.SnakeBodyDead;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }

        private async Task ShowCountDown()
        {
            for(int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(500);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "Press any key to play again";
        }
    }
}