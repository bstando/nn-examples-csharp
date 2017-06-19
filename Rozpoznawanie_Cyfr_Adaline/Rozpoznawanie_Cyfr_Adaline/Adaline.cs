using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;

namespace Rozpoznawanie_Cyfr_Adaline
{

    class Adaline
    {
        Neuron neuron;
       
        List<double> bledy;
        public volatile bool czyDziala = true;
        int numerMaszyny;

        
        public Adaline(int numer)
        {
            numerMaszyny = numer;   
            neuron = new Neuron(35); 
            bledy = new List<double>();
        }

        double LiczBlad(int[] tablica, double[] transformata)
        {
            int odpowiedz;
            if(numerMaszyny==tablica[35])
            {
                odpowiedz = 1;
            }
            else
            {
                odpowiedz = -1;
            }
            double wyn = odpowiedz-neuron.Klasyfikuj(tablica, transformata);
            return wyn;
        }

        void PoprawWagi(int[] dane,double[] transformata,double blad,double stala)
        {
            
            double[] poprawianeWagi = neuron.WagiWejscia;
            double[] poprawianeWagiTransformaty = neuron.WagiTransformaty;
            double poprawianyBias = neuron.Bias;
            double[] normalizwaneWagi = new double[35];
            double[] normalizowanaTransformata = new double[35];
            double wspolczynnikNormalizacjiWag = 0.0;
            double wspolczynnikNormalizacjiTransformaty = 0.0;
            for (int i = 0; i < 35; i++)
            {
                wspolczynnikNormalizacjiWag += dane[i]*dane[i];
                wspolczynnikNormalizacjiTransformaty += transformata[i]*transformata[i];
            }
            wspolczynnikNormalizacjiWag = Math.Sqrt(Math.Abs(wspolczynnikNormalizacjiWag));
            wspolczynnikNormalizacjiTransformaty = Math.Sqrt(Math.Abs(wspolczynnikNormalizacjiTransformaty));
            for (int i = 0; i < 35; i++)
            {
                normalizwaneWagi[i] = (double)dane[i] / wspolczynnikNormalizacjiWag;
                normalizowanaTransformata[i] = transformata[i] / wspolczynnikNormalizacjiTransformaty;
            }
            Parallel.For(0,35,i=>
            {
                poprawianeWagi[i] += stala * blad * normalizwaneWagi[i];
                poprawianeWagiTransformaty[i] += stala * blad * normalizowanaTransformata[i];
            });
            poprawianyBias += stala * blad;
            neuron.Bias = poprawianyBias;
            neuron.WagiTransformaty = poprawianeWagiTransformaty;
            neuron.WagiWejscia = poprawianeWagi;
        }

        public void UczJednostke(int[][] dane, double[][] wartosciTransformaty, int iloscDanych, double stalaUczaca,double wielkoscBledu)
        {
            int liczbaIteracji = 0; //Ograniczenie do testow
            bool pass = false;

            while (!pass)
            {
                if (czyDziala)
                {
                    pass = true;
                    if (liczbaIteracji < 1000000)
                    {
                      
                        for (int i = 0; i < iloscDanych; i++)
                        {
                            liczbaIteracji++;
                            double wyliczonyBlad = LiczBlad(dane[i], wartosciTransformaty[i]);

                            bledy.Add(wyliczonyBlad);

                            if (!((wyliczonyBlad <= wielkoscBledu) && (wyliczonyBlad >= -wielkoscBledu)))
                            {
                                pass = false;                          
                            }
                            PoprawWagi(dane[i], wartosciTransformaty[i], wyliczonyBlad, stalaUczaca);
                        }
                    }
                }
                else
                {
                    pass = true;
                }
            }
        }
        public double Klasyfikuj(int[] dane,double[] transformata)
        {
            return neuron.Klasyfikuj(dane, transformata);
        }

        public List<double> ZwrocBledy()
        {
            return bledy;
        }
    }
}
