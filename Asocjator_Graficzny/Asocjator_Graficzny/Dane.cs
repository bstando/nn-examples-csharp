using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asocjator_Graficzny
{
    class Dane
    {
        private List<int[]> przyklady;
        private int wysokosc, szerokosc;
        public Dane(int w, int h)
        {
            przyklady = new List<int[]>();
            wysokosc = h;
            szerokosc = w;
         
        }

        public int[][] PobierzPrzyklady()
        {
            return przyklady.ToArray();
        }

        public int[] PobierzPrzykad(int indeks)
        {
            return przyklady.ToArray()[indeks];
        }

        public void WczytajPrzyklady()
        {
            if (System.IO.File.Exists("dane.txt"))
            {
                string[] dane = System.IO.File.ReadAllLines("dane.txt");
                foreach (string linia in dane)
                {
                    int[] wczytane = new int[wysokosc * szerokosc];
                    for (int i = 0; i < wysokosc * szerokosc; i++)
                    {
                        wczytane[i] = Int32.Parse(String.Empty + linia[2 * i]);
                    }

                    przyklady.Add(wczytane);
                }
            }
        }
        public void zapiszDoPliku(int[] tablica)
        {
            int[] tmp = new int[wysokosc * szerokosc];
            String linia = String.Empty;
            for (int i = 0; i < wysokosc * szerokosc; i++)
            {
                linia += tablica[i] + " ";
                tmp[i] = tablica[i];
            }
            List<String> dodane = new List<string>();
            dodane.Add(linia);
            przyklady.Add(tablica);
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
        public int PobierzLiczbePrzykladow()
        {
            return przyklady.Count();
        }
    }
}
