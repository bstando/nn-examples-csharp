using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.IntegralTransforms;
using System.Numerics;
using FFTWSharp;
using System.Runtime.InteropServices;

namespace Rozpoznawanie_Cyfr_Adaline
{
    class Transformata
    {
        public Transformata()
        {
        }
        
        public double[][] WykonajTrasnformate(int[][] dane)
        {
            List<double[]> wynik = new List<double[]>();
            
            foreach(int[] przyklad in dane)
            {
                Complex[][] doTrasnformaty = new Complex[5][];
            
                double[] din, dout;

                GCHandle hdin, hdout;

                IntPtr fplan1;



                din = new double[70];
                dout = new double[70];


                for (int i = 0; i < 35; i++)
                {
                    din[i*2] = przyklad[i];
                    din[i * 2 + 1] = 0;
                } 

                hdin = GCHandle.Alloc(din, GCHandleType.Pinned);
                hdout = GCHandle.Alloc(dout, GCHandleType.Pinned);

                fplan1 = fftw.dft_2d(7, 5, hdin.AddrOfPinnedObject(), hdout.AddrOfPinnedObject(), fftw_direction.Forward, fftw_flags.Estimate);
                fftw.execute(fplan1);
                

                double[] gotowy = new double[35];
                for(int i =0;i<35;i++)
                {
                    gotowy[i] = Math.Sqrt(dout[i * 2] * dout[2 * i] + dout[i * 2 + 1] * dout[2 * i + 1]);
                   
                }
                wynik.Add(gotowy);
                fftw.destroy_plan(fplan1);
            }           
            return wynik.ToArray();
        }
        public double[] WykonajTransformateDlaPrzykadu(int[] dane)
        {
            double[] wynik = new double[35];

            Complex[][] doTrasnformaty = new Complex[5][];

            double[] din, dout;

            GCHandle hdin, hdout;

            IntPtr fplan1;



            din = new double[70];
            dout = new double[70];


            for (int i = 0; i < 35; i++)
            {
                din[i * 2] = dane[i];
                din[i * 2 + 1] = 0;
            }

            hdin = GCHandle.Alloc(din, GCHandleType.Pinned);
            hdout = GCHandle.Alloc(dout, GCHandleType.Pinned);

            fplan1 = fftw.dft_2d(7, 5, hdin.AddrOfPinnedObject(), hdout.AddrOfPinnedObject(), fftw_direction.Forward, fftw_flags.Estimate);
            fftw.execute(fplan1);

            double[] gotowy = new double[35];
            for (int i = 0; i < 35; i++)
            {
                wynik[i] = Math.Sqrt(dout[i * 2] * dout[2 * i] + dout[i * 2 + 1] * dout[2 * i + 1]);
            }
                
            return wynik;
        }
    }
}
