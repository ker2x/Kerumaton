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
        public static int framecount;
        WorldGrid worldGrid;
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
            bmp = new WriteableBitmap(world.imageWidth, world.imageHeight, 96, 96, PixelFormats.Bgr32, null);
            worldGrid = new WorldGrid();

            world.bots = new List<Automate>();
            var tasks = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);

            //Init some values
            MainImage.Source = bmp;
            framecount = 0;

            //Sample bot
            for (int i = 0; i < world.maxAutomaton; i++)
            {
                world.SpawnAutomaton();
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
                world.Tick();

                _ = Parallel.ForEach(world.bots, (bot) =>
                  {
                      bot.Tick();
                  });
            }
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {

            framecount++;

            bmp.Lock();

            //Clear screen
            bmp.Clear(Colors.Black);

            //Draw grid
            Color GridColor = Color.FromRgb(50, 50, 50);
            for (int y = 0; y < world.imageHeight; y++)
            {
                for (int x = 0; x < world.imageHeight; x++)
                {
                    if (y % WorldGrid.gridHeight == 0 || x % WorldGrid.gridWidth == 0) { bmp.SetPixel(x, y, GridColor); }
                }
            }

            //Draw bots
            foreach (var b in world.bots.ToArray())
            {
                //bmp.FillEllipseCentered(b.pos.x, b.pos.y, 3, 3, b.Color);
                bmp.FillRectangle(
                    b.pos.x - (WorldGrid.gridWidth / 2), b.pos.y - (WorldGrid.gridHeight / 2), 
                    b.pos.x + (WorldGrid.gridWidth / 2), b.pos.y + (WorldGrid.gridHeight / 2), 
                    b.Color);
                //bmp.SetPixel(b.pos.x, b.pos.y, b.Color);
            }

            bmp.AddDirtyRect(new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));
            bmp.Unlock();

            CreatureCountLabel.Text = $"Creature Count : {world.bots.Count}";
            WorldLifetimeLabel.Text = $"World Lifetime : {world.lifetime}";
            FrameCounterLabel.Text = $"Frame counter : { MainWindow.framecount}";
        }

    }
}