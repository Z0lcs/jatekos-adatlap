using System;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

namespace Nyilvántartó
{
    class Program
    {
        static List<Jatekos> jatekosok = new List<Jatekos>();
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
                            // nev, kor, csapat, meccs, gyoz, dont, ver, gol, bunteto, sarga, kiallitas, piros
            jatekosok.Add(new Jatekos("Bánhidi Bence", 29, "Pick Szeged", 24, 17,2,5, 85, 0, 4, 12, 1));
            jatekosok.Add(new Jatekos("Lékai Máté", 35, "Ferencváros", 26, 15, 3,8,92, 15, 2, 3, 0));
            jatekosok.Add(new Jatekos("Mikler Roland", 39, "Pick Szeged", 25, 18, 2,5,1, 0, 1, 0, 0));
            jatekosok.Add(new Jatekos("Klujber Katrin", 24, "FTC-Rail Cargo", 26, 20,2,4, 145, 42, 3, 5, 0));
            jatekosok.Add(new Jatekos("Böde-Bíró Blanka", 29, "FTC-Rail Cargo", 22, 16,1,5, 2, 0, 0, 0, 0));

            string[] hosszuMenupontok = ["Megtekintés", "Felvétel", "Módosítás", "Törlés", "Kilépés"];
            bool futAProgram = true;

            Console.CursorVisible = false;

            while (futAProgram)
            {
                Console.Clear();

                UI.MenuRajzolas(hosszuMenupontok, [], -1);

                ConsoleKeyInfo gombNyomas = Console.ReadKey(true);

                switch (gombNyomas.Key)
                {
                    case ConsoleKey.F1:
                        futAProgram = FunkcioInditasa(0);
                        break;
                    case ConsoleKey.F2:
                        futAProgram = FunkcioInditasa(1);
                        break;
                    case ConsoleKey.F3:
                        futAProgram = FunkcioInditasa(2);
                        break;
                    case ConsoleKey.F4:
                        futAProgram = FunkcioInditasa(3);
                        break;
                    case ConsoleKey.F5:
                        futAProgram = FunkcioInditasa(4);
                        break;
                }
            }
        }

        static bool FunkcioInditasa(int index)
        {
            Console.Clear();
            switch (index)
            {
                case 0:
                    //Console.WriteLine("═══ Megtekintés ═══");
                    UI.ListaMegjelenitese(jatekosok);
                    break;
                case 1:
                    //Console.WriteLine("═══ Új játékos felvétele ═══");
                    JatekosFelvetel();
                    break;
                case 2:
                    Console.WriteLine("═══ Játékos módosítása ═══");
                    // Ide jön a módosítás
                    break;
                case 3:
                    //Console.WriteLine("═══ Játékos törlése ═══");
                    JatekosTorol();
                    break;
                case 4:
                    // Kilépés
                    return false;
            }

            Console.WriteLine("\nNyomj meg egy gombot a visszalépéshez...");
            Console.ReadKey(true);
            return true;
        }
        static void JatekosFelvetel()
        {
            string nev =""; 
            string csapat ="";
            int kor, meccs, gol, bunteto, sarga, kiallitas, piros;
            int gyoz=0; 
            int dont=0; 
            int ver=0;
            Console.Clear();
            bool nevValasztasJo = false;
            while (!nevValasztasJo)
            {
                Console.Write("Add meg a játékos nevét: ");
                nev = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nev) || nev.Any(char.IsDigit))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hiba: A név nem lehet üres, és nem lehet benne szám!");
                    Console.ResetColor();
                    nevValasztasJo = false;
                }
                else
                    nevValasztasJo = true;
            }
            Console.WriteLine();

            kor = Szambekeres("Add meg a játékos életkorát (15 – 50): ", 15, 50);
            Console.WriteLine();

            bool csapatValasztasJo = false;
            while (!csapatValasztasJo)
            {
                Console.Write("Add meg a játékos nevét: ");
                csapat = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(csapat))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hiba: A név nem lehet üres!");
                    Console.ResetColor();
                    csapatValasztasJo = false;
                }
                else
                    csapatValasztasJo = true;
            }
            Console.WriteLine();

            meccs = Szambekeres("Add meg, hogy hány meccset játszott a játékos: ", 0, 999);
            Console.WriteLine();

            Console.WriteLine("Add meg a játékos meccseinek eredményeit: ");
            while (true)
            {
                gyoz = Szambekeres("Győzelem: ", 0, 999);
                dont = Szambekeres("Döntetlen: ", 0, 999);
                ver = Szambekeres("Vereség: ", 0, 999);
                if (gyoz + dont + ver < meccs || gyoz + dont + ver > meccs)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Hiba: az összes eredmény számának egyeznie kell a meccsek számával!");
                    Console.ResetColor();
                }
                else break;
            }
            Console.WriteLine();

            Console.WriteLine("Add meg a játékos meccsen elért statisztikáit: ");
            gol = Szambekeres("Gólok száma: ", 0, 999);
            bunteto = Szambekeres("Belőtt büntetők száma: ", 0, 999);
            sarga = Szambekeres("Sárga lapok száma: ", 0, 999);
            kiallitas = Szambekeres("2 perces kiállítások száma: ", 0, 999);
            piros = Szambekeres("Piros lapok száma: ", 0, 999);

            jatekosok.Add(new Jatekos(nev, kor, csapat, meccs, gyoz,dont,ver, gol, bunteto, sarga, kiallitas, piros));
        }
        static void JatekosTorol()
        {
            Console.Clear();
            Console.Write("Add meg a törölni kívánt játékos teljes nevét: ");
            UI.ListaMegjelenitese(jatekosok);
            string keresettNev = Console.ReadLine();
            Jatekos talalat = jatekosok.FirstOrDefault(j => j.Nev.Equals(keresettNev, StringComparison.OrdinalIgnoreCase));

            if (talalat != null)
            {
                Console.WriteLine($"\nTalált játékos: {talalat.Nev} ({talalat.Csapat})");
                Console.Write("Biztosan törlöd? ([I]/[N]): ");

                if (Console.ReadKey().Key == ConsoleKey.I)
                {
                    jatekosok.Remove(talalat);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nSikeres törlés!");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Törlés megszakítva.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNincs ilyen nevű játékos a listában!");
                Console.ResetColor();
            }
        }
        static int Szambekeres(string mit, int min, int max)
        {
            int szam = 0;
            Console.Write(mit);
            string k = Console.ReadLine();
            while (!int.TryParse(k, out szam) || min > szam || max < szam)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hiba!");
                Console.ResetColor();
                Console.Write(mit);
                k = Console.ReadLine();
            }
            return szam;
        }
    }
}