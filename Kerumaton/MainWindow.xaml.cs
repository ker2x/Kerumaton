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


namespace Kerumaton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static WriteableBitmap bmp;
        Random rand;
        Stopwatch stopwatch;
        List<Automate> bots;

        public static int imageWidth = 80;
        public static int imageHeight = 60;

        public MainWindow()
        {
            InitializeComponent();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            rand = new Random();
            bmp = new WriteableBitmap(imageWidth, imageHeight, 96, 96, PixelFormats.Bgr32, null);
            bots = new List<Automate>();

            //Sample bot
            bots.Add(new Automate(10, 58, 1));
            bots.Add(new Automate(12, 58, 2));

            //Set xaml Image as the bmp & update the screen
            MainImage.Source = bmp;
            ScreenUpdate();
            bots.Add(new Automate(14, 58, 3));
            ScreenUpdate();
        }

        private void ScreenUpdate()
        {
            bmp.Lock();
            bmp.Clear(Colors.Black);
            foreach (var b in bots) bmp.SetPixel(b.pos.x, b.pos.y, Colors.Red);
            bmp.AddDirtyRect(new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight));
            bmp.Unlock();
        }

        private void MainImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
