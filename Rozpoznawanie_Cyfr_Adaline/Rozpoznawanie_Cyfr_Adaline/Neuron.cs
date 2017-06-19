using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozpoznawanie_Cyfr_Adaline
{
    class Neuron
    {
        int iloscWejsc;
        double[] wagiWejscia;
        double[] wagiTransformaty;
        double bias;
        public Neuron(int rozmiarWejscia)
        {
            iloscWejsc = rozmiarWejscia;
            wagiWejscia = new double[iloscWejsc];
            wagiTransformaty = new double[iloscWejsc];
            LosujWagi();
        }
        void LosujWagi()
        {
            Random randomizer = new Random(Guid.NewGuid().GetHashCode());
            bias = randomizer.NextDouble();
            for(int i = 0; i < iloscWejsc; i++)
            {
                wagiWejscia[i] = randomizer.NextDouble();
                wagiTransformaty[i] = randomizer.NextDouble();
            }
        }

        public double Klasyfikuj(int[] tablica,double[] wynikTransformaty)
        {
            double suma = 0.0;
            double[] normalizwaneWagi = new double[iloscWejsc];
            double[] normalizowanaTransformata = new double[iloscWejsc];
            double wspolczynnikNormalizacjiWag = 0.0;
            double wspolczynnikNormalizacjiTransformaty = 0.0;
            for (int i = 0; i < iloscWejsc; i++)
            {
                wspolczynnikNormalizacjiWag += tablica[i]*tablica[i];
                wspolczynnikNormalizacjiTransformaty += wynikTransformaty[i]*wynikTransformaty[i];
            }
            wspolczynnikNormalizacjiWag = Math.Sqrt(Math.Abs(wspolczynnikNormalizacjiWag));
            wspolczynnikNormalizacjiTransformaty = Math.Sqrt(Math.Abs(wspolczynnikNormalizacjiTransformaty));
            for (int i = 0; i < iloscWejsc; i++)
            {
               normalizwaneWagi[i] = (double)tablica[i] / wspolczynnikNormalizacjiWag;
               normalizowanaTransformata[i] = wynikTransformaty[i] / wspolczynnikNormalizacjiTransformaty;
            }

            for (int i = 0; i < iloscWejsc; i++)
            {
                suma += wagiTransformaty[i] * normalizowanaTransformata[i];
                //suma += wagiWejscia[i] * normalizwaneWagi[i];
            }
            suma += bias;
            return suma;
        }

        public double[] WagiWejscia
        {
            get
            {
                return wagiWejscia;
            }
            set
            {
                wagiWejscia = value;
            }
        }

        public double[] WagiTransformaty
        {
            get
            {
                return wagiTransformaty;
            }
            set
            {
                wagiTransformaty = value;
            }
        }
        public double Bias
        {
            get
            {
                return bias;
            }
            set
            {
                bias = value;
            }
        }
    }
}
