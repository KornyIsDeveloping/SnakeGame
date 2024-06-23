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
            { GridValue.Empty, Image.Empty },
            { GridValue.Snake, Image.Body },
            { GridValue.Food, Image.Food }
        };

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetUpGrid();
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
                    Image image = new Image
                    {
                        Source = Images.Empty
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                } 
            }

            return images;
        }
    }
}