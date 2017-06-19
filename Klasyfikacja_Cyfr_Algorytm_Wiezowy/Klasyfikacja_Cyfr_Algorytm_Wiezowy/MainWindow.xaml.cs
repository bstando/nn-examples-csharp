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

namespace Klasyfikacja_Cyfr_Algorytm_Wiezowy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WriteableBitmap wejscie;
        Dane dane;
        EdytorObrazu edytor;
        int[] obraz;
        Wieza[] wieze;

        public MainWindow()
        {
            InitializeComponent();
            obraz = new int[35];
            dane = new Dane();
            edytor = new EdytorObrazu();
            wejscie = new WriteableBitmap(300, 420, 96, 96, PixelFormats.Bgr32, null);
            wejscie = edytor.WyczyscINalozLinie();
            obrazWejsciowy.Source = wejscie;
            wieze = new Wieza[10];
            for(int i=0;i<10;i++)
            {
                wieze[i] = new Wieza(35, i, 50);
            }
        }

        private void nauczBtn_Click(object sender, RoutedEventArgs e)
        {
            Parallel.For(0, 10, i =>
                {
                    wieze[i].UczWieze(dane.PobierzDane());
                    //MessageBox.Show("Zakonczono nauke: " +i);
                });
           
            MessageBox.Show("Zakonczono nauke");
        }

        private void rozpoznajBtn_Click(object sender, RoutedEventArgs e)
        {
            int rozpoznano = 0;
            for(int i=0;i<10;i++)
            {
                if(wieze[i].Klasyfikuj(obraz)==1)
                {
                    MessageBox.Show("Rozpoznano: " + i);
                    rozpoznano++;
                }
            }
            if(rozpoznano==0)
            {
                MessageBox.Show("Nic nie rozpoznano");
            }
            else if(rozpoznano>1)
            {
                MessageBox.Show("Rozpoznano wiecej niz jeden wzorzec");
            }
        }

        private void obrazWejsciowy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(obrazWejsciowy);

            int posX = (int)p.X;
            int posY = (int)p.Y;

            int startX = posX - posX % 60;
            int endX = startX + 60;
            int startY = posY - posY % 60;
            int endY = startY + 60;

            obraz[startX / 60 + (startY / 60) * 5] = 1;

            wejscie.FillRectangle(startX, startY, endX, endY, Color.FromRgb(0, 0, 0));
            obrazWejsciowy.Source = wejscie;
        }

        private void obrazWejsciowy_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(obrazWejsciowy);

            int posX = (int)p.X;
            int posY = (int)p.Y;

            int startX = posX - posX % 60;
            int endX = startX + 60;
            int startY = posY - posY % 60;
            int endY = startY + 60;

            obraz[startX / 60 + (startY / 60) * 5] = 0;

            wejscie.FillRectangle(startX, startY, endX, endY, Color.FromRgb(255, 255, 255));
            wejscie = edytor.NalozLinie(wejscie);
            obrazWejsciowy.Source = wejscie;
        }
    }
}
