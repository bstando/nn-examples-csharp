using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klasyfikacja_Cyfr_Algorytm_Wiezowy
{
    class Neuron
    {
        public double[] Wagi {get; set;}
        public double Bias {get; set;}
        public int RozmiarWejscia { get; set; }
        public Neuron(int iloscWejsc)
        {
            RozmiarWejscia = iloscWejsc;
            Wagi = new double[RozmiarWejscia];
        }
        public void LosujWagi()
        {
            Random randomizer = new Random(Guid.NewGuid().GetHashCode());
            for(int i=0;i<RozmiarWejscia;i++)
            {
                Wagi[i] = 0;
            }
            Bias =0;
        }
        public int Klasyfikuj(int[] dane)
        {
            double suma = 0;
            for(int i=0;i<RozmiarWejscia;i++)
            {
                suma += dane[i] * Wagi[i];
            }
            suma += Bias;
            if (suma < 0) return -1;
            else return 1;
        }
    }
}
