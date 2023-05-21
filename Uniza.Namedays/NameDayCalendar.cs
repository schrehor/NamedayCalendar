using System.Collections;
using System.Text.RegularExpressions;
using System.Text;

namespace Uniza.Namedays
{
    public record NameDayCalendar : IEnumerable<Nameday>
    {
        /// <summary>
        /// Počet mien v jendom dni.
        /// </summary>
        public int NameCount { get; set; }

        /// <summary>
        /// Počet dní v kalendári.
        /// </summary>
        public int DayCount { get; set; }

        /// <summary>
        /// Zoznam mien s ich dátumom.
        /// </summary>
        public List<Nameday> Namedays { get; set; } = new List<Nameday>();
        
        /// <summary>
        /// Vráti dátum oslavy podľa mena.
        /// </summary>
        public DayMonth? this[string name]
        {
            get { return Namedays.Find(n => n.Name == name).DayMonth; }
        }

        /// <summary>
        /// Vráti pole mien podľa dátumu.
        /// </summary>
        public string[] this[DayMonth dayMonth]
        {
            get { return Namedays.Where(d => d.DayMonth == dayMonth).Select(n => n.Name).ToArray(); }
        }

        /// <summary>
        /// Vráti pole mien podľa dátumu.
        /// </summary>
        public string[] this[DateOnly date]
        {
            get { return Namedays.Where(n => n.DayMonth.Month == date.Month && n.DayMonth.Day == date.Day).Select(n => n.Name).ToArray(); }
        }

        /// <summary>
        /// Vráti pole mien podľa dátumu.
        /// </summary>
        public string[] this[DateTime date]
        {
            get { return Namedays.Where(d => d.DayMonth.Month == date.Month && d.DayMonth.Day == date.Day).Select(n => n.Name).ToArray(); }
        }

        /// <summary>
        /// Vráti pole mien podľa dátumu.
        /// </summary>
        public string[] this[int day, int month]
        {
            get { return Namedays.Where(d => d.DayMonth.Equals(new DayMonth(day, month))).Select(n => n.Name).ToArray(); }
        }

        /// <summary>
        /// Vráti enumerátor, ktorý iteruje cez kolekciu.
        /// </summary>
        public IEnumerator<Nameday> GetEnumerator()
        {
            return Namedays.GetEnumerator();
        }

        /// <summary>
        /// Vráti enumerátor, ktorý iteruje cez kolekciu.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Namedays.GetEnumerator();
        }

        /// <summary>
        /// Vráti enumerátor, ktorý iteruje cez kolekciu.
        /// </summary>
        public IEnumerable<Nameday> GetNamedays()
        {
            return Namedays;
        }

        /// <summary>
        /// Vráti enumerátor, ktorý iteruje cez kolekciu podľa mesiaca.
        /// </summary>
        public IEnumerable<Nameday> GetNamedays(int month)
        {
            return Namedays.Where(n => n.DayMonth.Month == month);
        }

        /// <summary>
        /// Vráti enumerátor, ktorý iteruje cez kolekciu podľa určitého regex patternu.
        /// </summary>
        public IEnumerable<Nameday> GetNamedays(string pattern)
        {
            Regex regex = new Regex(@pattern);

            return Namedays.Where(n => regex.IsMatch(n.Name));
        }

        /// <summary>
        /// Pridá Nameday do kalendára.
        /// </summary>
        public void Add(Nameday nameday)
        {
            Namedays.Add(nameday);
        }

        /// <summary>
        /// Pridá Nameday do kalendára podľa dňa, mesiaca a zozanmu mien.
        /// </summary>
        public void Add(int day, int month, string[] names)
        {
            names.ToList().ForEach(n => Namedays.Add(new Nameday(n, new DayMonth(day, month))));
        }

        /// <summary>
        /// Pridá Nameday do kalendára podľa DayMonth a zozanmu mien.
        /// </summary>
        public void Add(DayMonth dayMonth, string[] names)
        {
            names.ToList().ForEach(n => Namedays.Add(new Nameday(n, dayMonth)));
        }

        /// <summary>
        /// Odobere meno z kalendára.
        /// </summary>
        public bool Remove(string name)
        {
            return Namedays.RemoveAll(n => n.Name == name) != 0;
        }

        /// <summary>
        /// Zistí či sa prvok nachádza v kolekcii.
        /// </summary>
        public bool Contains(string name)
        {
            return Namedays.Exists(n => n.Name == name);
        }

        /// <summary>
        /// Vymaže všetky Nameday záznamy z kalendára.
        /// </summary>
        public void Clear()
        {
            Namedays.Clear();
        }

        /// <summary>
        /// Načíta kalendár z CSV súboru.
        /// </summary>
        public void Load(string path)
        {
            if (path == "")
            {
                path = @"..\..\..\..\namedays-sk.csv";
            }

            Clear();
            
            
            using StreamReader reader = new StreamReader(path);
            
            while (!reader.EndOfStream)
            {
                string? dataLine = reader.ReadLine();

                if (dataLine == null) 
                    continue;
                string[] fields = dataLine.Split(';');
                string[] ints = fields[0].Split('.', ' ');

                DayMonth dayMonth = new DayMonth(int.Parse(ints[0]), int.Parse(ints[2]));
                Add(dayMonth, fields[1..].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray());

                if (fields[1].Equals("-")) 
                    continue;
                DayCount++;
                NameCount += fields[1..].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).Count();
            }
        }

        /// <summary>
        /// Uloží dáta do CSV súboru.
        /// </summary>
        public void Save(FileInfo csvFile)
        {
            using FileStream fileStream = csvFile.Create();
            using StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

            Namedays.GroupBy(d => new { d.DayMonth.Day, d.DayMonth.Month }).ToList().ForEach(x =>
            {
                var names = string.Join(";", x.Select(n => n.Name));
                writer.WriteLine($"{x.First().DayMonth.Day}.{x.First().DayMonth.Month}.;{names}");
            });
        }
    }
}
