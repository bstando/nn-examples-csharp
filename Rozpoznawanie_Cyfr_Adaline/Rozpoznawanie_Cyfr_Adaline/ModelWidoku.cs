using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rozpoznawanie_Cyfr_Adaline
{
    class ModelWidoku
    {
        public ModelWidoku(List<double> liczby)
        {
            this.Title = "Wykres błędu";
            this.Points = new List<DataPoint>();
            for(int i =0;i<liczby.Count;i++)
            {
                this.Points.Add(new DataPoint(i, liczby.ElementAt(i)));
            }
        }

        public ModelWidoku()
        {
            this.Title = "Wykres błędu";
            Wczytaj();
        }

        private void Wczytaj()
        {
            List<double> liczby = new List<double>();
            String[] dane = System.IO.File.ReadAllLines("blad.txt");
            foreach(String d in dane)
            {
                double w = Double.Parse(d);
                liczby.Add(w);
            }
            this.Points = new List<DataPoint>();
            for (int i = 0; i < liczby.Count; i++)
            {
                //if(i%100==0)
                this.Points.Add(new DataPoint(i, liczby.ElementAt(i)));
            }
        }
        public string Title { get; set; }

        public List<DataPoint> Points { get; set; }
    }
}
