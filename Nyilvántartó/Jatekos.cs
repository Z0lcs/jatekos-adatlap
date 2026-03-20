using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Jatekos
{
    public string Nev { get; set; }
    public int Eletkor { get; set; }
    public string Csapat { get; set; }
    public int Meccs { get; set; }
    public int Gyozelem { get; set; }
    public int Dontetlen { get; set; }
    public int Vereseg { get; set; }
    public int Gol { get; set; }
    public int Bunteto { get; set; }
    public int SargaLap { get; set; }
    public int Kiallitas { get; set; }
    public int PirosLap { get; set; }

    public Jatekos(string nev, int kor, string csapat, int meccs, int gyoz, int dont, int ver, int gol, int bunteto, int sarga, int kiallitas, int piros)
    {
        this.Nev = nev.Length > 20 ? nev.Substring(0, 20) : nev;
        this.Eletkor = Math.Max(0, Math.Min(50, kor));
        this.Csapat = csapat.Length > 25 ? csapat.Substring(0, 25) : csapat;
        this.Meccs = meccs < 0 ? 0 : meccs;
        this.Gyozelem = Math.Max(0, gyoz);
        this.Dontetlen = Math.Max(0, dont);
        this.Vereseg = Math.Max(0, ver);
        this.Gol = Math.Max(0, gol);
        this.Bunteto = Math.Max(0, bunteto);
        this.SargaLap = Math.Max(0, sarga);
        this.Kiallitas = Math.Max(0, kiallitas);
        this.PirosLap = Math.Max(0, piros);
    }
}
