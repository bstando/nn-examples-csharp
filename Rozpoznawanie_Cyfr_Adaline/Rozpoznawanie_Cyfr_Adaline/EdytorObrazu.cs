using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rozpoznawanie_Cyfr_Adaline
{
    /// <summary>
    /// Pomocnicza klasa służąca do operacji na obrazach
    /// </summary>
    class EdytorObrazu
    {
        public EdytorObrazu()
        {

        }
        public int[] PobierzLiczbyZObrazu(WriteableBitmap obraz)
        {
            int[] wartoscZwracana = new int[25];
            WriteableBitmap skalowanyObraz = obraz.Resize(7, 5, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            for (int i = 0; i < 35; i++)
            {
                Color wyn = obraz.GetPixel(i % 5, (i - i % 5) / 5);
                if(wyn == Color.FromRgb(255,255,255))
                {
                    wartoscZwracana[i] = 0;
                }
                else
                {
                    wartoscZwracana[i] = 1;
                }
            }
            return wartoscZwracana;
        }
        public WriteableBitmap ZamalujObrazZLiczb(int[] tablica)
        {

            WriteableBitmap wynik = new WriteableBitmap(7, 5, 96, 96, PixelFormats.Bgr32, null);
            for(int i=0;i<35;i++)
            {
                wynik.SetPixeli(i, Color.FromRgb((byte)((1 - tablica[i]) * 255), (byte)((1 - tablica[i]) * 255), (byte)((1 - tablica[i]) * 255)));
            }
            wynik.Resize(300, 420, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
            return wynik;
        }
        public WriteableBitmap WyczyscINalozLinie()
        {
            WriteableBitmap wynik = new WriteableBitmap(300, 420, 96, 96, PixelFormats.Bgr32, null);
            wynik.Clear(Color.FromRgb(255, 255, 255));
            for (int j = 0; j < 8; j++)
            {
                wynik.DrawLine(0, j*60, 300, j*60, Color.FromRgb(0, 0, 0));
            }
            for (int i = 0; i < 6; i++)
            {
                wynik.DrawLine(i * 60, 0, i * 60, 420, Color.FromRgb(0, 0, 0));
            }
            wynik.DrawLine(299, 0, 299, 419, Color.FromRgb(0, 0, 0));
            wynik.DrawLine(0, 419, 300, 419, Color.FromRgb(0, 0, 0));
            return wynik;
        }
    }
}
