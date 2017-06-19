using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cudafy;
using Cudafy.Translator;
using Cudafy.Host;
using Cudafy.Atomics;

namespace BEP_Obrazy_RGB
{
    class BEP_CUDA
    {
        static float[, ,] neurony;
        static float[,] macierzWejsc;
        static float[,] macierzWyjsc;
        static float[,] macierzDelt;
        static float[,] macierzSum;
        static int iloscWejsc, iloscWyjsc, iloscWarstw;
        static int[] iloscNeuronowWWarstwie;
        static int[] wyjscia;
        [Cudafy]
        static float[,] macierzWejscGPU;
        [Cudafy]
        static float[,] macierzWyjscGPU;
        [Cudafy]
        static float[,] macierzDeltGPU;
        [Cudafy]
        static float[,] macierzSumGPU;
        [Cudafy]
        static float[, ,] neuronyGPU;
        [Cudafy]
        static float[] wejscieGPU;
        static float[] odpowiedz, odpowiedzGPU;
        [Cudafy]
        static int[] warstwyGPU, wyjsciaGPU;
        static int[] numerWarstwy, numerWarstwyGPU;
        static float[] stala, stalaGPU;
        static int[] iloscWejscWWarstwie, iloscWejscWWarstwieGPU;
        static CudafyModule km;
        static GPGPU gpu;

        public BEP_CUDA(int rozmiarWejsca, int rozmiarWyjscia, int glebokosc)
        {

            iloscWejsc = rozmiarWejsca;
            iloscWyjsc = rozmiarWyjscia;
            iloscWarstw = glebokosc + 2;
            neurony = new float[iloscWarstw, 32, 32];
            macierzDelt = new float[iloscWarstw, 32];
            macierzWyjsc = new float[iloscWarstw + 1, 32];
            macierzWejsc = new float[iloscWarstw + 1, 32];
            macierzSum = new float[iloscWarstw, 32];
            iloscNeuronowWWarstwie = new int[iloscWarstw];
            wyjscia = new int[iloscWarstw + 1];
            odpowiedz = new float[rozmiarWyjscia];

            numerWarstwy = new int[1];
            stala = new float[1];
            iloscWejscWWarstwie = new int[iloscWarstw];
            TworzNeurony();
            CzyscMacierze();
            CudafyTranslator.GenerateDebug = true;
            km = CudafyTranslator.Cudafy(eArchitecture.sm_30,typeof(BEP_CUDA));
            
            gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(km);
            neuronyGPU = gpu.CopyToDevice(neurony);
            //gpu.CopyToConstantMemory(neurony, neuronyGPU);
            macierzDeltGPU = gpu.CopyToDevice(macierzDelt);
            //gpu.CopyToConstantMemory(macierzDelt, macierzDeltGPU);
            macierzWejscGPU = gpu.CopyToDevice(macierzWejsc);
            //gpu.CopyToConstantMemory(macierzWejsc, macierzWejscGPU);
            macierzWyjscGPU = gpu.CopyToDevice(macierzWyjsc);
            //gpu.CopyToConstantMemory(macierzWyjsc, macierzWyjscGPU);
            macierzSumGPU = gpu.CopyToDevice(macierzSum);
            //gpu.CopyToConstantMemory(macierzSum, macierzSumGPU);
            wyjsciaGPU = gpu.CopyToDevice(wyjscia);
            //gpu.CopyToConstantMemory(wyjscia, wyjsciaGPU);
            warstwyGPU = gpu.CopyToDevice(iloscNeuronowWWarstwie);
            //gpu.CopyToConstantMemory(iloscNeuronowWWarstwie, warstwyGPU);
            iloscWejscWWarstwieGPU = gpu.CopyToDevice(iloscWejscWWarstwie);
        }
        public void Czysc()
        {
            gpu.FreeAll();
            gpu.Dispose();

        }
        void TworzNeurony()
        {
            Random randomizer = new Random(Guid.NewGuid().GetHashCode());
            iloscNeuronowWWarstwie[0] = iloscWejsc;
            iloscWejscWWarstwie[0] = iloscWejsc;
            iloscWejscWWarstwie[1] = iloscWejsc;
            for (int i = 1; i < iloscWarstw - 1; i++)
            {
                iloscNeuronowWWarstwie[i] = randomizer.Next(3, 30);
                iloscWejscWWarstwie[i + 1] = iloscNeuronowWWarstwie[i];
            }
            iloscNeuronowWWarstwie[iloscWarstw - 1] = iloscWyjsc;

            for (int i = 0; i < iloscWarstw; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    for (int k = 0; k < 32; k++)
                    {
                        neurony[i, j, k] = (float)randomizer.NextDouble() * 10f * Math.Sign(randomizer.NextDouble() - 1d / 2d);
                    }
                }
            }
        }
        //public BEP_CUDA(int rozmiarWejsca, int rozmiarWyjscia, int glebokosc, int[] rozmiarNeuronowWWarstwie) : base(rozmiarWejsca, rozmiarWyjscia, glebokosc, rozmiarNeuronowWWarstwie) { }
        void CzyscMacierze()
        {
            for (int i = 0; i < iloscWarstw; i++)
            {
                for (int j = 0; j < 32; j++)
                {

                    macierzWyjsc[i, j] = 0;
                    macierzWejsc[i, j] = 0;
                    macierzSum[i, j] = 0;
                    macierzDelt[i, j] = 0;
                }
            }
            for (int j = 0; j < 32; j++)
            {

                macierzWyjsc[iloscWarstw, j] = 0;
                macierzWejsc[iloscWarstw, j] = 0;
                //macierzSum[iloscWarstw, j] = 0;
            }
        }

        [Cudafy]
        public static void Dodaj(GThread watek, float[] a, float[] b, float[] c, int rozmiar)
        {
            int idWatka = watek.blockIdx.x;
            if (idWatka < rozmiar)
                c[idWatka] = a[idWatka] + b[idWatka];
        }

        [Cudafy]
        public static void LiczWarstweGPU(GThread watek, float[, ,] neuron, float[] wyjscie, int[] warstwa, float[,] macierzWejsc, float[,] macierzSum)
        {
            int x = watek.blockIdx.x;
            int y = watek.blockIdx.y;
            watek.atomicAdd(ref wyjscie[x], neuron[warstwa[0], x, y] * macierzWejsc[warstwa[0], y]);
            watek.SyncThreads();
            //wyjscie[x] += neuron[warstwa[0], x, y] * macierzWejsc[warstwa[0], y];
           
        }
        [Cudafy]
        public static void PrzeniesWynikDoMacierzySumGPU(GThread watek, float[,] macierzSum, float[] wyjscie, int[] warstwa)
        {
            int x = watek.blockIdx.x;
            macierzSum[warstwa[0], x] = wyjscie[x];
            watek.SyncThreads();
        }
        [Cudafy]
        public static void NastepnaWarstwaGPU(GThread watek, int[] warstwa)
        {
            warstwa[0]++;
        }
        [Cudafy]
        public static void PoliczFunkcjeAktywacji(GThread watek, int[] warstwa, float[,] macierzWejsc, float[,] macierzWyjsc, float[] sumy)
        {
            int x = watek.blockIdx.x;
            macierzWyjsc[warstwa[0], x] = 1f / (1f + GMath.Exp(-sumy[x]));
            macierzWejsc[warstwa[0] + 1, x] = macierzWyjsc[warstwa[0], x];
            watek.SyncThreads();
          //  watek.SyncThreads();
        }
        [Cudafy]
        public static void ZerujWektorFloat(GThread watek, float[] wektor)
        {
            int x = watek.blockIdx.x;
            wektor[x] = 0f;
            watek.SyncThreads();
           // watek.SyncThreads();
        }
        [Cudafy]
        public static void ZerujMacierzFloat(GThread watek, float[,] macierz)
        {
            int x = watek.blockIdx.x;
            int y = watek.blockIdx.y;
            macierz[x, y] = 0f;
            watek.SyncThreads();
           // watek.SyncThreads();
        }
        void czyscMacierzeGPU()
        {
            gpu.Launch(new dim3(iloscWarstw, 32), 1).ZerujMacierzFloat(macierzDeltGPU);
            gpu.Launch(new dim3(iloscWarstw, 32), 1).ZerujMacierzFloat(macierzSumGPU);
            gpu.Launch(new dim3(iloscWarstw, 32), 1).ZerujMacierzFloat(macierzWejscGPU);
            gpu.Launch(new dim3(iloscWarstw, 32), 1).ZerujMacierzFloat(macierzWyjscGPU);

        }
        public System.Windows.Media.Color KlasyfikujGPU(float[] wejscie)
        {
            czyscMacierzeGPU();
            numerWarstwy[0] = 0;
            numerWarstwyGPU = gpu.CopyToDevice(numerWarstwy);

            float[] wyjscieGPU = gpu.Allocate<float>(32);
            float[] wejscieGPU = gpu.CopyToDevice<float>(wejscie);

            gpu.Launch(new dim3(wejscie.Length), new dim3(1)).UzupelnijWejscie(macierzWejscGPU, wejscieGPU);
            gpu.Launch(new dim3(32), new dim3(1)).ZerujWektorFloat(wyjscieGPU);

            for (numerWarstwy[0] = 0; numerWarstwy[0] < iloscWarstw; numerWarstwy[0]++)
            {
                numerWarstwyGPU = gpu.CopyToDevice(numerWarstwy);
                dim3 x = new dim3(iloscNeuronowWWarstwie[numerWarstwy[0]]);
                dim3 y = new dim3(iloscWejscWWarstwie[numerWarstwy[0]]);

                gpu.Launch(32, 1).ZerujWektorFloat(wyjscieGPU);
                gpu.Launch(new dim3(iloscNeuronowWWarstwie[numerWarstwy[0]], iloscWejscWWarstwie[numerWarstwy[0]]), 1).LiczWarstweGPU(neuronyGPU, wyjscieGPU, numerWarstwyGPU, macierzWejscGPU, macierzSumGPU);
                gpu.Synchronize();
                gpu.Launch(x, 1).PoliczFunkcjeAktywacji(numerWarstwyGPU, macierzWejscGPU, macierzWyjscGPU, wyjscieGPU);
                gpu.Free(numerWarstwyGPU);
                //float[] tmp = new float[iloscNeuronowWWarstwie[numerWarstwy[0]]];
                //gpu.CopyFromDevice(macierzWejscGPU, macierzWejsc);
                //gpu.CopyFromDevice(wyjscieZGPU, tmp);
                //int ddd = 0;
                //ddd++;
            }
            gpu.CopyFromDevice(macierzWyjscGPU, macierzWyjsc);
            gpu.Free(wyjscieGPU);
            System.Windows.Media.Color ret = new System.Windows.Media.Color();
            ret.ScR = macierzWyjsc[iloscWarstw - 1, 0];
            ret.ScG = macierzWyjsc[iloscWarstw - 1, 1];
            ret.ScB = macierzWyjsc[iloscWarstw - 1, 2];
            ret.ScA = 1;
            return ret;
        }
        [Cudafy]
        public static void UzupelnijWarstwe(GThread watek, float[, ,] neuron, float[,] macierzWejsc, float[,] macierzWyjsc, int[] warstwa)
        {
            int x = watek.blockIdx.x;
            int y = watek.blockIdx.y;
            watek.atomicAdd(ref macierzWyjsc[warstwa[0], x], neuron[warstwa[0], x, y] * macierzWejsc[warstwa[0], y]);
            macierzWejsc[warstwa[0] + 1, x] = macierzWyjsc[warstwa[0], x];
            watek.SyncThreads();
        }
        [Cudafy]
        public static void UzupelnijWejscie(GThread watek, float[,] macierzWejsc, float[] wektorWejsciowy)
        {
            int x = watek.blockIdx.x;
            macierzWejsc[0, x] = wektorWejsciowy[x];
            watek.SyncThreads();
           // watek.SyncThreads();
        }
        public System.Windows.Media.Color Klasyfikuj(float[] wejscie)
        {
            return KlasyfikujGPU(wejscie);
        }
        [Cudafy]
        public static void LiczDelteOstatniejWarstwyGPU(GThread watek, float[,] macierzDelt, float[,] macierzWyjsc, int[] warstwa, float[] odpowiedz)
        {
            int x = watek.blockIdx.x;
            macierzDelt[warstwa[0], x] = -1f * (macierzWyjsc[warstwa[0], x] - odpowiedz[x]) * macierzWyjsc[warstwa[0], x] * (1f - macierzWyjsc[warstwa[0], x]);
            watek.SyncThreads();
        }
        [Cudafy]
        public static void LiczSumyWarstwyGPU(GThread watek, int[] warstwa, float[,] macierzDelt, float[,] macierzSum, float[, ,] neuron)
        {
            int x = watek.blockIdx.x;
            int y = watek.blockIdx.y;
            watek.atomicAdd(ref  macierzSum[warstwa[0], x], macierzDelt[warstwa[0] + 1, y] * neuron[warstwa[0] + 1, y, x]);
            //macierzSum[warstwa[0], x] += macierzDelt[warstwa[0] + 1, y] * neuron[warstwa[0] + 1, y, x];
            watek.SyncThreads();
        }
        [Cudafy]
        public static void LiczDeltyWarstwyGPU(GThread watek, int[] warstwa, float[,] macierzDelt, float[,] macierzWyjsc, float[,] macierzSum)
        {
            int x = watek.blockIdx.x;
            macierzDelt[warstwa[0], x] = macierzSum[warstwa[0], x] * macierzWyjsc[warstwa[0], x] * (1 - macierzWyjsc[warstwa[0], x]);
            watek.SyncThreads();
        }
        [Cudafy]
        public static void UaktualnijWagiNeuronowGPU(GThread watek, float[, ,] neuron, float[,] macierzDelt, float[,] macierzWejsc, float[] stala)
        {
            int x = watek.blockIdx.x;
            int y = watek.blockIdx.y;
            int z = 0;
            int len = neuron.GetLength(2);
            //neuron[x, y, z] += stala[0] * macierzDelt[x, y] * macierzWejsc[x, z];
            while(z<len)
            {
                watek.atomicAdd(ref neuron[x, y, z], stala[0] * macierzDelt[x, y] * macierzWejsc[x, z]);
                z++;
            }
            watek.SyncThreads();
            //neuron[x, y, z+x] = 1;
        }
        public void UczMacierzowoGPU(float[] daneWejsciowe, float eta, System.Windows.Media.Color kolor)
        {
            czyscMacierzeGPU();
            Klasyfikuj(daneWejsciowe);
            //float[] odpowiedz = new float[3];
            odpowiedz[0] = kolor.ScR;
            odpowiedz[1] = kolor.ScG;
            odpowiedz[2] = kolor.ScB;
            odpowiedzGPU = gpu.CopyToDevice(odpowiedz);
            numerWarstwy[0] = iloscWarstw - 1;
            numerWarstwyGPU = gpu.CopyToDevice(numerWarstwy);

            gpu.Launch(3, 1).LiczDelteOstatniejWarstwyGPU(macierzDeltGPU, macierzWyjscGPU, numerWarstwyGPU, odpowiedzGPU);

            gpu.Launch(new dim3(iloscWarstw, 32), 1).ZerujMacierzFloat(macierzSumGPU);
            gpu.Free(numerWarstwyGPU);
            for (numerWarstwy[0] = iloscWarstw - 2; numerWarstwy[0] >= 0; numerWarstwy[0]--)
            {
                numerWarstwyGPU = gpu.CopyToDevice(numerWarstwy);
                gpu.Launch(new dim3(iloscNeuronowWWarstwie[numerWarstwy[0]], iloscNeuronowWWarstwie[numerWarstwy[0]]), 1).LiczSumyWarstwyGPU(numerWarstwyGPU, macierzDeltGPU, macierzSumGPU, neuronyGPU);
                gpu.Launch(iloscNeuronowWWarstwie[numerWarstwy[0]], 1).LiczDeltyWarstwyGPU(numerWarstwyGPU, macierzDeltGPU, macierzWyjscGPU, macierzSumGPU);
                gpu.Free(numerWarstwyGPU);
            }
            stala[0] = eta;
            stalaGPU = gpu.CopyToDevice(stala);
            //gpu.CopyFromDevice(neuronyGPU, neurony);
            gpu.Launch(new dim3(iloscWarstw, 32), 1).UaktualnijWagiNeuronowGPU(neuronyGPU, macierzDeltGPU, macierzWejscGPU, stalaGPU);
            gpu.Free(odpowiedzGPU);
            
            //gpu.CopyFromDevice(neuronyGPU, neurony);
            //gpu.CopyFromDevice(macierzWyjscGPU, macierzWyjsc);
            //gpu.CopyFromDevice(macierzWejscGPU, macierzWejsc);
            //gpu.CopyFromDevice(macierzSumGPU, macierzSum);
            //gpu.CopyFromDevice(macierzDeltGPU, macierzDelt);
            //int s = 1;
            //s++;

        }

        /*
        public void UczMacierzowo(float[] daneWejsciowe, float eta, System.Windows.Media.Color kolor)
        {
            CzyscMacierze();
           

            Parallel.For(0, neurony[0].Count(), i =>
            {
                macierzWejsc[0][i] = daneWejsciowe[i];
            });

            for (int i = 0; i < iloscWarstw + 2; i++)
            {
                float[] a = gpu.CopyToDevice(macierzWejsc[i]);
                for(int j = 0;j<neurony[i].Count();j++)
                {
                    float[] b = gpu.CopyToDevice(neurony[i][j].Wagi);
                    float[] t = new float[1];
                    t[0] = neurony[i][j].Bias;
                    float[] w = gpu.CopyToDevice(t);
                    
                    gpu.Launch(neurony[i].Count(), 1).KlasyfikujGPU(a,b,w,neurony[i].Count());
                    gpu.CopyFromDevice(w,t);
                    macierzWyjsc[i][j] = 1f/(float)(1+Math.Exp(-(float)t[0]));
                    macierzWejsc[i + 1][j] = macierzWyjsc[i][j];
                }

            }

            macierzDelt[iloscWarstw + 1][0] = -1f * (macierzWyjsc[iloscWarstw + 1][0] - kolor.ScR) * macierzWyjsc[iloscWarstw + 1][0] * (1f - macierzWyjsc[iloscWarstw + 1][0]);
            macierzDelt[iloscWarstw + 1][1] = -1f * (macierzWyjsc[iloscWarstw + 1][1] - kolor.ScG) * macierzWyjsc[iloscWarstw + 1][1] * (1f - macierzWyjsc[iloscWarstw + 1][1]);
            macierzDelt[iloscWarstw + 1][2] = -1f * (macierzWyjsc[iloscWarstw + 1][2] - kolor.ScB) * macierzWyjsc[iloscWarstw + 1][2] * (1f - macierzWyjsc[iloscWarstw + 1][2]);
            
            for (int k = iloscWarstw; k >= 0; k--)
            {
                
                Parallel.For(0, neurony[k].Count(), j =>
                {
                    Parallel.For(0, neurony[k + 1].Count(), i => macierzSum[k][j] += macierzDelt[k + 1][i] * neurony[k + 1][i].Wagi[j]);
                }
                    );
                Parallel.For(0, neurony[k].Count(), i => macierzDelt[k][i] = macierzSum[k][i] * macierzWyjsc[k][i] * (1 - macierzWyjsc[k][i]));
            }
            for (int k = 0; k < iloscWarstw + 2; k++)
            {
                Parallel.For(0, neurony[k].Count(), i =>
                {
                    Parallel.For(0, neurony[k][i].Wagi.Count(), j => neurony[k][i].Wagi[j] += eta * macierzDelt[k][i] * macierzWejsc[k][j]);
                    neurony[k][i].Bias -= eta * macierzDelt[k][i];
                });
            }
        }
         */
    }
}
