using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SnakeGame
{
    public static class Images
    {
        public readonly static ImageSource Head = LoadImage("SnakeHead.png");
        public readonly static ImageSource Body = LoadImage("SnakeBody.png");
        public readonly static ImageSource SnakeHeadDead = LoadImage("SnakeHeadDead.png");
        public readonly static ImageSource SnakeBodyDead = LoadImage("SnakeBodyDead.png");
        public readonly static ImageSource Food = LoadImage("Food.png");
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}
