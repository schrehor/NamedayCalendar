namespace Uniza.Namedays
{
    public record struct DayMonth(int Day, int Month)
    {
        /// <summary>
        /// Integrova reprezentacia dňa.
        /// </summary>
        public int Day { get; init; } = Day;
        
        /// <summary>
        /// Integrova reprezentacia mesiaca.
        /// </summary>
        public int Month { get; init; } = Month;

        /// <summary>
        /// Inicializuje prázdnu triedu.
        /// </summary>
        public DayMonth() : this(0, 0)
        {
        }

        /// <summary>
        /// Z dňa a mesiaca urobi Datetime s aktuálnym rokom.
        /// </summary>
        /// <returns>Objekt typu <see cref="DateTime"/> s daným dňom a mesiacom.</returns>
        public DateTime ToDateTime()
        {
            // Na to ze existuje DateTime.IsLeapYear() som prisiel az po tom, čo som to urobil po svojom
            if (DateTime.Now.Year % 4 != 0 && Month == 2 && Day == 29)
            {
                return default;
            }
            else
            {
                return new DateTime(DateTime.Now.Year, Month, Day);
            }
        }

        /// <summary>
        /// Z dňa a mesiaca urobi Datetime so zadaným rokom.
        /// </summary>
        /// <param name="year">Rok, pre ktorý sa má vytvoriť objekt typu DateTime.</param>
        /// <returns>Objekt typu DateTime s daným dňom, mesiacom a rokom.</returns>
        public DateTime ToDateTime(int year)
        {
            if (year % 4 != 0 && Month == 2 && Day == 29)
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