using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Cyfr_Algorytm_Wiezowy
{
    class Wieza
    {
        List<Neuron> neurony;
        int RozmiarWejsca;
        int NumerWiezy;
        int MaksymalnaIloscIteracji;
        int liczbaPoprawnieRozpoznanychPrzykladow = 0;
        int rozpoznano = 0;
        int najlepszyRozpoznal=0;
        double[] najlepszeWagi;
        double najlepszyBias;

        public Wieza(int iloscWejsc, int numer, int liczbaIteracji)
        {
            neurony = new List<Neuron>();
            RozmiarWejsca = iloscWejsc;
            Neuron neuron = new Neuron(RozmiarWejsca);
            neuron.LosujWagi();
            neurony.Add(neuron);
            NumerWiezy = numer;
            MaksymalnaIloscIteracji = liczbaIteracji;
        }
        public void UczWieze(int[][] daneWejsciowe)
        {
            while (liczbaPoprawnieRozpoznanychPrzykladow < daneWejsciowe.Count())
            {                
                for (int k = 0; k < MaksymalnaIloscIteracji; k++)
                {
                    //if (liczbaPoprawnieRozpoznanychPrzykladow == daneWejsciowe.Count())
                    //{
                    //    break;
                    //}
                    for (int j = 0; j < daneWejsciowe.Count(); j++)
                    {
                        int w = daneWejsciowe[j][35] == NumerWiezy ? 1 : -1;
                        int[] wejscie = ZwrocWejscieDlaOstatniegoNeuronu(daneWejsciowe[j]);
                        UczOstatniPerceptron(wejscie, w);
                    }
                    if (rozpoznano > najlepszyRozpoznal)
                    {
                        neurony.ElementAt(neurony.Count - 1).Wagi = najlepszeWagi;
                        neurony.ElementAt(neurony.Count - 1).Bias = najlepszyBias;
                        rozpoznano = najlepszyRozpoznal;
                    }
                    liczbaPoprawnieRozpoznanychPrzykladow = najlepszyRozpoznal;
                    rozpoznano = 0;
                    najlepszyRozpoznal = 0;
                }                
                if (liczbaPoprawnieRozpoznanychPrzykladow < daneWejsciowe.Count())
                {
                    Neuron n = new Neuron(RozmiarWejsca + 1);
                    n.LosujWagi();                    
                    neurony.Add(n);
                    System.Windows.MessageBox.Show("Dodano neuron o numerze: " + (neurony.Count - 1) + " w wiezy: " + NumerWiezy);
                }
                
                najlepszyRozpoznal = 0;
                rozpoznano = 0;
            }
            neurony.ElementAt(neurony.Count - 1).Wagi = najlepszeWagi;
        }
        void UczOstatniPerceptron(int[] wejscie,int wartoscOczekiwana)
        {
            int ostatni = neurony.Count-1;
            int klasyfikacja = neurony.ElementAt(ostatni).Klasyfikuj(wejscie);
            if(klasyfikacja==wartoscOczekiwana)
            {
                rozpoznano++;
                if (rozpoznano > najlepszyRozpoznal)
                {
                    najlepszeWagi = neurony.ElementAt(ostatni).Wagi;
                    najlepszyBias = neurony.ElementAt(ostatni).Bias;
                    najlepszyRozpoznal = rozpoznano;
                }
            }
            else
            {
                for(int i=0;i<neurony.ElementAt(ostatni).Wagi.Count();i++)
                {
                    neurony.ElementAt(ostatni).Wagi[i] += wartoscOczekiwana * wejscie[i]; 
                }
                neurony.ElementAt(ostatni).Bias -= wartoscOczekiwana;
                rozpoznano = 0;
            }
        }
        int[] ZwrocWejscieDlaOstatniegoNeuronu(int[] wejscie)
        {
            int[] wynik = new int[RozmiarWejsca + 1];
            for (int i = 0; i < RozmiarWejsca;i++)
            {
                wynik[i] = wejscie[i];
            }
            wynik[RozmiarWejsca] = neurony.ElementAt(0).Klasyfikuj(wejscie);
            for (int i = 1; i < neurony.Count;i++)
            {
                int w = neurony.ElementAt(i).Klasyfikuj(wynik);
                wynik[RozmiarWejsca] = w;
            }
            return wynik;

        }
        public int Klasyfikuj(int[] dane)
        {
            int[] wyn = ZwrocWejscieDlaOstatniegoNeuronu(dane);

            int wynik = neurony.ElementAt(neurony.Count - 1).Klasyfikuj(wyn);
            //int[] tmp = new int[RozmiarWejsca + 1];
            //for (int i = 0; i < RozmiarWejsca; i++)
            //{
            //    tmp[i] = dane[i];
            //}
            //tmp[RozmiarWejsca] = neurony.ElementAt(0).Klasyfikuj(dane);
            //wynik = tmp[RozmiarWejsca];

            //for (int j = 1; j < neurony.Count; j++)
            //{
            //    wynik = neurony.ElementAt(j).Klasyfikuj(dane);
            //    tmp[RozmiarWejsca] = wynik;
            //}
            return wynik;

        }
    }

}
