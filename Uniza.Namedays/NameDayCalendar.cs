using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniza.Namedays
{
    public record NameDayCalendar : IEnumerable<Nameday>
    {
        public int NameCount { get; }
        public int DayCount { get; }
        public List<Nameday> Namedays { get; set; } = new List<Nameday>();
        
        public DayMonth? this[string name]
        {
            get { return Namedays.Find(n => n.Name == name).DayMonth; }
        }

        public string[] this[DayMonth dayMonth]
        {
            get { return Namedays.FindAll(d => d.DayMonth == dayMonth).Select(n => n.Name).ToArray(); }
        }

        public string[] this[DateOnly date]
        {
            get { return Namedays.FindAll(n => n.DayMonth.ToDateTime().Equals(DateTime.Parse(date.ToString()))).Select(n => n.Name).ToArray(); }
        }

        public string[] this[DateTime date]
        {
            get { return Namedays.Where(d => d.DayMonth.ToDateTime().Equals(date.Date)).Select(n => n.Name).ToArray(); }
        }

        public string[] this[int day, int month]
        {
            get { return Namedays.Where(d => d.DayMonth.Equals(new DayMonth(day, month))).Select(n => n.Name).ToArray(); }
        }

        public IEnumerator<Nameday> GetEnumerator()
        {
            return Namedays.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Namedays.GetEnumerator();
        }

        public IEnumerable<Nameday> GetNamedays()
        {
            return Namedays;
        }

        public IEnumerable<Nameday> GetNamedays(int month)
        {
            return Namedays.Where(n => n.DayMonth.Month == month);
        }

        public IEnumerable<Nameday> GetNamedays(string pattern)
        {
            Regex regex = new Regex(@pattern);

            return Namedays.Where(n => regex.IsMatch(n.Name));
        }

        public void Add(Nameday nameday)
        {
            Namedays.Add(nameday);
        }

        public void Add(int day, int month, string[] names)
        {
            names.ToList().ForEach(n => Namedays.Add(new Nameday(n, new DayMonth(day, month))));
        }

        public void Add(DayMonth dayMonth, string[] names)
        {
            names.ToList().ForEach(n => Namedays.Add(new Nameday(n, dayMonth)));
        }

        public bool Remove(string name)
        {
            return Namedays.RemoveAll(n => n.Name == name) != 0;
        }

        public bool Contains(string name)
        {
            return Namedays.Exists(n => n.Name == name);
        }

        public void Clear()
        {
            Namedays.Clear();
        }

        public void Load(string? path)
        {
            // define the path to the CSV file
            //string path = @"C:\Users\Stano Rehor\Desktop\namedays-sk.csv";

            // open the CSV file for reading
            using (StreamReader reader = new StreamReader(path))
            {
                // read the remaining lines of the file (the data rows)
                while (!reader.EndOfStream)
                {
                    string? dataLine = reader.ReadLine();
                    if (dataLine != null)
                    {
                        string[] fields = dataLine.Split(';');
                        string[] ints = fields[0].Split('.', ' ');

                        DayMonth dayMonth = new DayMonth(int.Parse(ints[0]), int.Parse(ints[2]));
                        Add(dayMonth, fields[1..].Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray());
                    }
                }
            }
        }

        public void Save(FileInfo csvFile)
        {
            //todo test
            // open the CSV file for writing
            using (StreamWriter writer = new StreamWriter(csvFile.FullName))
            {
                // write the data rows
                Namedays.GroupBy(d => d.DayMonth.Day).ToList().ForEach(x =>
                {
                    var names = string.Join(";", x.Select(n => n.Name));
                    writer.Write($"{x.First().DayMonth.Day}.{x.First().DayMonth.Month}.;{names}");
                });
            }
        }
    }
}
