using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyilvántartó
{
    public class UI
    {
        public static void MenuRajzolas(string[] hosszuElemek, string[] rovidElemek, int kivalasztottIndex)
        {
            ConsoleColor[] hatterSzinek =
            {
            ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkBlue,
            ConsoleColor.DarkYellow, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan
            };

            int elemekSzama = hosszuElemek.Length;
            if (elemekSzama == 0) return;

            int konzolSzelesseg = Console.WindowWidth;
            int belsoTerkoz = 8;
            int dobozokKoztiTavolsag = 3;

            int elmeletiTeljesSzelesseg = 0;
            for (int i = 0; i < elemekSzama; i++)
            {
                elmeletiTeljesSzelesseg += hosszuElemek[i].Length + belsoTerkoz + dobozokKoztiTavolsag;
            }
            elmeletiTeljesSzelesseg -= 1;

            string[] megjelenitendoElemek = elmeletiTeljesSzelesseg > konzolSzelesseg ? rovidElemek : hosszuElemek;

            int veglegesTeljesSzelesseg = 0;
            int[] elemSzelessegek = new int[elemekSzama];

            for (int i = 0; i < elemekSzama; i++)
            {
                elemSzelessegek[i] = megjelenitendoElemek[i].Length + belsoTerkoz;
                veglegesTeljesSzelesseg += elemSzelessegek[i] + dobozokKoztiTavolsag;
            }
            veglegesTeljesSzelesseg -= 1;

            int balMargo = Math.Max(0, (konzolSzelesseg - veglegesTeljesSzelesseg) / 2);

            Console.SetCursorPosition(balMargo, Console.CursorTop);
            for (int i = 0; i < elemekSzama; i++)
            {
                string fGombSzoveg = $"F{i + 1}";
                int vonalHossz = Math.Max(0, elemSzelessegek[i] - fGombSzoveg.Length - 2);
                int balVonalHossz = vonalHossz / 2;
                int jobbVonalHossz = vonalHossz - balVonalHossz;

                string felsoKeret = new string('═', balVonalHossz) + " " + fGombSzoveg + " " + new string('═', jobbVonalHossz);
                string zaroTavolsag = (i == elemekSzama - 1) ? "" : " ";

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("╔" + felsoKeret + "╗" + zaroTavolsag);
            }
            Console.WriteLine();

            Console.SetCursorPosition(balMargo, Console.CursorTop);
            for (int i = 0; i < elemekSzama; i++)
            {
                string aktualisSzoveg = megjelenitendoElemek[i];
                int uresHely = elemSzelessegek[i] - aktualisSzoveg.Length;
                int balSzokozDb = uresHely / 2;
                int jobbSzokozDb = uresHely - balSzokozDb;

                string kozepezettSzoveg = new string(' ', balSzokozDb) + aktualisSzoveg + new string(' ', jobbSzokozDb);
                ConsoleColor alapHatterSzin = (i < hatterSzinek.Length) ? hatterSzinek[i] : ConsoleColor.DarkGray;


                ConsoleColor dobozHatterSzin = alapHatterSzin;
                ConsoleColor dobozSzovegSzin = ConsoleColor.White;

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("║");

                Console.BackgroundColor = dobozHatterSzin;
                Console.ForegroundColor = dobozSzovegSzin;
                Console.Write(kozepezettSzoveg);

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("║");

                if (i < elemekSzama - 1)
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();

            Console.SetCursorPosition(balMargo, Console.CursorTop);
            for (int i = 0; i < elemekSzama; i++)
            {
                string alsoKeret = new string('═', elemSzelessegek[i]);
                string zaroTavolsag = (i == elemekSzama - 1) ? "" : " ";

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("╚" + alsoKeret + "╝" + zaroTavolsag);
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }
        public static void ListaMegjelenitese(List<Jatekos> jatekosLista)
        {
            string cim = "═══ Játékosok Listája ═══";
            int cimMargo = Math.Max(0, (Console.WindowWidth - cim.Length) / 2);
            Console.WriteLine();
            Console.SetCursorPosition(cimMargo, Console.CursorTop);
            Console.WriteLine(cim);
            Console.WriteLine();

            if (jatekosLista.Count == 0)
            {
                string uzenet = "A nyilvántartás jelenleg üres. Vegyél fel egy új játékost az F2 gombbal!";
                Console.SetCursorPosition(Math.Max(0, (Console.WindowWidth - uzenet.Length) / 2), Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(uzenet);
                Console.ResetColor();
                return;
            }

            int tablaSzelesseg = 120;
            int balMargo = Math.Max(0, (Console.WindowWidth - tablaSzelesseg) / 2);

            Console.SetCursorPosition(balMargo, Console.CursorTop);
            Console.WriteLine("╔══════════════════════╦═════╦══════════════════════════╦═══════╦═════╦═════╦═════╦═════╦══════╦═══════╦═══════╦═══════╗");

            Console.SetCursorPosition(balMargo, Console.CursorTop);
            Console.WriteLine($"║ {"Név",-20} ║ {"Kor",3} ║ {"Csapat",-24} ║ {"Meccs",5} ║ {"Gy",3} ║ {"D",2}  ║ {"V",2}  ║ {"Gól",3} ║ {"Bünt",4} ║ {"Sárga",5} ║ {"Kiáll",5} ║ {"Piros",5} ║");

            Console.SetCursorPosition(balMargo, Console.CursorTop);
            Console.WriteLine("╠══════════════════════╬═════╬══════════════════════════╬═══════╬═════╬═════╬═════╬═════╬══════╬═══════╬═══════╬═══════╣");

            for (int i = 0; i < jatekosLista.Count; i++)
            {
                var j = jatekosLista[i];

                Console.SetCursorPosition(balMargo, Console.CursorTop);
                Console.ForegroundColor = (i % 2 == 0) ? ConsoleColor.White : ConsoleColor.Gray;
                Console.WriteLine($"║ {j.Nev,-20} ║ {j.Eletkor,3} ║ {j.Csapat,-24} ║ {j.Meccs,5} ║ {j.Gyozelem,3} ║ {j.Dontetlen,2}  ║ {j.Vereseg,2}  ║ {j.Gol,3} ║ {j.Bunteto,4} ║ {j.SargaLap,5} ║ {j.Kiallitas,5} ║ {j.PirosLap,5} ║");
            }

            Console.SetCursorPosition(balMargo, Console.CursorTop);
            Console.WriteLine("╚══════════════════════╩═════╩══════════════════════════╩═══════╩═════╩═════╩═════╩═════╩══════╩═══════╩═══════╩═══════╝");
            Console.WriteLine();
        }
    }
}
