using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
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
        }

        private void SetDateAndNames(DateTime dateTime)
        {
            DateLabel.Content = dateTime.ToString("dd.MM.yyyy");
            NamedaysTextBox.Text = string.Join("\n", NameDayCalendar[dateTime]);
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
    }
}
