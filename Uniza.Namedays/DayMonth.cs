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
            //todo opravit
            if (DateTime.Now.Year % 4 != 0 && Month == 2 && Day == 29)
            {
                return default;
            }
            else
            {
                return new DateTime(DateTime.Now.Year, Month, Day);
            }
        }
        
        public DateTime ToDateTime(int year)
        {
            //todo opravit
            if (DateTime.Now.Year % 4 != 0 && Month == 2 && Day == 29)
            {
                return default;
            }
            else
            {
                return new DateTime(year, Month, Day);
            }
        }
    }
}