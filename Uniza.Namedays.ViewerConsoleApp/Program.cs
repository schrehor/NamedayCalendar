﻿using System.Threading.Channels;
using Uniza.Namedays;

namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class Program
    {
        public NameDayCalendar? NDCalendar { get; set; }

        static void Main(string[] args)
        {
            Program program = new Program();

            if (args.Length > 0)
            {
                string csvFilePath = args[0];
                program.NDCalendar?.Load(csvFilePath);
            }

            
            while (true)
            {
                Console.WriteLine("Menu");
                Console.WriteLine("1 - Načítať kalendár");
                Console.WriteLine("2 - Zobraziť štatistiku");
                Console.WriteLine("3 - Vyhľadá mená");
                Console.WriteLine("4 - Vyhľadá mená podľa dátumu");
                Console.WriteLine("5 - Zobrazí kalendár mien v mesiaci");
                Console.WriteLine("6 | Escape - koniec");
                Console.WriteLine("Vaša voľba: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        program.NDCalendar = program.LoadCalendar();
                        break;
                    case "2":
                        program.PrintStatistics();
                        break;
                    case "3":
                        //SearchNames();
                        break;
                    case "4":
                        //SearchNamesByDate();
                        break;
                    case "5":
                        program.PrintMonthNamedays();
                        break;
                    case "6":
                        break;
                    default:
                        Console.WriteLine("Neplatná voľba");
                        break;
                }
            }
        }

        private void PrintStatistics()
        {
            
        }

        private NameDayCalendar? LoadCalendar()
        {
            Console.WriteLine("Zadajte cestu ku kalendáru: ");
            var path = Console.ReadLine();

            NameDayCalendar? calendar = new NameDayCalendar();
            if (path != null) calendar.Load(path);

            return calendar;
        }

        void PrintMonthNamedays()
        {
            ConsoleKeyInfo choice;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            do
            {
                if (NDCalendar != null)
                    NDCalendar.Where(m => m.DayMonth.Month == month)
                        .GroupBy(d => d.DayMonth.Day)
                        .ToList().ForEach(n =>
                        {
                            var day = n.First().DayMonth.ToDateTime(year);
                            if (day == DateTime.Today)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else if (day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                            }

                            var names = string.Join(", ", n.Select(x => x.Name));

                            Console.WriteLine($"{n.Key}. {names}");
                        });
                Console.WriteLine($"Šípka doľava/doprava - mesiac dozadu/dopredu");
                Console.WriteLine($"Šípka dole/hore - rok dozadu/dopredu");
                Console.WriteLine($"Klávesa Home alebo D - aktuálny deň");
                choice = Console.ReadKey();
                switch (choice.Key)
                {
                    case ConsoleKey.LeftArrow:
                        month--;
                        
                        break;
                    case ConsoleKey.RightArrow:
                        month++;
                        break;
                    case ConsoleKey.UpArrow:
                        year++;
                        break;
                    case ConsoleKey.DownArrow:
                        year--;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.Home:
                        year = DateTime.Now.Year;
                        month = DateTime.Now.Month;
                        break;
                    default:
                        Console.WriteLine("Zly vstup bracho");
                        break;
                }
            } while (choice.Key != ConsoleKey.Enter);
        }
    }
}