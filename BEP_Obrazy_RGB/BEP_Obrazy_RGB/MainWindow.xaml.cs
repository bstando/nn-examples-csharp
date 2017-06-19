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

namespace BEP_Obrazy_RGB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int tryb;
        WriteableBitmap wejscie;
        WriteableBitmap wyjscie;
        WriteableBitmap test;
        BEP_CPU CPU;
        BEP_CUDA GPU;
        
        List<float> iter;
        int liczbaIteracji = 1;
        float eta = 0;
        public MainWindow(int start)
        {
            tryb = start;
            InitializeComponent();

            BitmapImage img = new BitmapImage(new Uri("Lenna.png", UriKind.Relative));
            //BitmapImage img = new BitmapImage(new Uri("smallpic2.jpg", UriKind.Relative));
            iter = new List<float>();

            wejscie = new WriteableBitmap(img);

            test = wejscie.Resize(50, 50, WriteableBitmapExtensions.Interpolation.Bilinear);
            wyjscie = new WriteableBitmap(512, 512, 96, 96, PixelFormats.Bgr32, null);

            obrazWejsciowy.Source = wejscie.Resize(512, 512, WriteableBitmapExtensions.Interpolation.NearestNeighbor);

            if (tryb == TrybStartowy.CPU)
            {
                CPU = new BEP_CPU(14, 3, 2);
                CPU.LosujWagi();
                CPU.UtworzMacierze();
            }
            else if (tryb == TrybStartowy.CUDA)
            {
                GPU = new BEP_CUDA(14, 3, 2);
            }
           
            
           
            //for (int i = 0; i < 50; i++)
            //{
            //    for (int j = 0; j < 50; j++)
            //    {
            //        //CPU.Ucz(i / 255, j / 255, 0.06, wejscie.GetPixel(i, j));
            //        wyjscie.SetPixel(i, j, CPU.ZwrocKolor((float)i / 255, (float)j / 255));
            //    }
            //}
            wyjscie = test.Resize(512, 512, WriteableBitmapExtensions.Interpolation.Bilinear);
            obrazWyjscowy.Source = wyjscie;




            //for (int i = 0; i < 512; i++)
            //{
            //    for (int j = 0; j < 512; j ++)
            //        {
            //            wyjscie.SetPixel(i, j, CPU.ZwrocKolor(i / 255, j / 255));
            //        } 
            //}

            obrazWyjscowy.Source = wyjscie;
            //Color kolor = CPU.ZwrocKolor(0.3, 0.4);
            //MessageBox.Show("R: " + kolor.R + ", G: " + kolor.G + ", B:" + kolor.B);
            MessageBoxResult res = MessageBox.Show("Czy wykonać przykładowy test?\n Może potrwać bardzo długo(ponad 3 godziny)", "Test", MessageBoxButton.YesNo);
            if(res==MessageBoxResult.Yes)
            {
                Test();
            }
        }

        void Test()
        {
            DateTime start = DateTime.Now;
            
            float etaTestowa = 1;
            for (int k = 0; k < 10; k++)
            {
                
                test.Clear(Color.FromRgb(255, 255, 255));
                //test = wejscie.Resize(20, 20, WriteableBitmapExtensions.Interpolation.Bilinear);
                test = wejscie.Resize(50, 50, WriteableBitmapExtensions.Interpolation.Bilinear);
                float[] wej = new float[14];
                for (int w = 0; w < 1000; w++)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        for (int j = 0; j < 50; j++)
                        {
                            wej[0] = (float)i / 50f;
                            wej[1] = (float)j / 50f;
                            wej[2] = (float)Math.Sin(wej[0] * 2 * Math.PI);
                            wej[3] = (float)Math.Sin(wej[1] * 2 * Math.PI);
                            wej[4] = (float)Math.Cos(wej[0] * 2 * Math.PI);
                            wej[5] = (float)Math.Cos(wej[1] * 2 * Math.PI);
                            wej[6] = (float)Math.Sin(2 * wej[0] * 2 * Math.PI);
                            wej[7] = (float)Math.Sin(2 * wej[1] * 2 * Math.PI);
                            wej[8] = (float)Math.Cos(2 * wej[0] * 2 * Math.PI);
                            wej[9] = (float)Math.Cos(2 * wej[1] * 2 * Math.PI);
                            wej[10] = (float)Math.Sin(3 * wej[0] * 2 * Math.PI);
                            wej[11] = (float)Math.Sin(3 * wej[1] * 2 * Math.PI);
                            wej[12] = (float)Math.Cos(3 * wej[0] * 2 * Math.PI);
                            wej[13] = (float)Math.Cos(3 * wej[1] * 2 * Math.PI);
                            //Color k = test.GetPixel(i, j);
                            //wej[14] = k.ScR;
                            //wej[15] = k.ScG;
                            //wej[16] = k.ScB;
                            CPU.UczMacierzowo(wej, etaTestowa, test.GetPixel(i, j));
                            //Color c = CPU.ZwrocKolor((float)i / 255, (float)j / 255);
                            //wyjscie.SetPixel(i, j, c);
                        }
                    }
                    //float b = 0f;
                    //Parallel.For(0, 400, k => b += CPU.blad.ElementAt(k));
                    //iter.Add(b);
                    CPU.blad.Clear();

                    //obrazWyjscowy.Source = wyjscie;
                }
                test.Clear(Color.FromRgb(255, 255, 255));
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 50; j++)
                    {
                        //CPU.Ucz(i / 255, j / 255, 0.06, wejscie.GetPixel(i, j));
                        wej[0] = (float)i / 50f;
                        wej[1] = (float)j / 50f;
                        wej[2] = (float)Math.Sin(wej[0] * 2 * Math.PI);
                        wej[3] = (float)Math.Sin(wej[1] * 2 * Math.PI);
                        wej[4] = (float)Math.Cos(wej[0] * 2 * Math.PI);
                        wej[5] = (float)Math.Cos(wej[1] * 2 * Math.PI);
                        wej[6] = (float)Math.Sin(2 * wej[0] * 2 * Math.PI);
                        wej[7] = (float)Math.Sin(2 * wej[1] * 2 * Math.PI);
                        wej[8] = (float)Math.Cos(2 * wej[0] * 2 * Math.PI);
                        wej[9] = (float)Math.Cos(2 * wej[1] * 2 * Math.PI);
                        wej[10] = (float)Math.Sin(3 * wej[0] * 2 * Math.PI);
                        wej[11] = (float)Math.Sin(3 * wej[1] * 2 * Math.PI);
                        wej[12] = (float)Math.Cos(3 * wej[0] * 2 * Math.PI);
                        wej[13] = (float)Math.Cos(3 * wej[1] * 2 * Math.PI);
                        //Color k = test.GetPixel(i, j);
                        //wej[14] = k.ScR;
                        //wej[15] = k.ScG;
                        //wej[16] = k.ScB;
                        Color c = CPU.ZwrocKolor(wej);
                        //MessageBox.Show(String.Empty + "i: " + i + ",j: " + j + ",R: " + c.ScR + ",G: " + c.ScG + ",B: " + c.ScB);
                        test.SetPixel(i, j, c);
                    }
                }
                System.IO.Stream fs = System.IO.File.Create((1+k)*1000+".tga");
                test.WriteTga(fs);
                etaTestowa = etaTestowa * 0.65f;
            }
            DateTime stop = DateTime.Now;
            TimeSpan czasWyk = stop.Subtract(start);
            System.Media.SoundPlayer tadam = new System.Media.SoundPlayer("tadam.wav");
            tadam.Play();
            MessageBox.Show("Gotowe \nCzas wykonynowania: " + czasWyk.Hours + " godzin, " + czasWyk.Minutes + " minut, " + czasWyk.Seconds + " sekund, " + czasWyk.Milliseconds + " milisekund");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime start = DateTime.Now;
            test.Clear(Color.FromRgb(255, 255, 255));
            //test = wejscie.Resize(20, 20, WriteableBitmapExtensions.Interpolation.Bilinear);
            test = wejscie.Resize(20,20,WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            float[] wej = new float[14];
            for (int w = 0; w < liczbaIteracji; w++)
            {
                itersLabel.Content = "Iteracja: " + (w + 1);
                for (int i = 0; i <20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        wej[0] = (float)i / 20;
                        wej[1] = (float)j / 20;
                        wej[2] = (float)Math.Sin(wej[0] * 2 * Math.PI);
                        wej[3] = (float)Math.Sin(wej[1] * 2 * Math.PI);
                        wej[4] = (float)Math.Cos(wej[0] * 2 * Math.PI);
                        wej[5] = (float)Math.Cos(wej[1] * 2 * Math.PI);
                        wej[6] = (float)Math.Sin(2 * wej[0] * 2 * Math.PI);
                        wej[7] = (float)Math.Sin(2 * wej[1] * 2 * Math.PI);
                        wej[8] = (float)Math.Cos(2 * wej[0] * 2 * Math.PI);
                        wej[9] = (float)Math.Cos(2 * wej[1] * 2 * Math.PI);
                        wej[10] = (float)Math.Sin(3 * wej[0] * 2 * Math.PI);
                        wej[11] = (float)Math.Sin(3 * wej[1] * 2 * Math.PI);
                        wej[12] = (float)Math.Cos(3 * wej[0] * 2 * Math.PI);
                        wej[13] = (float)Math.Cos(3 * wej[1] * 2 * Math.PI);
                        //Color k = test.GetPixel(i, j);
                        //wej[14] = k.ScR;
                        //wej[15] = k.ScG;
                        //wej[16] = k.ScB;
                        if (tryb == TrybStartowy.CPU)
                        {
                            CPU.UczMacierzowo(wej, eta, test.GetPixel(i, j));
                        } else if (tryb==TrybStartowy.CUDA)
                        {
                            GPU.UczMacierzowoGPU(wej, eta, test.GetPixel(i, j));
                        }
                        
                        //
                        //Color c = CPU.ZwrocKolor((float)i / 255, (float)j / 255);
                        //wyjscie.SetPixel(i, j, c);
                    }
                }
                //float b = 0f;
                //Parallel.For(0, 400, k => b += CPU.blad.ElementAt(k));
                //iter.Add(b);
                //CPU.blad.Clear();

                //obrazWyjscowy.Source = wyjscie;
            }
            DateTime stop = DateTime.Now;
            TimeSpan czasWyk = stop.Subtract(start);
            System.Media.SoundPlayer tadam = new System.Media.SoundPlayer("tadam.wav");
            tadam.Play();
            MessageBox.Show("Gotowe \nCzas wykonynowania: "+czasWyk.Hours+" godzin, "+czasWyk.Minutes+" minut, "+czasWyk.Seconds+" sekund, "+czasWyk.Milliseconds+" milisekund");
            /*
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    //CPU.Ucz(i / 255, j / 255, 0.06, wejscie.GetPixel(i, j));
                    wej[0] = (float)i / 20f;
                    wej[1] = (float)j / 20f;
                    wej[2] = (float)Math.Sin(wej[0] * 2 * Math.PI);
                    wej[3] = (float)Math.Sin(wej[1] * 2 * Math.PI);
                    wej[4] = (float)Math.Cos(wej[0] * 2 * Math.PI);
                    wej[5] = (float)Math.Cos(wej[1] * 2 * Math.PI);
                    wej[6] = (float)Math.Sin(2 * wej[0] * 2 * Math.PI);
                    wej[7] = (float)Math.Sin(2 * wej[1] * 2 * Math.PI);
                    wej[8] = (float)Math.Cos(2 * wej[0] * 2 * Math.PI);
                    wej[9] = (float)Math.Cos(2 * wej[1] * 2 * Math.PI);
                    wej[10] = (float)Math.Sin(3 * wej[0] * 2 * Math.PI);
                    wej[11] = (float)Math.Sin(3 * wej[1] * 2 * Math.PI);
                    wej[12] = (float)Math.Cos(3 * wej[0] * 2 * Math.PI);
                    wej[13] = (float)Math.Cos(3 * wej[1] * 2 * Math.PI);
                    Color k = test.GetPixel(i, j);
                    wej[14] = k.ScR;
                    wej[15] = k.ScG;
                    wej[16] = k.ScB;
                    Color c = CPU.ZwrocKolor(wej);
                    //MessageBox.Show(String.Empty + "i: " + i + ",j: " + j + ",R: " + c.ScR + ",G: " + c.ScG + ",B: " + c.ScB);
                    test.SetPixel(i, j, c);
                }
            }
            string s = String.Empty;
            for (int w = 0; w < iter.Count; w++)
                s += iter.ElementAt(w) + " ";
            iter.Clear();
            //MessageBox.Show(s);
            obrazWyjscowy.Source = test;
            */
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            test.Clear(Color.FromRgb(255, 255, 255));
            //test = wejscie.Resize(20, 20, WriteableBitmapExtensions.Interpolation.Bilinear);
            test = wejscie.Resize(20,20,WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            float[] wej = new float[17];
            
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    //CPU.Ucz(i / 255, j / 255, 0.06, wejscie.GetPixel(i, j));
                    wej[0] = (float)i/20;
                    wej[1] = (float)j/20;
                    wej[2] = (float)Math.Sin(wej[0] * 2 * Math.PI);
                    wej[3] = (float)Math.Sin(wej[1] * 2 * Math.PI);
                    wej[4] = (float)Math.Cos(wej[0] * 2 * Math.PI);
                    wej[5] = (float)Math.Cos(wej[1] * 2 * Math.PI);
                    wej[6] = (float)Math.Sin(2 * wej[0] * 2 * Math.PI);
                    wej[7] = (float)Math.Sin(2 * wej[1] * 2 * Math.PI);
                    wej[8] = (float)Math.Cos(2 * wej[0] * 2 * Math.PI);
                    wej[9] = (float)Math.Cos(2 * wej[1] * 2 * Math.PI);
                    wej[10] = (float)Math.Sin(3 * wej[0] * 2 * Math.PI);
                    wej[11] = (float)Math.Sin(3 * wej[1] * 2 * Math.PI);
                    wej[12] = (float)Math.Cos(3 * wej[0] * 2 * Math.PI);
                    wej[13] = (float)Math.Cos(3 * wej[1] * 2 * Math.PI);
                    //Color k = test.GetPixel(i, j);
                    //wej[14] = k.ScR;
                    //wej[15] = k.ScG;
                    //wej[16] = k.ScB;
                    Color c = new Color();
                    if (tryb == TrybStartowy.CPU)
                    {
                        c = CPU.ZwrocKolor(wej);
                    }
                    else if (tryb == TrybStartowy.CUDA)
                    {
                        c = GPU.KlasyfikujGPU(wej);
                    }
                   
                    //
                    //MessageBox.Show(String.Empty + "i: " + i + ",j: " + j + ",R: " + c.ScR + ",G: " + c.ScG + ",B: " + c.ScB);
                    test.SetPixel(i, j, c);
                }
            }
            obrazWyjscowy.Source = test.Resize(512, 512, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            
            //obrazWyjscowy.Source = wyjscie;
        }

        private void iteracjeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            liczbaIteracji = (int)e.NewValue;
            String tekst = "Liczba iteracji: " + liczbaIteracji;
            iteracjeLabel.Content = tekst;
        }

        private void stalaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            eta = (float)e.NewValue;
            String tekst = "Stała uczenia: " + eta;
            stalaLabel.Content = tekst;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(tryb == TrybStartowy.CUDA)
            GPU.Czysc();
        }
    }
}
