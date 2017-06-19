using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asocjator_Graficzny
{
    class Perceptron
    {
        private double[] wagi;
        private double bias;
        int iloscElementow;
        public Perceptron(int rozmiarDanychWejsciowych)
        {
            wagi = new double[rozmiarDanychWejsciowych];
            iloscElementow = rozmiarDanychWejsciowych;
        }
        public void LosujWagi()
        {
            Random randomizer = new Random();
            for(int i = 0; i < iloscElementow; i++)
            {
                wagi[i] = randomizer.NextDouble();
            }
            bias = randomizer.NextDouble();
        }
        public double PrzeliczObraz(int[] zapalonePiksele)
        {
            double wynik = 0.0;
            for(int i=0;i<iloscElementow;i++)
            {
                wynik += zapalonePiksele[i] * wagi[i];
            }
            wynik += bias;
            return wynik;
        }

        public double[] Wagi
        {
            get
            {
                return wagi;
            }
            set
            {
                wagi = value;
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
