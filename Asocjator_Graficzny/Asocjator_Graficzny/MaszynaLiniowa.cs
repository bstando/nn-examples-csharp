using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asocjator_Graficzny
{
    class MaszynaLiniowa
    {
        int iloscPikseliWObrazie;
        int iloscPerceptronow;
        Perceptron[] perceptrony;
        
        int[][] przyklady;
        //int[][] nieprzejrzanePrzyklady;
        
        int numerMaszyny;
        int liczbaNieprzejrzanychPrzykladow;
        int liczbaPrzykladow;
        public MaszynaLiniowa(int indeks,int rozmiarDanychWejsciowych,int liczbaPerceptronow,int[][] przykladoweDane,int iloscPrzykladow)
        {
            numerMaszyny = indeks;
            liczbaPrzykladow = iloscPrzykladow;
            liczbaNieprzejrzanychPrzykladow = iloscPrzykladow;
            
            iloscPikseliWObrazie = rozmiarDanychWejsciowych;
            
            iloscPerceptronow = liczbaPerceptronow;
            
            przyklady = przykladoweDane;
            //nieprzejrzanePrzyklady = przykladoweDane;
            
            perceptrony = new Perceptron[liczbaPerceptronow];

            for (int i = 0; i < liczbaPerceptronow; i++)
            {
                perceptrony[i] = new Perceptron(rozmiarDanychWejsciowych);
            }
        }
        private int[] PobierzPrzykladZListy()
        {
            Random randomizer = new Random();
            int indeks = randomizer.Next(liczbaNieprzejrzanychPrzykladow);
            int[] retval = przyklady[indeks];
            przyklady[indeks] = przyklady[liczbaNieprzejrzanychPrzykladow - 1];
	        przyklady[liczbaNieprzejrzanychPrzykladow - 1] = retval;
            liczbaNieprzejrzanychPrzykladow--;
            return retval;
        }

        private void PoprawWagiPercetronu(Perceptron p, int[] poprawka, int znak)
        {
            double[] wagi = p.Wagi;
            for(int i=0;i<iloscPikseliWObrazie;i++)
            {
                wagi[i] += poprawka[i]*znak;
            }
            p.Wagi = wagi;
            p.Bias += poprawka[0];
        }

        public int Klasyfikuj(int[] przyklad)
        {
            int indeks = 0;
            for(int i=0;i<iloscPerceptronow;i++)
                if (perceptrony[indeks].PrzeliczObraz(przyklad) < perceptrony[i].PrzeliczObraz(przyklad))
                    indeks = i;
            return indeks;
        }
        public void UczMaszyne()
        {
            for(int i = 0; i<iloscPerceptronow;i++)
            {
                perceptrony[i].LosujWagi();
            }
            //bool pass = false;
            //while (!pass)
            //{
            //    pass = true;
            //    while (liczbaNieprzejrzanychPrzykladow > 0)
            //    {
            //        int[] wylosowanyPrzyklad = PobierzPrzykladZListy();

            //        int klasyfikacja = Klasyfikuj(wylosowanyPrzyklad);
            //        int wlasciwaOdpowiedz = wylosowanyPrzyklad[numerMaszyny];

            //        if (klasyfikacja != wlasciwaOdpowiedz)
            //        {
            //            PoprawWagiPercetronu(perceptrony[wlasciwaOdpowiedz], wylosowanyPrzyklad, 1);
            //            PoprawWagiPercetronu(perceptrony[klasyfikacja], wylosowanyPrzyklad, -1);
            //            liczbaNieprzejrzanychPrzykladow = liczbaPrzykladow;
            //            pass = false;
            //        }
            //    }
            //    if (pass == false) liczbaNieprzejrzanychPrzykladow = liczbaPrzykladow;
            //}
            bool pass = false;
            while (!pass)
            {
                pass = true;
                foreach (int[] wylosowanyPrzyklad in przyklady)
                {
                    int klasyfikacja = Klasyfikuj(wylosowanyPrzyklad);
                    int wlasciwaOdpowiedz = wylosowanyPrzyklad[numerMaszyny];

                    if (klasyfikacja != wlasciwaOdpowiedz)
                    {
                        PoprawWagiPercetronu(perceptrony[wlasciwaOdpowiedz], wylosowanyPrzyklad, 1);
                        PoprawWagiPercetronu(perceptrony[klasyfikacja], wylosowanyPrzyklad, -1);
                        //iczbaNieprzejrzanychPrzykladow = liczbaPrzykladow;
                        pass = false;
                    }
                }

            }


        }
    }
}
