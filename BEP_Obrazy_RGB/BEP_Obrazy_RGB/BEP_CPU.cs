using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BEP_Obrazy_RGB
{
    class BEP_CPU : BEP
    {
        

        public List<float> blad;
        public BEP_CPU(int rozmiarWejsca, int rozmiarWyjscia, int glebokosc)
            : base(rozmiarWejsca, rozmiarWyjscia, glebokosc)
        {
            blad = new List<float>();
            //UtworzMacierze();
        }
        public BEP_CPU(int rozmiarWejsca, int rozmiarWyjscia, int glebokosc, int[] rozmiarNeuronowWWarstwie)
            : base(rozmiarWejsca, rozmiarWyjscia, glebokosc, rozmiarNeuronowWWarstwie)
        {
            blad = new List<float>();
            //UtworzMacierze();
        }

        

        public Color ZwrocKolor(float[] wejscie)
        {
            Color kolor = new Color();
            //Color c = new Color();
            float[] wyliczone = new float[neurony[0].Count()];
            float[] poprzednie;

            Parallel.For(0, warstwy[0], k => wyliczone[k] = neurony[0][k].Klasyfikuj(wejscie));
            poprzednie = wyliczone;

            for (int i = 1; i < iloscWarstw + 2; i++)
            {
                float[] gotowe = new float[warstwy[i]];
                Parallel.For(0, warstwy[i], j =>
                    {
                        gotowe[j] = neurony[i][j].Klasyfikuj(poprzednie);
                    });
                wyliczone = gotowe;
                poprzednie = wyliczone;
            }
            //kolor.R = (byte)(neurony[iloscWarstw + 1][0].Klasyfikuj(poprzednie) * 255);
            kolor.ScR = poprzednie[0];
            kolor.ScG = poprzednie[1];
            kolor.ScB = poprzednie[2];
            kolor.ScA = 1.0f;
            //kolor.G = (byte)(neurony[iloscWarstw + 1][1].Klasyfikuj(poprzednie) * 255);
            //kolor.B = (byte)(neurony[iloscWarstw + 1][2].Klasyfikuj(poprzednie) * 255);
            return kolor;
        }
        
        public void UczMacierzowo(float[] daneWejsciowe, float eta, Color kolor)
        {
            CzyscMacierze();
            Parallel.For(0, warstwy[0], i =>
                {
                    macierzWejsc[0][i] = daneWejsciowe[i];
                });
            for (int i = 0; i < iloscWarstw + 2; i++)
            {
                Parallel.For(0, warstwy[i], j =>
                    {
                        macierzWyjsc[i][j] = neurony[i][j].Klasyfikuj(macierzWejsc[i]);
                        macierzWejsc[i + 1][j] = macierzWyjsc[i][j];
                    });

            }
            macierzDelt[iloscWarstw + 1][0] = -1f * (macierzWyjsc[iloscWarstw + 1][0] - kolor.ScR) * macierzWyjsc[iloscWarstw + 1][0] * (1f - macierzWyjsc[iloscWarstw + 1][0]);
            macierzDelt[iloscWarstw + 1][1] = -1f * (macierzWyjsc[iloscWarstw + 1][1] - kolor.ScG) * macierzWyjsc[iloscWarstw + 1][1] * (1f - macierzWyjsc[iloscWarstw + 1][1]);
            macierzDelt[iloscWarstw + 1][2] = -1f * (macierzWyjsc[iloscWarstw + 1][2] - kolor.ScB) * macierzWyjsc[iloscWarstw + 1][2] * (1f - macierzWyjsc[iloscWarstw + 1][2]);
            for (int k = iloscWarstw; k >= 0; k--)
            {
                Parallel.For(0, warstwy[k], j =>
                    {
                        Parallel.For(0, warstwy[k+1], i => 
                            {
                                macierzSum[k][j] += macierzDelt[k + 1][i] * neurony[k + 1][i].Wagi[j];
                                //macierzSum[k][j] += macierzDelt[k+1][i] * neurony[k+1][i].Bias;
                            });
                    }
                    );
                Parallel.For(0, warstwy[k], i => macierzDelt[k][i] = macierzSum[k][i] * macierzWyjsc[k][i] * (1 - macierzWyjsc[k][i]));
            }
            //for (int k = 0; k < iloscWarstw + 2; k++)
            //{
                Parallel.For(0, iloscWarstw + 2, k =>
                    {
                        Parallel.For(0, warstwy[k], i =>
                            {
                                Parallel.For(0, neurony[k][i].Wagi.Count(), j => neurony[k][i].Wagi[j] += eta * macierzDelt[k][i] * macierzWejsc[k][j]);
                                neurony[k][i].Bias -= eta * macierzDelt[k][i];
                            });
                    });
            //}
        }
        public void Ucz(float[] daneWejsciowe, float eta, Color kolor)
        {
            List<float[]> wyjsciaZWarstw = new List<float[]>();
            List<float[]> delty = new List<float[]>();
            //1. Obliczanie wyjsc z warstw
            float[] wejscie = daneWejsciowe;
            for (int i = 0; i < iloscWarstw + 2; i++)
            {
                float[] wynik = new float[neurony[i].Count()];
                Parallel.For(0, neurony[i].Count(), j => wynik[j] = neurony[i][j].Klasyfikuj(wejscie));
                wyjsciaZWarstw.Add(wynik);
                wejscie = wynik;
            }
            //2. Obliczenie delty dla warstwy wyjsciowej
            float[] poch = new float[3];
            Parallel.For(0, 3, i => poch[i] = neurony[iloscWarstw + 1][i].ZwrocPochodna(wyjsciaZWarstw.ElementAt(iloscWarstw)));
            Color wyj = ZwrocKolor(daneWejsciowe);
            float[] d = new float[neurony[iloscWarstw + 1].Count()];
            d[0] = ((float)(wejscie[0] - wyj.ScR)) * poch[0];
            d[1] = ((float)(wejscie[1] - wyj.ScG)) * poch[1];
            d[2] = ((float)(wejscie[2] - wyj.ScB)) * poch[2];
            blad.Add((kolor.ScR - wyj.ScR) + (kolor.ScG - wyj.ScG) + (kolor.ScB - wyj.ScB));
            delty.Add(d);

            //for (int k = 0; k < 3;k++ )
            //{
            //    Parallel.For(0, neurony[iloscWarstw + 1][k].Wagi.Count(), l => neurony[iloscWarstw + 1][k].Wagi[l] += d*eta* wyjsciaZWarstw.ElementAt(iloscWarstw)[l]);
            //}

            //3a. Obliczanie delty dla warstw ukrytych
            for (int i = iloscWarstw; i > 0; i--)
            {
                float[] pochodne = new float[neurony[i].Count()];
                Parallel.For(0, neurony[i].Count(), j => pochodne[j] = neurony[i][j].ZwrocPochodna(wyjsciaZWarstw.ElementAt(i - 1)));

                float[] sumy = new float[neurony[i].Count()];

                float[] deltyZPoprzedniejWarstwy = delty.ElementAt(iloscWarstw - i);

                float[] deltyZWarstwy = new float[neurony[i].Count()];

                Parallel.For(0, neurony[i].Count(), j =>
                    {
                        for (int k = 0; k < neurony[i + 1].Count(); k++)
                        {
                            sumy[j] += deltyZPoprzedniejWarstwy[k] * neurony[i + 1][k].Wagi[j];
                        }
                    });
                Parallel.For(0, neurony[i].Count(), k => deltyZWarstwy[k] = sumy[k] * pochodne[k]);
                delty.Add(deltyZWarstwy);
            }
            //3b. Obliczanie delty dla warstwy wejsciowej
            float[] pochodneZWejscia = new float[neurony[0].Count()];
            Parallel.For(0, neurony[0].Count(), j => pochodneZWejscia[j] = neurony[0][j].ZwrocPochodna(daneWejsciowe));
            float[] sumyZWejscia = new float[neurony[0].Count()];
            float[] deltyZWejscia = new float[neurony[0].Count()];
            float[] deltyZDrugiejWarstwy = delty.ElementAt(iloscWarstw);
            Parallel.For(0, neurony[0].Count(), j =>
            {
                for (int k = 0; k < neurony[1].Count(); k++)
                {
                    sumyZWejscia[j] += deltyZDrugiejWarstwy[k] * neurony[1][k].Wagi[j];
                }
            });
            Parallel.For(0, neurony[0].Count(), k => deltyZWejscia[k] = sumyZWejscia[k] * pochodneZWejscia[k]);
            delty.Add(deltyZWejscia);
            //4a. Uaktualnienie wag i biasu dla warstwy wejsciowej
            for (int i = 0; i < neurony[0].Count(); i++)
            {
                //for(int j = 0; j<neurony[0][i].Wagi.Count();j++)
                //{
                Parallel.For(0, neurony[0][i].Wagi.Count(), j =>
                neurony[0][i].Wagi[j] += eta * delty.ElementAt(iloscWarstw + 1)[i] * daneWejsciowe[j]);

                //}
                neurony[0][i].Bias += eta * delty.ElementAt(iloscWarstw + 1)[i];
            }
            //4b. Uaktualnianie wag i biasu dla warstw ukrytych i wyjsciowej
            for (int i = 1; i < iloscWarstw + 2; i++)
            {
                float[] daneZPopWarstwy = wyjsciaZWarstw.ElementAt(i - 1);
                float[] deltyZWarstwy = delty.ElementAt(delty.Count() - 1 - i);
                for (int j = 0; j < neurony[i].Count(); j++)
                {
                    //for (int k = 0; k < neurony[i][j].Wagi.Count(); k++)
                    //{
                    Parallel.For(0, neurony[i][j].Wagi.Count(), k =>
                    neurony[i][j].Wagi[k] += eta * deltyZWarstwy[j] * daneZPopWarstwy[k]);
                    //}
                    neurony[i][j].Bias += eta * deltyZWarstwy[j];
                }
            }

        }
    }

}