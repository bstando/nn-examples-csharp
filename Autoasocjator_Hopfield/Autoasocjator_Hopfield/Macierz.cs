using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoasocjator_Hopfield
{
    class Macierz
    {
        double[][] Pola { get; set; }
        public int Wymiar {get; set; }
        public Macierz(int rozmiar)
        {
            Wymiar = rozmiar;
            Pola = new double[Wymiar][];
            for(int i=0;i<Wymiar;i++)
            {
                Pola[i] = new double[Wymiar];
            }
        }

        public void Zeruj()
        {
            for(int i=0;i<Wymiar;i++)
                for(int j=0;j<Wymiar;j++)
                {
                    Pola[i][j] = 0;
                }
        }

        public double[] MnozRazyWektor(double[] wektor)
        {
            double[] wynik = new double[Wymiar];
            for(int i=0;i<Wymiar;i++)
            {
                double sum = 0;
                for(int j=0;j<Wymiar;j++)
                {
                    sum += wektor[j] * Pola[i][j];
                }
                wynik[i] = sum;
            }
            return wynik;
        }
        public void CzyscPrzekatna()
        {
            for(int i=0;i<Wymiar;i++)
            {
                Pola[i][i] = 0;
            }
        }
        public Macierz Dodaj(Macierz a)
        {
            Macierz wynik = new Macierz(Wymiar);
            wynik.Zeruj();
            for (int i = 0; i < Wymiar;i++)
            {
                for(int j=0;j<Wymiar;j++)
                {
                    wynik.Pola[i][j] = this.Pola[i][j] + a.Pola[i][j];
                }
            }
         return wynik;
        }
        public Macierz Odejmij(Macierz a)
        {
            Macierz wynik = new Macierz(Wymiar);
            wynik.Zeruj();
            for (int i = 0; i < Wymiar; i++)
            {
                for (int j = 0; j < Wymiar; j++)
                {
                    wynik.Pola[i][j] = this.Pola[i][j] - a.Pola[i][j];
                }
            }
            return wynik;
        }
        public Macierz Mnoz(Macierz a)
        {
            Macierz wynik = new Macierz(Wymiar);
            wynik.Zeruj();
            for (int i = 0; i < Wymiar; i++)
            {
                for (int j = 0; j < Wymiar; j++)
                {
                    for (int k = 0; k < Wymiar;k++ )
                        wynik.Pola[i][j] += this.Pola[i][k] * a.Pola[k][j];
                }
            }
            return wynik;
        }
        public override string ToString()
        {
            String a = String.Empty;
            for (int i = 0; i < Wymiar; i++)
            {
                a += "| ";
                for (int j = 0; j < Wymiar; j++)
                {
                    a += this.Pola[i][j] + " ";
                }
                a += "|\n";
            }
            return a;
        }
        public void NadajWartosci(double[][] wartosci)
        {
            this.Zeruj();
            this.Pola = wartosci;
        }

        public Macierz MnozWektorRazyTranspozycje(double[] wartosci)
        {
            int wymiarWyjsciowy = wartosci.Count();
            Macierz wyjscie = new Macierz(wymiarWyjsciowy);
            for(int i = 0;i<wymiarWyjsciowy;i++)
                for(int j = 0;j<wymiarWyjsciowy;j++)
                {
                    wyjscie.Pola[i][j] = wartosci[i] * wartosci[j];
                }
            return wyjscie;
        }
    }
}
