using System;

namespace Nyilvántartó
{
    class Program
    {
        static List<Jatekos> jatekosok = new List<Jatekos>();
        static void Main(string[] args)
        {
            Console.WindowWidth = 120;
            Console.CursorVisible = false;

            // --- Teszt adatok feltöltése ---
            jatekosok.Add(new Jatekos("Szoboszlai Dominik", 23, "Liverpool FC", 25, "Győztes", 5, 2, 3, 0, 0));
            jatekosok.Add(new Jatekos("Sallai Roland", 26, "SC Freiburg", 20, "Döntetlen", 3, 0, 4, 0, 0));
            jatekosok.Add(new Jatekos("Gulácsi Péter", 33, "RB Leipzig", 30, "Vesztes", 0, 0, 1, 0, 0));

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
                    Console.WriteLine("═══ Új játékos felvétele ═══");
                    // Ide jön az adatbekérés
                    break;
                case 2:
                    Console.WriteLine("═══ Játékos módosítása ═══");
                    // Ide jön a módosítás
                    break;
                case 3:
                    Console.WriteLine("═══ Játékos törlése ═══");
                    // Ide jön a törlés
                    break;
                case 4:
                    // Kilépés
                    return false;
            }

            Console.WriteLine("\nNyomj meg egy gombot a visszalépéshez...");
            Console.ReadKey(true);
            return true;
        }
    }
}