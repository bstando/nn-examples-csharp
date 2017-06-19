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

namespace Rozpoznawanie_Cyfr_Adaline
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WriteableBitmap obrazek;
        EdytorObrazu edytor;
        Adaline[] jednostki;
        Dane dane;
        Transformata transformata;
        int[] tablica;
        bool isRunning = false;
        bool wasRunned = false;
        public MainWindow()
        {
            InitializeComponent();
            obrazek = new WriteableBitmap(300, 420, 96, 96, PixelFormats.Bgr32, null);
            edytor = new EdytorObrazu();
            obrazek = edytor.WyczyscINalozLinie();
            rozpoznawanyObraz.Source = obrazek;
            jednostki = new Adaline[10];

            for (int i = 0; i < 10; i++)
            {
                jednostki[i] = new Adaline(i);
            }

            dane = new Dane();
            transformata = new Transformata();
            tablica = new int[35];

        }

        private void Wyczysctablice()
        {
            Parallel.For(0, 35, i => tablica[i] = 0);
        }

        //Metoda ucząca jednostki wywoływana po wciśnięciu przycisku "Rozpocznij naukę"
        private void uczBtn_Click(object sender, RoutedEventArgs e)
        {
            //Pobieranie danych uczących
            int[][] pobrane = dane.PobierzDane();
            //Pobranie ilości wczytanych danych
            int wczytano = dane.Wczytano;
            //Wykonanie transformaty na danych uczących
            double[][] wykonanaTransformata = transformata.WykonajTrasnformate(pobrane);
            MessageBox.Show("Uczenie Rozpoczete");
            //Równoległa pętla ucząca każdą jednostkę
            Parallel.For(0, 10, i =>
                {
                    jednostki[i].UczJednostke(pobrane, wykonanaTransformata, wczytano, 0.006, 0.01);
                });

            MessageBox.Show("Uczenie Zakonczone");

        }
        //Metoda służąca do klasyfikacji obrazu narysowanego przez użytkownika. Wykonywana jest po wciśnięciu przycisku "Rozpoznaj"
        private void rozpoznajBtn_Click(object sender, RoutedEventArgs e)
        {
            //Wygenerowanie transformaty dla przykładu i jej wyświtlenie
            WizualizacjaDFT dft = new WizualizacjaDFT(transformata.WykonajTransformateDlaPrzykadu(tablica));
            dft.Show();
            List<double> wyniki = new List<double>();
            //Pętla służąca do klasyfikacji obrazu przez poszczególne neurony
            for (int i = 0; i < 10; i++)
            {
                //Wykonanie klasyfikacji
                double wynik = jednostki[i].Klasyfikuj(tablica, transformata.WykonajTransformateDlaPrzykadu(tablica)) - 1;
                //Dodanie do wyniku do wektora wizualizującego wyniki
                wyniki.Add(jednostki[i].Klasyfikuj(tablica, transformata.WykonajTransformateDlaPrzykadu(tablica)));
                if ((wynik <= 0.1) && (wynik >= -0.1))
                    MessageBox.Show("Rozpoznano: " + i);
            }
            //Wyświetlenie wektora wyników
            dane.ZapiszDaneDoPliku(wyniki);
            WykresWynikow w = new WykresWynikow();
            w.Show();
        }

        private void rozpoznawanyObraz_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(rozpoznawanyObraz);

            int posX = (int)p.X;
            int posY = (int)p.Y;

            int startX = posX - posX % 60;
            int endX = startX + 60;
            int startY = posY - posY % 60;
            int endY = startY + 60;

            tablica[startX / 60 + (startY / 60) * 5] = 1;

            obrazek.FillRectangle(startX, startY, endX, endY, Color.FromRgb(0, 0, 0));
            rozpoznawanyObraz.Source = obrazek;
        }

        private void rozpoznawanyObraz_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(rozpoznawanyObraz);

            int posX = (int)p.X;
            int posY = (int)p.Y;

            int startX = posX - posX % 60;
            int endX = startX + 60;
            int startY = posY - posY % 60;
            int endY = startY + 60;

            tablica[startX / 60 + (startY / 60) * 5] = 0;

            obrazek.FillRectangle(startX + 1, startY + 1, endX - 1, endY - 1, Color.FromRgb(255, 255, 255));
            rozpoznawanyObraz.Source = obrazek;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isRunning)
                for (int i = 0; i < 10; i++)
                {
                    jednostki[i].czyDziala = false;
                }
        }

        private void pozkazWykres_btn_Click(object sender, RoutedEventArgs e)
        {
            List<double>[] lista = new List<double>[10];
            int max = jednostki[0].ZwrocBledy().Count;
            for (int i = 0; i < 10; i++)
            {
                lista[i] = jednostki[i].ZwrocBledy();
                //MessageBox.Show("i: " + i + ", ilosc: " + lista[i].Count);
                if (max < lista[i].Count) max = lista[i].Count;
            }
            List<double> wynik = new List<double>();
            for (int k = 0; k < max; k++)
            {
                double tmp = 0;
                for (int i = 0; i < 10; i++)
                {
                    if (lista[i].Count > k)
                    {
                        tmp += lista[i].ElementAt(k);
                    }
                }
                wynik.Add(tmp * tmp);
            }


            dane.ZapiszBladDoPliku(wynik);
            Wykres wykres = new Wykres();
            wykres.Show();
        }
    }
}
