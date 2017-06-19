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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Autoasocjator_Hopfield
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WriteableBitmap wejscie;
        WriteableBitmap wyjscie;
        Hopfield siec;
        double[] obraz;
        List<double[]> przyklady;
        int aktualnyPrzyklad = 0;
        public MainWindow()
        {
            InitializeComponent();
            wejscie = new WriteableBitmap(250, 250, 96, 96, PixelFormats.Bgr32, null);
            wyjscie = new WriteableBitmap(250, 250, 96, 96, PixelFormats.Bgr32, null);
            wejscie.Clear(Color.FromRgb(255, 255, 255));
            wyjscie.Clear(Color.FromRgb(255, 255, 255));
            obrazWejsciowy.Source = wejscie;
            obrazWyjsciowy.Source = wyjscie;
            siec = new Hopfield(625);
            obraz = new double[625];
            for(int i=0;i<625;i++)
            {
                obraz[i] = -1;
            }
            przyklady = new List<double[]>();
            ProcesujPrzyklady();
            for (int i = 0; i < przyklady.Count(); i++)
            {
                siec.Ucz(przyklady.ElementAt(i));
            }
        }

        void ProcesujPrzyklady()
        {
            for (int i = 1; i < 3; i++)
            {
                string nazwa = string.Empty + i + ".jpg";
                BitmapImage img = new BitmapImage(new Uri(@nazwa, UriKind.Relative));
                WriteableBitmap w = new WriteableBitmap(img);
                double[] tablica = new double[625];
                for (int j = 0; j < 25; j++)
                {
                    for (int k = 0; k < 25; k++)
                    {
                        if (w.GetPixel(j, k).G < 200)
                        {
                            tablica[j + 25 * k] = 1;
                        }
                        else
                        {
                            tablica[j + 25 * k] = -1;
                        }
                    }
                }
                przyklady.Add(tablica);
            }
        }
        WriteableBitmap zamaluj(double[] tablica)
        {
            WriteableBitmap gotowe = new WriteableBitmap(25, 25, 96, 96, PixelFormats.Bgr32, null);
            for(int i=0;i<25;i++)
            {
                for(int j=0;j<25;j++)
                {
                    if(tablica[i+25*j]==-1)
                    {
                        gotowe.SetPixel(i, j, Color.FromRgb(255, 255, 255));
                    }
                    else
                    {
                        gotowe.SetPixel(i, j, Color.FromRgb(0, 0, 0));
                    }
                }
            }
            return gotowe.Resize(250, 250, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
        }
        private void rozpoznajBtn_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap skala = wejscie.Resize(25, 25, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            
            double[] wynik = siec.Klasyfikuj(obraz);
            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (wynik[i + (j * 25)] == -1)
                    {
                        skala.SetPixel(i, j, Color.FromRgb(255, 255, 255));
                    }
                    else
                    {
                        skala.SetPixel(i, j, Color.FromRgb(0, 0, 0));
                    }
                }
            }
            wyjscie = skala.Resize(250, 250, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            obrazWyjsciowy.Source = wyjscie;
            MessageBoxResult r = MessageBox.Show("Czy wykonac dodatkowe wygladzanie?", "Pytanie", MessageBoxButton.OKCancel);
            if(r==MessageBoxResult.OK)
            {
                for(int k=0;k<10;k++)
                {
                    double[] wynik2 = siec.Klasyfikuj(wynik);
                    for (int i = 0; i < 25; i++)
                    {
                        for (int j = 0; j < 25; j++)
                        {
                            if (wynik2[i + (j * 25)] == -1)
                            {
                                skala.SetPixel(i, j, Color.FromRgb(255, 255, 255));
                            }
                            else
                            {
                                skala.SetPixel(i, j, Color.FromRgb(0, 0, 0));
                            }
                        }
                    }
                    wyjscie = skala.Resize(250, 250, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
                    obrazWyjsciowy.Source = wyjscie;
                    wynik = wynik2;
                }
            }
        }

        private void obrazWejsciowy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(obrazWejsciowy);

            int posX = (int)p.X;
            int posY = (int)p.Y;

            int startX = posX - posX % 10;
            int endX = startX + 10;
            int startY = posY - posY % 10;
            int endY = startY + 10;

            obraz[startX / 10 + (startY / 10) * 25] = 1;

            wejscie.FillRectangle(startX, startY, endX, endY, Color.FromRgb(0, 0, 0));
            obrazWejsciowy.Source = wejscie;
        }

        private void obrazWejsciowy_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(obrazWejsciowy);

            int posX = (int)p.X;
            int posY = (int)p.Y;

            int startX = posX - posX % 10;
            int endX = startX + 10;
            int startY = posY - posY % 10;
            int endY = startY + 10;

            obraz[startX / 10 + (startY / 10) * 25] = -1;

            wejscie.FillRectangle(startX, startY, endX, endY, Color.FromRgb(255, 255, 255));
            obrazWejsciowy.Source = wejscie;
        }

        private void uczBtn_Click(object sender, RoutedEventArgs e)
        {
            przyklady.Add(obraz);
            for (int i = 0; i < przyklady.Count(); i++)
            {
                siec.Ucz(przyklady.ElementAt(i));
            }
            MessageBox.Show("Nauczono");
        }

        private void nastepnyBtn_Click(object sender, RoutedEventArgs e)
        {
            obraz = przyklady.ElementAt(aktualnyPrzyklad);
            wejscie = zamaluj(obraz);
            aktualnyPrzyklad = (aktualnyPrzyklad + 1) % przyklady.Count;
            obrazWejsciowy.Source = wejscie;
        }

        private void poprzedniBtn_Click(object sender, RoutedEventArgs e)
        {
            obraz = przyklady.ElementAt(aktualnyPrzyklad);
            wejscie = zamaluj(obraz);
            aktualnyPrzyklad = ((aktualnyPrzyklad - 1)+przyklady.Count) % przyklady.Count;
            obrazWejsciowy.Source = wejscie;
        }

        private void czyscBtn_Click(object sender, RoutedEventArgs e)
        {
            wejscie.Clear(Color.FromRgb(255, 255, 255));
            for(int i =0; i<625;i++)
            {
                obraz[i] = -1;
            }
            obrazWejsciowy.Source = wejscie;
        }


    }
}
