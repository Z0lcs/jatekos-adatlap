using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyilvántartó
{
    internal class Jatekos
    {
        public string Nev { get; set; }
        public int Eletkor { get; set; }
        public string Csapat { get; set; }
        public int Meccs { get; set; }
        public string Eredmeny { get; set; }
        public int Gol { get; set; }
        public int Bunteto { get; set; }
        public int SargaLap { get; set; }
        public int Kiallitas { get; set; }
        public int PirosLap { get; set; }

        public Jatekos(string nev, int kor, string csapat, int meccs, string eredmeny, int gol, int bunteto, int sarga, int kiallitas, int piros)
        {
            this.Nev = nev.Length > 20 ? nev.Substring(0, 20) : nev;
            this.Eletkor = kor;
            this.Csapat = csapat.Length > 25 ? csapat.Substring(0, 25) : csapat;
            this.Meccs = meccs;
            this.Eredmeny = eredmeny;
            this.Gol = gol;
            this.Bunteto = bunteto;
            this.SargaLap = sarga;
            this.Kiallitas = kiallitas;
            this.PirosLap = piros;

            if (meccs < 0) meccs = 0;
            if (gol < 0) gol = 0;
            if (bunteto < 0) bunteto = 0;
            if (sarga < 0) sarga = 0;
            if (kiallitas < 0) kiallitas = 0;
            if (piros < 0) piros = 0;

        }
    }
}
