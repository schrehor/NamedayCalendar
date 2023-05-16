using System;
using System.Linq;
using System.Windows;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for NamedayEdit.xaml
    /// </summary>
    public partial class NamedayEdit
    {
        public Nameday SingleNameday { get; set; }
        public NameDayCalendar NameDayCalendar { get; set; }

        public NamedayEdit(NameDayCalendar nameDayCalendar, Nameday singleNameday = default)
        {
            InitializeComponent();
            SingleNameday = singleNameday;
            NameDayCalendar = nameDayCalendar;
            DatePicker.SelectedDate = singleNameday == default ? DateTime.Now : singleNameday.DayMonth.ToDateTime();
            NameBox.Text = singleNameday == default ? "" : singleNameday.Name;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text;
            var date = DatePicker.SelectedDate;

            if (!SingleNameday.Equals(default))
            {
                NameDayCalendar.Namedays.Remove(SingleNameday);
            }
            if (date != null)
                NameDayCalendar.Add(new Nameday(name, new DayMonth(date.Value.Day, date.Value.Month)));

            NameDayCalendar.Namedays = NameDayCalendar.OrderBy(n => n.DayMonth.Day).ThenBy(n => n.DayMonth.Month)
                .ThenBy(n => n.Name).ToList();

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
