using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;

namespace Nyilvántartó
{
    public class UI
    {
        public static string MenuValaszto()
        {
            AnsiConsole.Write(new Rule("[yellow]KEZELŐPULT[/]").RuleStyle("grey").LeftJustified());

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.DeepSkyBlue1))
                    .AddChoices(new[] { "Szűrés", "Felvétel", "Módosítás", "Törlés", "Fájl", "Kilépés" }));
        }
        public static void ListaMegjelenitese(List<Jatekos> jatekosLista)
        {
            if (jatekosLista.Count == 0)
            {
                var hibaPanel = new Panel("[red]Nincs adat a rendszerben![/]").BorderColor(Color.Red);
                AnsiConsole.Write(new Align(hibaPanel, HorizontalAlignment.Center));
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Grey35)
                .Title("[bold white on blue] 🤾 KÉZILABDA JÁTÉKOS NYILVÁNTARTÁS [/]")
                .Expand();

            table.AddColumn(new TableColumn("[grey]#[/]")); 
            table.AddColumn(new TableColumn("[bold yellow]Név[/]"));
            table.AddColumn(new TableColumn("[grey]Kor[/]").Centered());
            table.AddColumn(new TableColumn("[blue]Csapat[/]"));
            table.AddColumn(new TableColumn("[white]Meccs (GY/D/V)[/]").Centered());
            table.AddColumn(new TableColumn("[green]Gól (7m)[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Sárga[/]").Centered());
            table.AddColumn(new TableColumn("[purple]2 min[/]").Centered());
            table.AddColumn(new TableColumn("[red]Piros[/]").Centered());

            int maxGol = jatekosLista.Max(j => j.Gol);

            int sorszam = 1;

            foreach (var j in jatekosLista)
            {
                string nevStyle = j.Gol == maxGol ? $"[bold gold1]{j.Nev} 👑[/]" : j.Nev;
                string merleg = $"{j.Meccs} [grey]({j.Gyozelem}/{j.Dontetlen}/{j.Vereseg})[/]";
                string golok = $"[bold green]{j.Gol}[/] [grey]({j.Bunteto})[/]";
                string sarga = j.SargaLap > 0 ? $"[yellow]{j.SargaLap}[/]" : "[grey]-[/]";
                string kiallitas = j.Kiallitas > 0 ? $"[purple]{j.Kiallitas}[/]" : "[grey]-[/]";
                string piros = j.PirosLap > 0 ? $"[white on red] {j.PirosLap} [/]" : "[grey]-[/]";

                table.AddRow(
                    $"[grey]{sorszam}.[/]", 
                    nevStyle,
                    j.Eletkor.ToString(),
                    j.Csapat,
                    merleg,
                    golok,
                    sarga,
                    kiallitas,
                    piros
                );

                sorszam++; 
            }

            AnsiConsole.Write(new Align(table, HorizontalAlignment.Center));

            var osszGol = jatekosLista.Sum(x => x.Gol);
            var osszMeccs = jatekosLista.Sum(x => x.Meccs);

            var stats = new Columns(
                new Panel($"[bold green]Összes gól:[/] {osszGol}").BorderColor(Color.Green).Expand(),
                new Panel($"[bold blue]Átlag életkor:[/] {Math.Round(jatekosLista.Average(x => x.Eletkor), 1)} év").BorderColor(Color.Blue).Expand(),
                new Panel($"[bold yellow]Összes meccs:[/] {osszMeccs}").BorderColor(Color.Yellow).Expand()
            );

            AnsiConsole.Write(new Align(stats, HorizontalAlignment.Center));
            Console.WriteLine();

        }
        public static void TopGollovokChart(List<Jatekos> lista)
        {
            if (lista.Count == 0) return;

            var top5 = lista.OrderByDescending(j => j.Gol).Take(5).ToList();

            var chart = new BarChart()
                .Width(60)
                .Label("[green bold]TOP 5 GÓLLÖVŐ (ÖSSZES GÓL)[/]")
                .CenterLabel();

            foreach (var j in top5)
            {
                chart.AddItem(j.Nev, j.Gol, Color.Green);
            }

            AnsiConsole.Write(new Align(chart, HorizontalAlignment.Center));
            AnsiConsole.WriteLine();
        }
        public static void HibaUzenet(string uzenet) => AnsiConsole.MarkupLine($"[bold white on red] HIBA [/] [red]{uzenet}[/]");
        public static void SikeresMuvelet(string uzenet) => AnsiConsole.MarkupLine($"[bold black on green] OK [/] [green]{uzenet}[/]");
        public static void BekeresFejlec(string lepes, string info)
        {
            Console.Clear();
            var panel = new Panel(new Align(new Text(info, new Style(Color.Yellow)), HorizontalAlignment.Center))
            {
                Header = new PanelHeader($"[bold blue] ÚJ JÁTÉKOS: {lepes} [/]"),
                Border = BoxBorder.Rounded,
                Padding = new Padding(1, 1, 1, 1)
            };
            AnsiConsole.Write(panel);
            AnsiConsole.WriteLine();
        }
        public static string SzuresValaszto()
        {
            //AnsiConsole.Write(new Rule("[yellow]SZŰRÉS[/]").RuleStyle("grey").LeftJustified());
            AnsiConsole.WriteLine();
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
                    .AddChoices(new[] { "Csapat szerint", "Játékos szerint", "Top gól lövők szerint", "<- Vissza" }));
        }
        public static string FajlMenuValaszto()
        {
            Console.Clear();
            AnsiConsole.Write(new Rule("[yellow]FÁJL MŰVELETEK[/]").RuleStyle("grey"));

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
                    .AddChoices(new[] { "Új adatbázis", "Betöltés fájlból", "Mentés fájlba", "<- Vissza" }));
        }
    }
}