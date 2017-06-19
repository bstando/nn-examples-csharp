using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asocjator_Graficzny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WriteableBitmap inBitmap,outBitmap;
        Dane dane;
        MaszynaLiniowa[] maszyny;
        int[] zapalonePiksele;
        int wysokosc;
        int szerokosc;
        int aktualnyPrzyklad;
        
        public MainWindow()
        {
            wysokosc = 50;
            szerokosc = 50;

           

            InitializeComponent();

            inBitmap = new WriteableBitmap((int)InImage.Width, (int)InImage.Height, 96, 96, PixelFormats.Bgr32, null);
            outBitmap = new WriteableBitmap((int)outImage.Width, (int)outImage.Height, 96, 96, PixelFormats.Bgr32, null);

            inBitmap.Clear(Color.FromRgb(255, 255, 255));
            outBitmap.Clear(Color.FromRgb(255, 255, 255));
            InImage.Source = inBitmap;
            outImage.Source = outBitmap;
            dane = new Dane(szerokosc, wysokosc);
            dane.WczytajPrzyklady();
            int[][] przyklady = dane.PobierzPrzyklady();
            maszyny = new MaszynaLiniowa[szerokosc * wysokosc];
            zapalonePiksele = new int[szerokosc * wysokosc];
            aktualnyPrzyklad = 0;

            for (int i = 0; i < szerokosc * wysokosc; i++)
            {
                maszyny[i] = new MaszynaLiniowa(i, szerokosc * wysokosc, 2, przyklady, dane.PobierzLiczbePrzykladow());
                zapalonePiksele[i] = 0;
            }
            Parallel.For(0, szerokosc * wysokosc, i => { maszyny[i].UczMaszyne(); });
            //Image a = new Image();
            //PixelFormat pf = new PixelFormat();
            //JpegBitmapDecoder jpg = new JpegBitmapDecoder(new Uri("test.jpg"),BitmapCreateOptions.None,BitmapCacheOption.Default);
            //a.Source = System.Windows.Media.Imaging.BitmapSource.Create(100, 100, 72, 72, pf, jpg.Palette, jpg.Frames);
        }

        

        private void InImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(InImage);
            inBitmap.SetPixel((int)p.X, (int)p.Y,Color.FromRgb(0,0,0));
            
        }

        private void InImage_MouseMove(object sender, MouseEventArgs e)
        {
            MouseButtonState mb = e.LeftButton;
            MouseButtonState rb = e.RightButton;
            if (MouseButtonState.Pressed == mb)
            {
                Point p = e.GetPosition(InImage);
               
                    int posX = (int)p.X;
                    int posY = (int)p.Y;
                    if ((posX >= 2) && (posY >= 2) && (posX < inBitmap.Width-2) && (posY < inBitmap.Height-2))
                    {
                        int skala = (int)inBitmap.Width / szerokosc;
                        inBitmap.FillRectangle(posX-2, posY-2, posX+2, posY+2, Color.FromRgb(0, 0, 0));
                        zapalonePiksele[posX/skala + (posY/skala) * szerokosc] = 1;
                    }                 
            }
            else if(MouseButtonState.Pressed == rb)
            {
                inBitmap.Clear(Color.FromRgb(255, 255, 255));
                Parallel.For(0, wysokosc * szerokosc, i => zapalonePiksele[i] = 0);
            }
            InImage.Source = inBitmap;
        }

        private void dodajDoBazy_btn_Click(object sender, RoutedEventArgs e)
        {
            //byte[] tab = inBitmap.ToByteArray();
            WriteableBitmap doBazy = inBitmap.Resize(szerokosc, wysokosc, WriteableBitmapExtensions.Interpolation.Bilinear);
            byte[] tab = doBazy.ToByteArray();
            int[] outTab = new int[szerokosc * wysokosc];
            for (int i = 0; i < szerokosc * wysokosc * 4; i += 4)
            {
                if(tab[i]!=0)
                {
                    outTab[i/4] = 0;
                }
                else
                {
                    outTab[i/4] = 1;
                }
            }
            Thread thread = new Thread(() => { dane.zapiszDoPliku(outTab); });
            thread.Start();
            thread.Join();
            MessageBox.Show("Zapisano!");
        }

        private void klasyfikuj_btn_Click(object sender, RoutedEventArgs e)
        {
            int[] tablicaWynikowa = new int[szerokosc * wysokosc];
            outBitmap.Clear(Color.FromRgb(255, 255, 255));
            WriteableBitmap doSkalowania = new WriteableBitmap(szerokosc, wysokosc, 96, 96, PixelFormats.Bgr32, null);
            Parallel.For(0, szerokosc * wysokosc, i => { tablicaWynikowa[i] = maszyny[i].Klasyfikuj(zapalonePiksele); });
            for (int i = 0; i < szerokosc * wysokosc; i ++)
            {
                if (tablicaWynikowa[i] == 0)
                {

                    doSkalowania.SetPixeli(i, Color.FromRgb(255, 255, 255));
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(255, 255, 255));

                }
                else
                {
                    doSkalowania.SetPixeli(i, Color.FromRgb(0, 0, 0));
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(0, 0, 0));
                }
            }

            outBitmap = doSkalowania.Resize(300, 300, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            outImage.Source = outBitmap;
            
        }

        private void wyglad_btn_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap doBazy = outBitmap.Resize(szerokosc, wysokosc, WriteableBitmapExtensions.Interpolation.Bilinear);
            byte[] tab = doBazy.ToByteArray();
            int[] outTab = new int[szerokosc * wysokosc];
            for (int i = 0; i < szerokosc * wysokosc * 4; i += 4)
            {
                if (tab[i] != 0)
                {
                    outTab[i / 4] = 0;
                }
                else
                {
                    outTab[i / 4] = 1;
                }
            }
            outBitmap.Clear(Color.FromRgb(255, 255, 255));
            int[] tablicaWynikowa = new int[szerokosc * wysokosc];
            WriteableBitmap doSkalowania = new WriteableBitmap(szerokosc, wysokosc, 96, 96, PixelFormats.Bgr32, null);
            Parallel.For(0, szerokosc * wysokosc, i => { tablicaWynikowa[i] = maszyny[i].Klasyfikuj(outTab); });
            for (int i = 0; i < szerokosc * wysokosc; i++)
            {
                if (tablicaWynikowa[i] == 0)
                {

                    doSkalowania.SetPixeli(i, Color.FromRgb(255, 255, 255));
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(255, 255, 255));

                }
                else
                {
                    doSkalowania.SetPixeli(i, Color.FromRgb(0, 0, 0));
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(0, 0, 0));
                }
            }

            outBitmap = doSkalowania.Resize(300, 300, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            outImage.Source = outBitmap;

        }

        private void nastepny_btn_Click(object sender, RoutedEventArgs e)
        {
            inBitmap.Clear(Color.FromRgb(255, 255, 255));
            WriteableBitmap doSkalowania = new WriteableBitmap(szerokosc, wysokosc, 96, 96, PixelFormats.Bgr32, null);
            int[] tablicaWynikowa = dane.PobierzPrzykad(aktualnyPrzyklad);
            for (int i = 0; i < szerokosc * wysokosc; i++)
            {
                if (tablicaWynikowa[i] == 0)
                {

                    doSkalowania.SetPixeli(i, Color.FromRgb(255, 255, 255));
                    zapalonePiksele[i] = 0;
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(255, 255, 255));

                }
                else
                {
                    doSkalowania.SetPixeli(i, Color.FromRgb(0, 0, 0));
                    zapalonePiksele[i] = 1;
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(0, 0, 0));
                }
            }
            inBitmap = doSkalowania.Resize(300, 300, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            InImage.Source = inBitmap;

            aktualnyPrzyklad = (aktualnyPrzyklad + 1) % dane.PobierzLiczbePrzykladow();
        }

        private void poprzedni_btn_Click(object sender, RoutedEventArgs e)
        {
            inBitmap.Clear(Color.FromRgb(255, 255, 255));
            WriteableBitmap doSkalowania = new WriteableBitmap(szerokosc, wysokosc, 96, 96, PixelFormats.Bgr32, null);
            int[] tablicaWynikowa = dane.PobierzPrzykad(aktualnyPrzyklad);
            for (int i = 0; i < szerokosc * wysokosc; i++)
            {
                if (tablicaWynikowa[i] == 0)
                {

                    doSkalowania.SetPixeli(i, Color.FromRgb(255, 255, 255));
                    zapalonePiksele[i] = 0;
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(255, 255, 255));

                }
                else
                {
                    doSkalowania.SetPixeli(i, Color.FromRgb(0, 0, 0));
                    zapalonePiksele[i] = 1;
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(0, 0, 0));
                }
            }
            inBitmap = doSkalowania.Resize(300, 300, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            InImage.Source = inBitmap;

            aktualnyPrzyklad = aktualnyPrzyklad > 0 ? --aktualnyPrzyklad : dane.PobierzLiczbePrzykladow() - 1;
        }

        private void zaszum_btn_Click(object sender, RoutedEventArgs e)
        {
            Random randomizer = new Random();
            int randomPixels = randomizer.Next(50);
            for (int i = 0; i < randomPixels; i++)
            {
                int pixelX = randomizer.Next(szerokosc);
                int pixelY = randomizer.Next(wysokosc);
                zapalonePiksele[pixelX + pixelY*szerokosc] = 1;
            }

            WriteableBitmap doSkalowania = new WriteableBitmap(szerokosc, wysokosc, 96, 96, PixelFormats.Bgr32, null);

            for (int i = 0; i < szerokosc * wysokosc; i++)
            {
                if (zapalonePiksele[i] == 0)
                {

                    doSkalowania.SetPixeli(i, Color.FromRgb(255, 255, 255));
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(255, 255, 255));

                }
                else
                {
                    doSkalowania.SetPixeli(i, Color.FromRgb(0, 0, 0));
                    //outBitmap.FillRectangle(i % 50 - 2, (i - i % 50 - 2) / 50, i % 50 + 2, (i - i % 50 + 2) / 50, Color.FromRgb(0, 0, 0));
                }
            }
            inBitmap = doSkalowania.Resize(300, 300, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            InImage.Source = inBitmap;
        }
    }
}
