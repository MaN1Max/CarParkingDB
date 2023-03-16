using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace CarParking
{
    public partial class Game : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        Random rnd = new Random();

        bool goLeft;
        bool goRight;
        bool isGameOver;

        int score;
        int ballX;
        int Bx;
        int ballY;
        int By;
        int gameSpeed;

        public Game()
        {
            InitializeComponent();
            GameFrame.Focus();
            gameSetup();
        }

        private void gameSetup() //start game setup
        {
            score = 0;
            ballX = 2;
            ballY = 2;
            gameSpeed = 15;

            Score.Content = "Score: " + score;

            gameTimer.Tick += new EventHandler(gameTimer_Tick);
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            gameTimer.Start();

            foreach (var x in GameFrame.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "Block")
                {
                    x.Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(255), (byte)rnd.Next(255), (byte)rnd.Next(255)));
                }
            }
        }

        private void gameOver(string message) //end of game
        {
            isGameOver = true;

            Score.Content = "Score: " + score + "\n" + message;

            gameTimer.Stop();
        }

        private void gameTimer_Tick(object sender, EventArgs e) //timer game process
        {
            Score.Content = "Score: " + score;

            if (goLeft == true && Canvas.GetLeft(Platform) > 0)
            {
                Canvas.SetLeft(Platform, Canvas.GetLeft(Platform) - gameSpeed);
            }
            if (goRight == true && Canvas.GetLeft(Platform) + (Platform.Width + 20) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Platform, Canvas.GetLeft(Platform) + gameSpeed);
            }

            Bx = (int)Canvas.GetLeft(Ball);
            Bx += ballX;
            Canvas.SetLeft(Ball, Bx);

            By = (int)Canvas.GetTop(Ball);
            By += ballY;
            Canvas.SetTop(Ball, By);

            if (Canvas.GetLeft(Ball) < 0 || Canvas.GetLeft(Ball) + (Ball.Width + 20) > Application.Current.MainWindow.Width)
            {
                ballX = -ballX;
            }
            if (Canvas.GetTop(Ball) < 0)
            {
                ballY = -ballY;
            }

            Rect BallHitBox = new Rect(Canvas.GetLeft(Ball), Canvas.GetTop(Ball), Ball.Width, Ball.Height);

            var rects = GameFrame.Children.OfType<Rectangle>().ToList();

            foreach (var x in rects) //collision
            {
                if ((string)x.Tag == "Block")
                {
                    Rect BlockHitBoxTop = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, 1);
                    Rect BlockHitBoxDown = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x) + x.Height, x.Width, 1);
                    Rect BlockHitBoxLeft = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x) + 1, 1, x.Height - 1);
                    Rect BlockHitBoxRight = new Rect(Canvas.GetLeft(x) + x.Width, Canvas.GetTop(x) + 1, 1, x.Height - 1);

                    if (BallHitBox.IntersectsWith(BlockHitBoxTop))
                    {
                        ballY = -ballY;
                        score += 1;
                        Canvas.SetTop(Ball, Canvas.GetTop(x) - Ball.Height);
                        GameFrame.Children.Remove(x);
                    }
                    if (BallHitBox.IntersectsWith(BlockHitBoxDown))
                    {
                        ballY = -ballY;
                        score += 1;
                        Canvas.SetBottom(Ball, Canvas.GetBottom(x));
                        GameFrame.Children.Remove(x);
                    }
                    if (BallHitBox.IntersectsWith(BlockHitBoxLeft))
                    {
                        ballX = -ballX;
                        score += 1;
                        Canvas.SetLeft(Ball, Canvas.GetLeft(x) - Ball.Width);
                        GameFrame.Children.Remove(x);
                    }
                    if (BallHitBox.IntersectsWith(BlockHitBoxRight))
                    {
                        ballX = -ballX;
                        score += 1;
                        Canvas.SetRight(Ball, Canvas.GetRight(x));
                        GameFrame.Children.Remove(x);
                    }
                }
                if ((string)x.Tag == "Platform")
                {
                    Rect PlatformHitBox = new Rect(Canvas.GetLeft(Platform), Canvas.GetTop(Platform), Platform.Width, Platform.Height);

                    if (BallHitBox.IntersectsWith(PlatformHitBox))
                    {
                        ballY = rnd.Next(3, 13) * -1;
                        Canvas.SetTop(Ball, Canvas.GetTop(Platform) - Ball.Height);
                    }
                }
            }

            if (!rects.Exists(x => (string)x.Tag == "Block"))
            {
                gameOver("You broke all the blocks! You WON! Press 'R' to restart.");
            }
            if (Canvas.GetTop(Ball) > Application.Current.MainWindow.Height)
            {
                gameOver("You lost the ball! You LOSE! Press 'R' to restart.");
            }
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true;
            }
            if (e.Key == Key.Right)
            {
                goRight = true;
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false;
            }
            if (e.Key == Key.Right)
            {
                goRight = false;
            }
            if (e.Key == Key.R && isGameOver == true)
            {
                Application.Current.Shutdown();
                System.Windows.Forms.Application.Restart();
            }
        }
    }
}
