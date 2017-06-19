using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEP_Obrazy_RGB
{
    struct RGB
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        bool CompareTo(RGB a)
        {
            return((this.R==a.R)&&(this.G==a.G)&&(this.B==a.B));
        }
        //public RGB(byte r, byte g, byte b)
        //{
        //    R = r;
        //    G = g;
        //    B = b;
        //}

        //public RGB()
        //{
        //    R = 0;
        //    G = 0;
        //    B = 0;
        //}
    }
}
