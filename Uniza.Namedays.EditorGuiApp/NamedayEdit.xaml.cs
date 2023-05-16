using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for NamedayEdit.xaml
    /// </summary>
    public partial class NamedayEdit : Window
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
