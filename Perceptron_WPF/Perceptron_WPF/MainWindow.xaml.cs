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

namespace Perceptron_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[] zaznaczonePola;
        SPLA nauczyciel;
        Perceptron[] p;
        Brush defaultColor;
        double stalaUczenia = 0.1;
        public MainWindow()
        {
           
            zaznaczonePola = new int[25];

            for(int i=0;i<25;i++)
            {
                zaznaczonePola[i] = 0;
            }

            nauczyciel = new SPLA(zaznaczonePola);
            p = new Perceptron[10];

            for (int i = 0; i < 10;i++)
            {
                p[i] = new Perceptron();
                p[i].LosujWagi();
            }

            
            nauczyciel.nauczPerceptrony(p,stalaUczenia);

            InitializeComponent();

            defaultColor = button.Background;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[0] == 0)
            {
                zaznaczonePola[0] = 1;
                button.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[0] = 0;
                button.Background = defaultColor;
            }
         }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[1] == 0)
            {
                zaznaczonePola[1] = 1;
                button1.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[1] = 0;
                button1.Background = defaultColor;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[2] == 0)
            {
                
                zaznaczonePola[2] = 1;
                button2.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[2] = 0;
                button2.Background = defaultColor;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[3] == 0)
            {
                
                zaznaczonePola[3] = 1;
                button3.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[3] = 0;
                button3.Background = defaultColor;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[4] == 0)
            {
                
                zaznaczonePola[4] = 1;
                button4.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[4] = 0;
                button4.Background = defaultColor;
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[5] == 0)
            {
                
                zaznaczonePola[5] = 1;
                button5.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[5] = 0;
                button5.Background = defaultColor;
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[6] == 0)
            {
                
                zaznaczonePola[6] = 1;
                button6.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[6] = 0;
                button6.Background = defaultColor;
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[7] == 0)
            {
                
                zaznaczonePola[7] = 1;
                button7.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[7] = 0;
                button7.Background = defaultColor;
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[8] == 0)
            {
                
                zaznaczonePola[8] = 1;
                button8.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[8] = 0;
                button8.Background = defaultColor;
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[9] == 0)
            {
                
                zaznaczonePola[9] = 1;
                button9.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[9] = 0;
                button9.Background = defaultColor;
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[10] == 0)
            {
                
                zaznaczonePola[10] = 1;
                button10.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[10] = 0;
                button10.Background = defaultColor;
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[11] == 0)
            {
                
                zaznaczonePola[11] = 1;
                button11.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[11] = 0;
                button11.Background = defaultColor;
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[12] == 0)
            {
                
                zaznaczonePola[12] = 1;
                button12.Background = Brushes.Black;


            }
            else
            {
                
                zaznaczonePola[12] = 0;
                button12.Background = defaultColor;
            }
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[13] == 0)
            {
 
                zaznaczonePola[13] = 1;
                button13.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[13] = 0;
                button13.Background = defaultColor;
            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[14] == 0)
            {
                zaznaczonePola[14] = 1;
                button14.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[14] = 0;
                button14.Background = defaultColor;
            }
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[15] == 0)
            {
                zaznaczonePola[15] = 1;
                button15.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[15] = 0;
                button15.Background = defaultColor;
            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[16] == 0)
            {
                zaznaczonePola[16] = 1;
                button16.Background = Brushes.Black;
            }
            else
            {
                zaznaczonePola[16] = 0;
                button16.Background = defaultColor;
            }
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[17] == 0)
            {
                zaznaczonePola[17] = 1;
                button17.Background = Brushes.Black;


            }
            else
            {
               
                zaznaczonePola[17] = 0;
                button17.Background = defaultColor;
            }
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[18] == 0)
            {
                zaznaczonePola[18] = 1;
                button18.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[18] = 0;
                button18.Background = defaultColor;
            }
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[19] == 0)
            {
                zaznaczonePola[19] = 1;
                button19.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[19] = 0;
                button19.Background = defaultColor;
            }
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[20] == 0)
            {
                zaznaczonePola[20] = 1;
                button20.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[20] = 0;
                button20.Background = defaultColor;
            }
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[21] == 0)
            {
                zaznaczonePola[21] = 1;
                button21.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[21] = 0;
                button21.Background = defaultColor;
            }
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[22] == 0)
            {
                zaznaczonePola[22] = 1;
                button22.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[22] = 0;
                button22.Background = defaultColor;
            }
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[23] == 0)
            {
                zaznaczonePola[23] = 1;
                button23.Background = Brushes.Black;


            }
            else
            {
                zaznaczonePola[23] = 0;
                button23.Background = defaultColor;
            }
        }

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            if (zaznaczonePola[24] == 0)
            {
                zaznaczonePola[24] = 1;
                button24.Background = Brushes.Black;
            }
            else
            {
                zaznaczonePola[24] = 0;
                button24.Background = defaultColor;
            }
        }

        private void Uczenie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int wartosc = Int32.Parse(poleNauki.Text);
                
                if (!((wartosc < 0) || (wartosc > 9)))
                {
                    nauczyciel.zapiszDoPliku(zaznaczonePola, wartosc);
                    //nauczyciel.losujWagiPerceptornow(p);
                    nauczyciel.nauczPerceptrony(p, stalaUczenia);
                    MessageBox.Show("Zakonczono nauke");
                    
                }
                else
                {
                    MessageBox.Show("W polu nie znajduje się liczba z przedziału 0-9");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show("W polu nie znajduje się liczba");
            }
        }

        private void Klasyfikator_Click(object sender, RoutedEventArgs e)
        {
            int ilosc = 0;
            for(int i=0;i<10;i++)
            {
                if(p[i].sprawdz(zaznaczonePola)==1)
                {
                    String message = "Wykryto liczbe: " + i;
                    MessageBox.Show(message);
                    ilosc++;
                }
            }
            if(ilosc==0)
            {
                String message = "Nie rozpoznano liczby";
                MessageBox.Show(message);
              
            }
            if(ilosc>1)
            {
                String message = "Rozpoznano wiecej niż jedną liczbę";
                MessageBox.Show(message);
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.OldValue != e.NewValue)
            {
                poleStalej.Content = "Stala: " + e.NewValue;
                stalaUczenia = e.NewValue;
                //nauczyciel.losujWagiPerceptornow(p);
                nauczyciel.nauczPerceptrony(p, stalaUczenia);
               // MessageBox.Show("Zakonczono nauke");
            }
        }
    }
}
