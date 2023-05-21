namespace Uniza.Namedays
{
    public record struct Nameday(string Name, DayMonth DayMonth)
    {
        /// <summary>
        /// Meno oslávenca.
        /// </summary>
        public string Name { get; init; } = Name;
        
        /// <summary>
        /// Dátum oslavy.
        /// </summary>
        public DayMonth DayMonth { get; init; } = DayMonth;

        /// <summary>
        /// Prázdny konštruktor.
        /// </summary>
        public Nameday() : this("", new DayMonth())
        {
        }
    }
}
