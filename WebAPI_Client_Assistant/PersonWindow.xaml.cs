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
using WebAPI_Client_Assistant.DataProviders;
using WebAPI_Client_Assistant.Models;

namespace WebAPI_Client_Assistant
{
    /// <summary>
    /// Interaction logic for PersonWindow.xaml
    /// </summary>
    public partial class PersonWindow : Window
    {
        private Person _person;
        public PersonWindow(Person person)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (person != null)
            {
                _person = person;
                FirstNameTextBox.IsReadOnly = true;
                FirstNameTextBox.Text = person.FirstName;
                LastNameTextBox.IsReadOnly = true;
                LastNameTextBox.Text = person.LastName;
                AddressTextBox.IsReadOnly = true;
                AddressTextBox.Text = person.Address;
                SocialSecurityNumberTextBox.IsReadOnly = true;
                SocialSecurityNumberTextBox.Text = person.SocialSecurityNumber;
                ComplaintTextBox.IsReadOnly = true;
                ComplaintTextBox.Text = person.Complaint;
                DateTextBox.SelectedDate = person.DateOfBirth;
                DateOfArrivalTextBox.Text = _person.DateOfArrival.ToString();
                CompletePatient.Visibility = Visibility.Collapsed;
            }
            else
            {
                DeletePatient.Visibility = Visibility.Collapsed;
                ModifyDate.Visibility = Visibility.Collapsed;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete the selected patient?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                PersonDataProvider.DeletePerson(_person.Id);
                DialogResult = true;
                Close();
            }
        }

        private bool ValidatePerson()
        {
            if (string.IsNullOrEmpty(DateOfArrivalTextBox.Text))
            {
                MessageBox.Show("Please add diagnosis!");
                return false;
            }
            return true;
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
