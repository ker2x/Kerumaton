using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;           //Parallel stuff & task factory
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;     //WriteableBitmap

namespace Kerumaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Declaration
        public static WriteableBitmap bmp;

        private readonly Random rand;
        //private Stopwatch stopwatch;
        //private BlockingCollection<Automate> automateQueue;


        public MainWindow()
        {
            //MS Stuff
            InitializeComponent();
            CompositionTarget.Rendering += new EventHandler(CompositionTargetRendering);
            //Clock
            //stopwatch = new Stopwatch();
            //stopwatch.Start();

            //Call some required constructor
            rand = new Random();
            bmp = new WriteableBitmap(World.imageWidth, World.imageHeight, 96, 96, PixelFormats.Bgr32, null);
            World.bots = new List<Automate>();
            var tasks = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

            //Init some values
            MainImage.Source = bmp;

            //Sample bot
            for (int i = 0; i < World.maxAutomaton; i++)
            {
                World.SpawnAutomaton();
            }
            //Set xaml Image as the bmp & update the screen
            _ = tasks.StartNew(() => MainLoop());
        }

        //MainLoop is in a manually started task.
        public static void MainLoop()
        {
            Random rand = new Random();
            Thread.Sleep(3000);

            while (true)
            {
                _ = Parallel.ForEach(World.bots, (bot) =>
                  {
                      bot.SampleTick();
                  });
                if (rand.Next(1) == 0)
                {
                    World.SpawnAutomaton();
                }
                //lock(World.bots)
                {
                    if (rand.Next(1000) == 0 && World.bots.Count > 1)
                    {
                        World.bots.Take(1);
                    }
                }
            }
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            bmp.Lock();

            //Clear screen
            bmp.Clear(Colors.Black);

            //Draw grid
            Color GridColor = Color.FromRgb(50, 50, 50);
            for(int y = 0; y < World.imageHeight; y++)
            {
                for(int x = 0; x < World.imageHeight; x++)
                {
                    if (y % Grid.gridHeight == 0 || x % Grid.gridWidth == 0) { bmp.SetPixel(x, y, GridColor); }
                }
            }

            //Draw bots
            foreach (var b in World.bots.ToArray())
            {
                bmp.FillEllipseCentered(b.pos.x, b.pos.y, 3, 3, b.Color);
                //bmp.SetPixel(b.pos.x, b.pos.y, b.Color);
            }

            bmp.AddDirtyRect(new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));
            bmp.Unlock();

            CreatureCountLabel.Text = $"Creature Count : {World.bots.Count}";
        }

        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}