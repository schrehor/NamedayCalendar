namespace Uniza.Namedays
{
    public record struct Nameday(string Name, DayMonth DayMonth)
    {
        public string Name { get; init; } = Name;
        public DayMonth DayMonth { get; init; } = DayMonth;

        public Nameday() : this(null, new DayMonth())
        {
        }
    }
}
