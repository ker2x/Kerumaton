using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;     //WriteableBitmap
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Concurrent;    //BlockingCollection<T>
using System.Diagnostics;               // stopwatch
using System.Threading.Tasks;           //Parallel stuff & task factory


namespace Kerumaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Declaration
        public static WriteableBitmap bmp;
        Random rand;
        Stopwatch stopwatch;
        List<Automate> bots;
        BlockingCollection<Automate> automateQueue;

        public static int imageWidth = 1000;
        public static int imageHeight = 1000;

        public MainWindow()
        {
            //MS Stuff
            InitializeComponent();
            CompositionTarget.Rendering += new EventHandler(CompositionTargetRendering);
            //Clock
            stopwatch = new Stopwatch();
            stopwatch.Start();

            //Call some required constructor
            rand = new Random();
            bmp = new WriteableBitmap(imageWidth, imageHeight, 96, 96, PixelFormats.Bgr32, null);
            bots = new List<Automate>();
            var tasks = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

            //Init some values
            MainImage.Source = bmp;

            //Sample bot
            for (int i = 0; i < 5000; i++)
            {
                bots.Add(new Automate(rand.Next(0,imageWidth-1), rand.Next(0, imageHeight-1), i));
            }
            //Set xaml Image as the bmp & update the screen
            tasks.StartNew(() => MainLoop());
        }

        private void MainLoop()
        {
            while (true)
            {
                Parallel.ForEach(bots, (bot) =>
                {
                    double closestDistance = int.MaxValue;
                    Automate.Direction closestDir = bot.RandomDirection();
                    Automate.Direction tmpDirx, tmpDiry;
                    
                    foreach (var b in bots)
                    {
                        double distance = Math.Sqrt(
                            (b.pos.x - bot.pos.x)*(b.pos.x - bot.pos.x) + (b.pos.y - bot.pos.y)*(b.pos.y - bot.pos.y));

                        if (distance < closestDistance && distance != 0)
                        {
                            tmpDirx = (bot.pos.x < b.pos.x) ? Automate.Direction.W : Automate.Direction.E;
                            tmpDiry = (bot.pos.y < b.pos.y) ? Automate.Direction.N : Automate.Direction.S;
                            closestDir = (Math.Abs(b.pos.x - bot.pos.x) < Math.Abs(b.pos.y - bot.pos.y)) ? tmpDirx : tmpDiry;
                            closestDistance = distance;
                        }
                    }
                    if (rand.Next(10000) == 1) { bot.pos.y = rand.Next(imageHeight); bot.pos.x = rand.Next(imageWidth); }
                    else
                        bot.Move(closestDir);
                });
            }
        }

        private void ScreenUpdate()
        {
        }

        void CompositionTargetRendering(object sender, EventArgs e)
        {
            //if (rand.Next(100) == 1)
            {
                bmp.Lock();
                bmp.Clear(Colors.Black);
                foreach (var b in bots)
                {
                    //bmp.SetPixel(b.pos.x, b.pos.y, b.color);
                    bmp.FillEllipseCentered(b.pos.x, b.pos.y, 3, 3, b.color);
                }
                bmp.AddDirtyRect(new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));
                bmp.Unlock();
            }
        }

        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //imageWidth = bmp.PixelWidth;
            //imageHeight = bmp.PixelHeight;
        }
    }
}
