using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEP_Obrazy_RGB
{
    class BEP
    {
        protected int iloscWejsc;
        protected int iloscWyjsc;
        protected int iloscWarstw;
        protected int[] iloscNeuronowWWarstwie;
        protected float[][] macierzDelt;
        protected float[][] macierzWejsc;
        protected float[][] macierzWyjsc;
        protected float[][] macierzSum;
        protected int[] warstwy;
        //protected double blad = 0.0;
        protected Neuron[][] neurony;
        public BEP(int rozmiarWejsca, int rozmiarWyjscia,int glebokosc)
        {
            iloscWejsc = rozmiarWejsca;
            iloscWyjsc = rozmiarWyjscia;
            iloscWarstw = glebokosc;
            LosujIloscNeuronowWWarstwie();
            TworzNeurony();
        }
        public BEP(int rozmiarWejsca, int rozmiarWyjscia, int glebokosc,int[] rozmiarNeuronowWWarstwie)
        {
            iloscWejsc = rozmiarWejsca;
            iloscWyjsc = rozmiarWyjscia;
            iloscWarstw = glebokosc;
            iloscNeuronowWWarstwie = rozmiarNeuronowWWarstwie;
            TworzNeurony();
        }

        void LosujIloscNeuronowWWarstwie()
        {
            iloscNeuronowWWarstwie = new int[iloscWarstw];
            Random rand = new Random(100);
            for(int i = 0; i < iloscWarstw;i++)
            {
                iloscNeuronowWWarstwie[i] = rand.Next(3, 30);
            }
        }
        void TworzNeurony()
        {
            warstwy = new int[iloscWarstw + 2];
            neurony = new Neuron[iloscWarstw+2][];
            neurony[0] = new Neuron[iloscWejsc];
            warstwy[0] = neurony[0].Count();
            for(int i=1;i<iloscWarstw+1;i++)
            {
                neurony[i] = new Neuron[iloscNeuronowWWarstwie[i - 1]];
                warstwy[i] = neurony[i].Count();
            }
            neurony[iloscWarstw + 1] = new Neuron[iloscWyjsc];
            warstwy[iloscWarstw + 1] = iloscWyjsc;
            for(int k=0;k<iloscWejsc;k++)
            {
                neurony[0][k] = new Neuron(iloscWejsc);
            }
            for(int i = 1;i<iloscWarstw+1;i++)
            {
                for(int j = 0; j< iloscNeuronowWWarstwie[i-1];j++)
                {
                    neurony[i][j] = new Neuron(neurony[i-1].Count());
                }
            }
            for(int k=0;k<iloscWyjsc;k++)
            {
                neurony[iloscWarstw+1][k] = new Neuron(iloscNeuronowWWarstwie[iloscWarstw-1]);
            }
        }
        public void UtworzMacierze()
        {
            macierzDelt = new float[neurony.Count()][];
            macierzWyjsc = new float[neurony.Count()][];
            macierzSum = new float[neurony.Count()][];
            macierzWejsc = new float[neurony.Count() + 1][];
            macierzWejsc[0] = new float[iloscWejsc];
            for (int i = 0; i < neurony.Count(); i++)
            {
                macierzDelt[i] = new float[neurony[i].Count()];
                macierzWyjsc[i] = new float[neurony[i].Count()];
                macierzSum[i] = new float[neurony[i].Count()];
            }
            for (int j = 1; j < neurony.Count() + 1; j++)
            {
                macierzWejsc[j] = new float[neurony[j - 1].Count()];
            }
        }
        public void LosujWagi()
        {
            Parallel.For(0, iloscWejsc, k =>
            {
                neurony[0][k].LosujWagi();
            });
            Parallel.For(1, iloscWarstw + 1, i =>
            {
                Parallel.For(0, iloscNeuronowWWarstwie[i - 1], j =>
                {
                    neurony[i][j].LosujWagi();
                });
            });
            Parallel.For(0, iloscWyjsc, k =>
            {
                neurony[iloscWarstw + 1][k].LosujWagi();
            });
        }
        protected void CzyscMacierze()
        {
            Parallel.For(0, iloscWarstw+2, i =>
            {
                Parallel.For(0, warstwy[i], j =>
                {
                    macierzDelt[i][j] = 0f;
                    macierzWyjsc[i][j] = 0f;
                    macierzSum[i][j] = 0f;
                });
            });
            Parallel.For(0, macierzWejsc.Count(), i =>
            {
                Parallel.For(0, macierzWejsc[i].Count(), j =>
                {
                    macierzWejsc[i][j] = 0f;
                });
            });

        }
    }
}
