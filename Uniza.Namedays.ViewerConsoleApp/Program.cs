using Uniza.Namedays;

namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class Program
    {
        public NameDayCalendar NDCalendar { get; set; }

        static void Main(string[] args)
        {
            Program program = new Program();

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
                    LoadCalendar();
                    break;
                case "2":
                    //PrintStatistics();
                    break;
                case "3":
                   // SearchNames();
                    break;
                case "4":
                    //SearchNamesByDate();
                    break;
                case "5":
                    //PrintCalendar();
                    break;
                case "6":
                    break;
                default:
                    Console.WriteLine("Neplatná voľba");
                    break;
            }
        }

        private static NameDayCalendar? LoadCalendar()
        {
            string? path;

            do
            {
                Console.WriteLine("Zadajte cestu k súboru: ");
                path = Console.ReadLine();

                if (path == "")
                {
                    return null;
                }

                if (!File.Exists(path))
                {
                    Console.WriteLine("Zadaná cesta nie je platná.");
                }
                else if (Path.GetExtension(path).ToLower() != ".csv")
                {
                    Console.WriteLine("Zadaný súbor nie je vo formáte CSV.");
                }
                else
                {
                    break;
                }
            } while (true);

            NameDayCalendar? calendar = new NameDayCalendar();
            calendar.Load(path);
           
            return calendar;
        }

        void PrintCalendar()
        {
            // NameDayCalendar na = new NameDayCalendar();
            // na.Load();
            //
            // foreach (Nameday yaho in na)
            // {
            //     Console.WriteLine($"{yaho.DayMonth.Day}.{yaho.DayMonth.Month}. {yaho.Name}");
            // }
        }
    }
}