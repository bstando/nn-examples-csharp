using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozpoznawanie_Cyfr_Adaline
{
    /// <summary>
    /// Klasa pomocnicza służąca do operowania danymi
    /// </summary>
    class Dane
    {
        public int Wczytano { get; set; }
        string nazwaPliku = "dane.txt";
        public Dane()
        {
        }
        public int[][] PobierzDane()
        {
            if (System.IO.File.Exists(nazwaPliku))
            {
                List<int[]> lista = new List<int[]>();

                String[] wczytaneLinie = System.IO.File.ReadAllLines(nazwaPliku);

                foreach(String linia in wczytaneLinie)
                {
                    int[] dodanaLiczba = new int[36];
                    for(int i = 0;i<36;i++)
                    {
                        dodanaLiczba[i] = Int32.Parse(String.Empty + linia[2 * i]);
                    }
                    lista.Add(dodanaLiczba);
                }
                Wczytano = lista.Count();
                return lista.ToArray();
            }
            else throw new System.IO.FileNotFoundException();
        }

        public void ZapiszBladDoPliku(List<double> dane)
        {
            //if(!System.IO.File.Exists("blad.txt"))
            //{
            //    System.IO.File.Delete("blad.txt");
            //    System.IO.File.Create("blad.txt");
            //}
            //else
            //{
            //    System.IO.File.Create("blad.txt");
            //}
            List<String> doZapisu = new List<string>();
            foreach(double d in dane)
            {
                doZapisu.Add(d.ToString());
            }
            System.IO.File.WriteAllLines("blad.txt",doZapisu);
        }

        public void ZapiszDaneDoPliku(List<double> dane)
        {
            //if(!System.IO.File.Exists("blad.txt"))
            //{
            //    System.IO.File.Delete("blad.txt");
            //    System.IO.File.Create("blad.txt");
            //}
            //else
            //{
            //    System.IO.File.Create("blad.txt");
            //}
            List<String> doZapisu = new List<string>();
            foreach (double d in dane)
            {
                doZapisu.Add(d.ToString());
            }
            System.IO.File.WriteAllLines("dane_ada.txt", doZapisu);
        }
    }

    
}
