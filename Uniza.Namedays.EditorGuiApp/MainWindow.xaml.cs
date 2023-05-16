using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Uniza.Namedays.EditorGuiApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public NameDayCalendar NameDayCalendar { get; set; } = new NameDayCalendar();

        public MainWindow()
        {
            InitializeComponent();
            NameDayCalendar.Load(@"C:\Users\Stano Rehor\Desktop\namedays-sk.csv");
            SetDateAndNames(DateTime.Now);
            Calendar.SelectedDatesChanged += OnClickDayInCalendar;
            FillComboBox();
            MonthFilter.SelectionChanged += RefreshNames;
            NameFilter.TextChanged += RefreshNames;
            DisableButtons();
            FilteredNames.SelectionChanged += OnClickNameInFilteredNames;
        }

        private void OnClickNameInFilteredNames(object sender, SelectionChangedEventArgs e)
        {
            EnableButtons();
        }

        private void DisableButtons()
        {
            AddButton.IsEnabled = false;
            EditButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;
            ShowOnCalendarButton.IsEnabled = false;
        }

        private void EnableButtons()
        {
            AddButton.IsEnabled = true;
            EditButton.IsEnabled = true;
            RemoveButton.IsEnabled = true;
            ShowOnCalendarButton.IsEnabled = true;
        }

        private void RefreshNames(object sender, EventArgs e)
        {
            FilteredNames.Items.Clear();

            int selectedMonth = MonthFilter.SelectedIndex + 1;
            string regexPattern = NameFilter.Text;

            IEnumerable<Nameday> filteredNamedays = NameDayCalendar.GetNamedays(selectedMonth)
                .Where(nameday => NameDayCalendar.GetNamedays(regexPattern).Contains(nameday));

            var enumerable = filteredNamedays.ToList();
            NamesCount.Content = $"{enumerable.ToList().Count()} / {NameDayCalendar.GetNamedays().Count()}";

            foreach (Nameday nameday in enumerable)
            {
                if (!nameday.Name.Equals("-"))
                {
                    FilteredNames.Items.Add($"{nameday.DayMonth.Day}.{nameday.DayMonth.Month}. {nameday.Name}");
                }
            }
        }

        private void SetDateAndNames(DateTime dateTime)
        {
            DateLabel.Content = dateTime.ToString("dd.MM.yyyy");
            NamedaysTextBox.Text = string.Join("\n", NameDayCalendar[dateTime]);
            Calendar.DisplayDate = dateTime;
            Calendar.SelectedDate = dateTime;
        }

        private void OnClickNew(object sender, RoutedEventArgs routedEventArgs)
        {
            if (NameDayCalendar.GetNamedays().Any())
            {
                MessageBoxResult messageBox = MessageBox.Show("Are you sure you want to create a new calendar?", "New calendar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBox == MessageBoxResult.Yes)
                {
                    NameDayCalendar.Clear();
                }
            }

            RefreshNames(sender, routedEventArgs);
            SetDateAndNames(DateTime.Now);
        }

        private void OnClickOpen(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Nacitaj CSV subor",
                Filter = "CSV Files (*.csv)|*.csv",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                NameDayCalendar.Load(filePath);
            }

            RefreshNames(sender, routedEventArgs);
            SetDateAndNames(DateTime.Now);
        }

        private void OnClickSave(object sender, RoutedEventArgs routedEventArgs)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Uloz CSV subor",
                Filter = "CSV Files (*.csv)|*.csv",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                NameDayCalendar.Save(new FileInfo(filePath));
                MessageBox.Show("Calendar saved successfully.");
            }
        }

        private void OnClickExit(object sender, RoutedEventArgs routedEventArgs)
        {
            Close();
        }

        private void OnClickAbout(object sender, RoutedEventArgs routedEventArgs)
        {
            //todo formatovanie textu
            string title = "About";
            string message = "Namedays\nVersion 1.0\nCopyright free for all\n" +
                             "This application was created by Stanislav Rehor as a project for the subject \"jazyk C# a .NET\" at the University of Žilina.";

            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            SetDateAndNames(DateTime.Now);
        }

        private void OnClickDayInCalendar(object? sender, SelectionChangedEventArgs e)
        {
            SetDateAndNames(Calendar.SelectedDate ?? DateTime.Now);
        }

        private void FillComboBox()
        {
            foreach (SlovakMonth month in Enum.GetValues(typeof(SlovakMonth)))
            {
                string slovakName = SlovakMonthUtility.GetSlovakName(month);
                MonthFilter.Items.Add(slovakName);
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            MonthFilter.SelectedIndex = -1;
            NameFilter.Text = "";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NamedayEdit addNamedayWindow = new NamedayEdit(NameDayCalendar);
            addNamedayWindow.ShowDialog();
            RefreshNames(sender, e);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNameday = FilteredNames.SelectedItem.ToString();
            if (selectedNameday != null)
            {
                string[] parts = selectedNameday.Split('.');
                int day = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                string name = parts[2].Trim();

                NamedayEdit addNamedayWindow = new NamedayEdit(NameDayCalendar, new Nameday(name, new DayMonth(day, month)));
                addNamedayWindow.ShowDialog();
                RefreshNames(sender, e);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNameday = FilteredNames.SelectedItem.ToString();
            if (selectedNameday != null)
            {
                string[] parts = selectedNameday.Split('.');
                string name = parts[2].Trim();

                MessageBoxResult answer = MessageBox.Show($"Do you chces vymazat zaznam \"{FilteredNames.SelectedItem}\" z kalendara?", "Deletni osobu",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    NameDayCalendar.Remove(name);
                }
                RefreshNames(sender, e);
            }
        }

        private void ShowOnCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNameday = FilteredNames.SelectedItem.ToString();
            if (selectedNameday != null)
            {
                string[] parts = selectedNameday.Split('.');
                int day = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);

                SetDateAndNames(new DateTime(DateTime.Now.Year, month, day));
            }
        }
    }
}
