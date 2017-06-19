using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rozpoznawanie_Cyfr_Adaline
{
    /// <summary>
    /// Interaction logic for WizualizacjaDFT.xaml
    /// </summary>
    public partial class WizualizacjaDFT : Window
    {
        double[] wynikDFT;
        public WizualizacjaDFT(double[] wejscie)
        {
            InitializeComponent();
            wynikDFT = wejscie;
            WriteableBitmap w = new WriteableBitmap(7, 5, 96, 96, PixelFormats.Bgr32, null);
            for(int i=0;i<35;i++)
            {
                w.SetPixeli(i, Color.FromRgb((byte)((wejscie[i] * 100) % 255), (byte)((wejscie[i] * 100) % 255), (byte)((wejscie[i] * 100) % 255)));
            }
            obrazDFT.Source = w.Resize(300, 420, WriteableBitmapExtensions.Interpolation.NearestNeighbor);
        }
    }
}
