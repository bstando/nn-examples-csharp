using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEP_Obrazy_RGB
{
    class Neuron
    {
        public float Bias { get; set; }
        public float[] Wagi { get; set; }
        public int RozmiarWejsca { get; set; }

        public Neuron(int iloscWejsc)
        {
            RozmiarWejsca = iloscWejsc;
        }

        public void LosujWagi()
        {
            Wagi = new float[RozmiarWejsca];
            Random randomizer = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < RozmiarWejsca; i++)
            {

                Wagi[i] = (float)randomizer.NextDouble() * 10f * Math.Sign(randomizer.NextDouble() - 1d / 2d);
                //Wagi[i] = (float)randomizer.NextDouble();
            }
            Bias = (float)randomizer.NextDouble() * 10f * Math.Sign(randomizer.NextDouble() - 1d / 2d);
            //Bias = (float)randomizer.NextDouble();
        }

        public float Klasyfikuj(float[] dane)
        {
            float wynik = 0f;
            for (int i = 0; i < RozmiarWejsca; i++)
            {
                wynik += dane[i] * Wagi[i];
            }
            wynik += Bias;


            return (float)(1f / (1f + Math.Exp(-wynik)));
        }

        public float ZwrocPochodna(float[] dane)
        {
            float w = Klasyfikuj(dane);
            return w * (1 - w);
        }
    }
}
