namespace Uniza.Namedays
{
    public record struct DayMonth(int Day, int Month)
    {
        public int Day { get; init; } = Day;
        public int Month { get; init; } = Month;

        public DayMonth() : this(0, 0)
        {
        }

        public DateTime ToDateTime()
        {
            return new DateTime(DateTime.Now.Year, Month, Day);
        }
    }
}