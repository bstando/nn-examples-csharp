using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoasocjator_Hopfield
{
    class Hopfield
    {
        int iloscWejsc;
        Macierz macierz;
        public Hopfield(int wejscia)
        {
            iloscWejsc = wejscia;
            macierz = new Macierz(iloscWejsc);
            macierz.Zeruj();
        }

        public double[] Klasyfikuj(double[] wejscie)
        {
            double[] wynik = macierz.MnozRazyWektor(wejscie);
            for(int i=0;i<macierz.Wymiar;i++)
            {
                if (wynik[i] >= 0) wynik[i] = 1;
                else wynik[i] = -1;
            }
            return wynik;
        }
        public void Ucz(double[] przyklad)
        {
            Macierz t = macierz.MnozWektorRazyTranspozycje(przyklad);
            t.CzyscPrzekatna();
            Macierz w = new Macierz(t.Wymiar);
            w = macierz.Dodaj(t);
            macierz = w;
        }
    }
}
