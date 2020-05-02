using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebAPI_Client_Assistant.DataProviders;
using WebAPI_Client_Assistant.Models;

namespace WebAPI_Client_Assistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IList<Person> people;
        

        public MainWindow(string Text)
        {
            
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            mainText.Content = "Welcome Back " + Text + "!";
            UpdatePeople();
        }

        private void todayPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPerson = todayPeople.SelectedItem as Person;
            if (selectedPerson != null)
            {
                var window = new PersonWindow(selectedPerson);
                if (window.ShowDialog() ?? false)
                {
                    UpdatePeople();
                }
                todayPeople.UnselectAll();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new PersonWindow(null);
            if (window.ShowDialog() ?? false)
            {
                UpdatePeople();
            }
        }

        private void UpdatePeople()
        {
            people = PersonDataProvider.GetPeople();
            DateTime ActualTime = DateTime.Now;
            dateText.Content = "Today's date is: " + ActualTime.ToShortDateString().ToString();
            IList<Person> SortedList = people.OrderBy(o => o.DateOfArrival).ToList();
            IList<Person> NotDiagnosed = new List<Person>();
            foreach(Person p in SortedList)
            {
                if(p.Diagnosis == null)
                {
                    NotDiagnosed.Add(p);
                }
            }
            todayPeople.ItemsSource = NotDiagnosed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdatePeople();
        }
    }
}
