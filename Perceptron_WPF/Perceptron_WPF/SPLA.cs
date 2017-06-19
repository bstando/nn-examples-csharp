using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perceptron_WPF
{
    class SPLA
    {
        int[] wektorWynikow;
        List<int[]> zapisaneDane;
        public SPLA(int[] wartosciZMacierzy)
        {
            wektorWynikow = new int[25];
            zapisaneDane = new List<int[]>();
            for(int i = 0 ;i <25; i++)
            {
                wektorWynikow[i] = wartosciZMacierzy[i];
            }
            wczytajDaneZPliku();
        }
        public void wczytajDaneZPliku()
        {
            if(System.IO.File.Exists("dane.txt"))
            {
                string[] dane = System.IO.File.ReadAllLines("dane.txt");
                foreach(string linia in dane)
                {
                    int[] wczytane = new int[26];
                    for(int i=0;i<26;i++)
                    {
                        wczytane[i] = Int32.Parse(String.Empty + linia[2*i]);
                    }
                    
                    zapisaneDane.Add(wczytane);
                }
                int ilosc = zapisaneDane.Count();
                Random r = new Random(ilosc);
                int losowane = r.Next(ilosc);
                zapisaneDane.Reverse(0, losowane);
                zapisaneDane.Reverse(losowane, ilosc - losowane);
            }
        }
        public void zapiszDoPliku(int[] tablica, int wartosc)
        {
            int[] tmp = new int[26];
            String linia = String.Empty;
            for (int i = 0; i < 25; i++)
            {
                linia += tablica[i] + " ";
                tmp[i] = tablica[i];
            }

            tmp[25] = wartosc;
            linia += wartosc;
            List<String> dodane = new List<string>();
            dodane.Add(linia);
            zapisaneDane.Add(tmp);
            
            if (System.IO.File.Exists("dane.txt"))
            {                
                System.IO.File.AppendAllLines("dane.txt", dodane);                
            }
            else
            {
                System.IO.File.Create("dane.txt");
                System.IO.File.AppendAllLines("dane.txt", dodane);                
            }
        }
        public void nauczPerceptrony(Perceptron[] p, double mi)
        {
            //losujWagiPerceptornow(p);
            Parallel.For(0, 10, i =>
            //for (int i = 0; i < 10; i++)
            {
                nauczPerceptron(p[i], i, mi);
            });

        }

        void nauczPerceptron(Perceptron p, int numerPerceptronu, double mi)
        {
            bool pass = false;
            if (zapisaneDane.Count == 0) pass = true;

            p.LosujWagi();

            while (!pass)
            {
                //losujWagiPerceptornow(p);
                pass = true;
                foreach (int[] linia in zapisaneDane)
                {                                
                    
                    int[] wczytana = new int[25];
                    for (int k = 0; k < 25; k++) // Tu zapisuje to tablicy liczbę z wczytanej lini z pliku
                    {
                        wczytana[k] = linia[k];
                    }

                    int val = (linia[25] == numerPerceptronu) ? 1 : -1;
                    int blad = val - p.sprawdz(wczytana);

                    if (blad != 0)
                    {
                        pass = false;
                        p.poprawWagi(wczytana, mi, blad);
                    }
                }
            }
        }

        public void losujWagiPerceptornow(Perceptron[] p)
        {
            Parallel.For(0, 10, i =>
            //for(int i=0;i<10;i++)
            {
                p[i].LosujWagi();
            });
        }
    }
}
