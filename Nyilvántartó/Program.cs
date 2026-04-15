using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Nyilvántartó
{
    class Program
    {
        static List<Jatekos> jatekosok = new List<Jatekos>();
        static List<Jatekos> szures = new List<Jatekos>();


        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            InitAdatok();

            bool futAProgram = true;
            while (futAProgram)
            {
                string valasztas = UI.MenuValaszto();

                switch (valasztas)
                {
                    case "Fájl":
                        string fajlMuvelet = UI.FajlMenuValaszto();
                        FajlKezeles(fajlMuvelet);
                        Visszaleptetes();
                        break;
                    case "Megtekintés":
                        Console.Clear();
                        UI.ListaMegjelenitese(jatekosok);
                        string szuresMuvelet = UI.SzuresValaszto();
                        SzuresKezeles(szuresMuvelet);
                        break;
                    case "Felvétel":
                        JatekosFelvetel();
                        break;
                    case "Módosítás":
                        JatekosModositas();
                        Visszaleptetes();
                        break;
                    case "Törlés":
                        JatekosTorol();
                        break;
                    case "Kilépés":
                        futAProgram = false;
                        break;
                }
            }
        }

        static void JatekosFelvetel()
        {
            string nev = ""; int kor = 0; string csapat = "";
            int meccs = 0; int gyoz = 0; int dont = 0; int ver = 0;
            int gol = 0; int bunteto = 0; int sarga = 0; int kiall = 0; int piros = 0;

            void RenderAdatlap(string aktualisMezo)
            {
                Console.Clear();
                var table = new Table().Border(TableBorder.Rounded).Expand().BorderColor(Color.DeepSkyBlue1);
                table.Title("[bold white on blue] 📝 ÚJ JÁTÉKOS ADATLAPJA [/]");
                table.AddColumn(new TableColumn("[grey]Mező[/]").Width(20));
                table.AddColumn(new TableColumn("[bold white]Érték[/]"));

                table.AddRow("Név:", string.IsNullOrEmpty(nev) ? "[grey]...[/]" : $"[bold yellow]{nev}[/]");
                table.AddRow("Kor:", kor == 0 ? "[grey]-[/]" : kor.ToString());
                table.AddRow("Csapat:", string.IsNullOrEmpty(csapat) ? "[grey]-[/]" : csapat);
                table.AddRow("Mérleg (GY/D/V):", meccs == 0 ? "[grey]-[/]" : $"{meccs} ({gyoz}/{dont}/{ver})");
                table.AddRow("Gólok (7m):", gol == 0 ? "[grey]-[/]" : $"{gol} ({bunteto})");
                table.AddRow("Büntetések (S/2/P):", $"[yellow]{sarga}[/] / [blue]{kiall}[/] / [red]{piros}[/]");

                AnsiConsole.Write(table);
                AnsiConsole.Write(new Rule($"[yellow]Kérjük, add meg: {aktualisMezo}[/]").LeftJustified());
                AnsiConsole.WriteLine();
            }


            RenderAdatlap("Játékos neve");
            nev = SzovegBekeres("Játékos neve", false);

            RenderAdatlap("Életkor");
            kor = Szambekeres("Életkor", 15, 60);

            RenderAdatlap("Csapat");
            csapat = SzovegBekeres("Csapat neve", true);

            RenderAdatlap("Összes mérkőzés");
            meccs = Szambekeres("Meccsszám", 0, 999);

            if (meccs > 0)
            {
                RenderAdatlap("Győzelmek száma");
                gyoz = Szambekeres("Győzelmek", 0, meccs);

                RenderAdatlap("Döntetlenek száma");
                dont = Szambekeres("Döntetlenek", 0, meccs - gyoz);

                ver = meccs - (gyoz + dont);
            }

            RenderAdatlap("Gólok száma");
            gol = Szambekeres("Összes gól", 0, 9999);

            RenderAdatlap("Hétméteresek száma");
            bunteto = Szambekeres("Büntetők", 0, gol);

            RenderAdatlap("Sárga lapok");
            sarga = Szambekeres("Sárga lap", 0, meccs);

            RenderAdatlap("Kiállítások");
            kiall = Szambekeres("2 percesek", 0, meccs * 3);

            RenderAdatlap("Piros lapok");
            piros = Szambekeres("Piros lap", 0, meccs);

            RenderAdatlap("MENTÉS...");
            AnsiConsole.Status().Start("Adatok rögzítése...", ctx =>
            {
                System.Threading.Thread.Sleep(1000);
                jatekosok.Add(new Jatekos(nev, kor, csapat, meccs, gyoz, dont, ver, gol, bunteto, sarga, kiall, piros));
            });

            UI.SikeresMuvelet("A játékos adatai elmentve!");
            Visszaleptetes();
        }
        static Jatekos JatekosKereso(string bemenet)
        {
            if (int.TryParse(bemenet, out int index))
            {
                int valodiIndex = index - 1;
                if (valodiIndex >= 0 && valodiIndex < jatekosok.Count)
                {
                    return jatekosok[valodiIndex];
                }
            }

            return jatekosok.FirstOrDefault(j => j.Nev.Equals(bemenet, StringComparison.OrdinalIgnoreCase));
        }
        static void JatekosModositas()
        {
            Console.Clear();
            UI.ListaMegjelenitese(jatekosok);

            var bemenet = AnsiConsole.Ask<string>("Módosítani kívánt játékos [bold yellow]neve[/] vagy [bold yellow]sorszáma[/]: ");

            var jatekos = JatekosKereso(bemenet);

            if (jatekos == null)
            {
                UI.HibaUzenet("Nincs ilyen játékos vagy érvénytelen sorszám!");
                Visszaleptetes();
                return;
            }

            var mit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Mit szeretnél módosítani [bold cyan]{jatekos.Nev}[/] adatain?")
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.DeepSkyBlue1))
                    .AddChoices(new[] { "Csapat", "Gólok", "Büntetések", "Vissza" }));

            if (mit == "Vissza") return;

            switch (mit)
            {
                case "Csapat":
                    jatekos.Csapat = SzovegBekeres("Új csapat neve", true);
                    break;
                case "Gólok":
                    jatekos.Gol = Szambekeres("Új gólszám", 0, 9999);
                    jatekos.Bunteto = Szambekeres("Ebből büntető", 0, jatekos.Gol);
                    break;
                case "Büntetések":
                    jatekos.SargaLap = Szambekeres("Sárga lapok", 0, jatekos.Meccs);
                    jatekos.Kiallitas = Szambekeres("2 perces kiállítások", 0, 50);
                    jatekos.PirosLap = Szambekeres("Piros lapok", 0, 10);
                    break;
            }

            UI.SikeresMuvelet("Az adatok sikeresen frissítve!");
        }
        static void JatekosTorol()
        {
            Console.Clear();
            UI.ListaMegjelenitese(jatekosok);

            string bemenet = AnsiConsole.Ask<string>("Törölni kívánt játékos [bold yellow]neve[/] vagy [bold yellow]sorszáma[/]: ");

            var talalat = JatekosKereso(bemenet);

            if (talalat != null)
            {
                if (AnsiConsole.Confirm($"[red]Biztosan törlöd[/] [white bold]{talalat.Nev}[/] adatait?"))
                {
                    jatekosok.Remove(talalat);
                    UI.SikeresMuvelet("Törölve.");
                }
            }
            else
            {
                UI.HibaUzenet("Nincs ilyen játékos vagy érvénytelen sorszám.");
            }
            Visszaleptetes();
        }
        static int Szambekeres(string cimke, int min, int max)
        {
            int startLine = Console.CursorTop;

            while (true)
            {
                Console.SetCursorPosition(0, startLine);
                Console.Write(new string(' ', Console.WindowWidth * 2));
                Console.SetCursorPosition(0, startLine);

                AnsiConsole.Markup($"[white]{cimke} ({min}-{max}): [/]");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int szam) && szam >= min && szam <= max)
                {
                    return szam;
                }

                Console.SetCursorPosition(0, startLine + 1);
                AnsiConsole.Markup($"[bold red]! Érvénytelen: {min} és {max} közötti számot adj meg![/]");
                System.Threading.Thread.Sleep(1000);
            }
        }
        static string SzovegBekeres(string cimke, bool szamotTartalmazhat = true)
        {
            int startLine = Console.CursorTop;

            while (true)
            {
                Console.SetCursorPosition(0, startLine);
                Console.Write(new string(' ', Console.WindowWidth * 2));
                Console.SetCursorPosition(0, startLine);

                AnsiConsole.Markup($"[white]{cimke}: [/]");
                string input = Console.ReadLine()?.Trim();

                // 3. Validáció
                bool hiba = false;
                string hibaUzenet = "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    hiba = true;
                    hibaUzenet = "A mező nem maradhat üres!";
                }
                else if (!szamotTartalmazhat && input.Any(char.IsDigit))
                {
                    hiba = true;
                    hibaUzenet = "A név nem tartalmazhat számot!";
                }

                if (!hiba) return input;

                Console.SetCursorPosition(0, startLine + 1);
                AnsiConsole.Markup($"[bold red]! {hibaUzenet}[/]");
                System.Threading.Thread.Sleep(1000);
            }
        }
        static void Visszaleptetes()
        {
            AnsiConsole.Markup("\n[grey]Nyomj ENTER-t a visszalépéshez...[/]");
            Console.ReadLine();
        }
        static void InitAdatok()
        {
            jatekosok.Add(new Jatekos("Bánhidi Bence", 29, "Pick Szeged", 24, 17, 2, 5, 85, 0, 4, 12, 1));
            jatekosok.Add(new Jatekos("Lékai Máté", 35, "Ferencváros", 26, 15, 3, 8, 92, 15, 2, 3, 0));
            jatekosok.Add(new Jatekos("Klujber Katrin", 24, "FTC-Rail Cargo", 26, 20, 2, 4, 145, 42, 3, 5, 0));
        }
        static void SzuresKezeles(string muvelet)
        {
            switch (muvelet)
            {
                case "Csapat szerint":
                    CsapatSzures();
                    break;
                case "Játékos szerintl":
                    JatekosSzures();
                    break;
                case "Top gól lövők szerint":
                    UI.TopGollovokChart(jatekosok);
                    break;
                case "<- Vissza":
                    break;
            }
        }
        static void CsapatSzures()
        {
            szures.Clear();
            string input = AnsiConsole.Ask<string>("Szűrés a kívánt [bold yellow]csapatra[/]: ");
            foreach (var jatekos in jatekosok)
            {
                if (jatekos.Csapat == input)
                {
                    szures.Add(jatekos);
                }
            }
            UI.ListaMegjelenitese(szures);
        }
        static void JatekosSzures()
        {
            szures.Clear();
            string input = AnsiConsole.Ask<string>("Szűrés a kívánt [bold yellow]játékosra[/]: ");
            foreach (var jatekos in jatekosok)
            {
                if (jatekos.Nev == input)
                {
                    szures.Add(jatekos);
                }
            }
            UI.ListaMegjelenitese(szures);
        }
        static void FajlKezeles(string muvelet)
        {
            switch (muvelet)
            {
                case "Új adatbázis":
                    if (AnsiConsole.Confirm("[red]Biztosan törlöd a jelenlegi listát?[/]"))
                    {
                        jatekosok.Clear();
                        UI.SikeresMuvelet("Adatbázis kiürítve.");
                    }
                    break;
                case "Betöltés fájlból":
                    BetöltésFájlból();
                    break;
                case "Mentés fájlba":
                    MentésFájlba();
                    break;
                case "<- Vissza":
                    break;
            }
        }
        static void MentésFájlba()
        {
            try
            {
                string mentesIde = AnsiConsole.Ask<string>("Add meg a [bold yellow]mentés[/] nevét (kiterjesztés nélkül): ");
                List<string> sorok = new List<string>();
                string fejlec = "Nev;Eletkor;Csapat;Meccs;Gyozelem;Dontetlen;Vereseg;Gol;Bunteto;SargaLap;Kiallitas;PirosLap";
                sorok.Add(fejlec);

                foreach (var j in jatekosok)
                {
                    string sor = $"{j.Nev};{j.Eletkor};{j.Csapat};{j.Meccs};{j.Gyozelem};{j.Dontetlen};" +
                                    $"{j.Vereseg};{j.Gol};{j.Bunteto};{j.SargaLap};{j.Kiallitas};{j.PirosLap}";

                    sorok.Add(sor);
                }

                File.WriteAllLines($"{mentesIde}.csv", sorok, System.Text.Encoding.UTF8);

                Console.WriteLine($"\nAz adatok sikeresen elmentve a {mentesIde}.csv fájlba.");
            }
            catch (Exception hiba)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nHiba történt a mentés során: " + hiba.Message);
                Console.ResetColor();
                Console.ReadKey(true);
            }

        }

        static void BetöltésFájlból()
        {
            try
            {
                string fajlNev = AnsiConsole.Ask<string>("Add meg a betöltendő fájl nevét (kiterjesztés nélkül): ");
                string teljesPath = $"{fajlNev}.csv";

                if (!File.Exists(teljesPath))
                {
                    UI.HibaUzenet($"A fájl nem található: {teljesPath}");
                    return;
                }

                string[] sorok = File.ReadAllLines(teljesPath, System.Text.Encoding.UTF8);

                // Ha üres a fájl vagy csak fejléc van
                if (sorok.Length <= 1)
                {
                    UI.HibaUzenet("A fájl nem tartalmaz adatokat!");
                    return;
                }

                AnsiConsole.Status().Start("Adatok betöltése...", ctx =>
                {
                    // Töröljük a jelenlegi listát, ha friss betöltést akarunk (opcionális)
                    // jatekosok.Clear(); 

                    // Az első sort (fejlécet) kihagyjuk
                    for (int i = 1; i < sorok.Length; i++)
                    {
                        string[] adatok = sorok[i].Split(';');
                        if (adatok.Length == 12)
                        {
                            jatekosok.Add(new Jatekos(
                                adatok[0],                          // Név
                                int.Parse(adatok[1]),               // Kor
                                adatok[2],                          // Csapat
                                int.Parse(adatok[3]),               // Meccs
                                int.Parse(adatok[4]),               // Győzelem
                                int.Parse(adatok[5]),               // Döntetlen
                                int.Parse(adatok[6]),               // Vereség
                                int.Parse(adatok[7]),               // Gól
                                int.Parse(adatok[8]),               // Büntető
                                int.Parse(adatok[9]),               // Sárga
                                int.Parse(adatok[10]),              // Kiállítás
                                int.Parse(adatok[11])               // Piros
                            ));
                        }
                    }
                    System.Threading.Thread.Sleep(800);
                });

                UI.SikeresMuvelet("Az adatok betöltése sikeresen megtörtént!");
            }
            catch (Exception hiba)
            {
                UI.HibaUzenet($"Hiba történt a betöltés során: {hiba.Message}");
            }
        }

    }
}
