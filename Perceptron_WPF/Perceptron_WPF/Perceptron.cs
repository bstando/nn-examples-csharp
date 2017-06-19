using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perceptron_WPF
{
    class Perceptron
    {
        double[] wagi;
        //double wynik;

        double bias;

        public Perceptron()
        {
            wagi = new double[25];
            //wynik = 0.0;
        }
        
        public void LosujWagi()
        {
            Random rand = new Random();
            for(int i = 0; i < 25; i++)
            {
                wagi[i] = rand.NextDouble();
            }
            bias = rand.NextDouble();
        }

        public short sprawdz(int[] wartoscPol)
        {
            double wynik = 0.0;
            for(int i = 0;i<25;i++)
            {
                wynik += wagi[i] * wartoscPol[i]; 
            }
            return (short) ((wynik - bias>=0)?1:-1);
        }

        public void poprawWagi(int[] wartosciPol, double mi, int blad)
        {
            for (int i = 0; i < 25; i++)
            {
                double poprawka = blad * wartosciPol[i] * mi;
                wagi[i] += poprawka;
            }
            bias -= blad;
        }

    }
}
