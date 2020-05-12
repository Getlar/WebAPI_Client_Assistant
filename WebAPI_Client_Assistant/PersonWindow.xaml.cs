using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
                DateOfArrivalTextBox.Foreground = Brushes.Gray;
                FirstNameTextBox.Foreground = Brushes.Gray;
                LastNameTextBox.Foreground = Brushes.Gray;
                SocialSecurityNumberTextBox.Foreground = Brushes.Gray;
                AddressTextBox.Foreground = Brushes.Gray;
                ComplaintTextBox.Foreground = Brushes.Gray;
                DeletePatient.Visibility = Visibility.Collapsed;
                ModifyDate.Visibility = Visibility.Collapsed;
                FirstNameTextBox.Text = "First Name...";
                LastNameTextBox.Text = "Last Name...";
                AddressTextBox.Text = "Postal Code, City, Street Address...";
                ComplaintTextBox.Text = "Symptoms...";
                SocialSecurityNumberTextBox.Text = "XXX XXX XXX";
                DateOfArrivalTextBox.Text = "yyyy-MM-dd HH:mm";
            }
        }

        private bool ValidatePatient()
        {
            if (FirstNameTextBox.Text.ToString().Equals("First Name..."))
            {
                MessageBox.Show("Please add first name!", "Field Missing", MessageBoxButton.OK,MessageBoxImage.Warning);
                return false;
            }
            if (LastNameTextBox.Text.ToString().Equals("Last Name..."))
            {
                MessageBox.Show("Please add last name!", "Field Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!DateTextBox.SelectedDate.HasValue)
            {
                MessageBox.Show("Please add birth date!", "Field Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (AddressTextBox.Text.ToString().Equals("Postal Code, City, Street Address..."))
            {
                MessageBox.Show("Please add address!", "Field Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (SocialSecurityNumberTextBox.Text.ToString().Equals("XXX XXX XXX"))
            {
                MessageBox.Show("Please add ssn!", "Field Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (ComplaintTextBox.Text.ToString().Equals("Symptoms..."))
            {
                MessageBox.Show("Please add complaint!", "Field Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (DateOfArrivalTextBox.Text.ToString().Equals("yyyy-MM-dd HH:mm"))
            {
                MessageBox.Show("Please add arrival date!", "Field Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public bool CheckDate()
        {
            if (string.IsNullOrEmpty(DateOfArrivalTextBox.Text))
            {
                MessageBox.Show("Please add arrival date!","Missing Date",MessageBoxButton.OK,MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        public bool ValidateName(string name)
        {
            
            bool result = true;
            string[] Subnames = name.Split(' ');
            foreach (string subname in Subnames)
            {
                if(result == false)
                {
                    break;
                }
                if (subname.Length == 0)
                {
                    result = false;
                    break;
                }
                if (Char.IsUpper(subname[0]) == false)
                {
                    result = false;
                    break;
                }
                for (int i = 1; i < subname.Length; i++)
                {
                    if (char.IsUpper(subname[i]))
                    {
                        result = false;
                        break;
                    }
                }
                if (Regex.IsMatch(subname, @"^[a-zA-Z]+$") != true)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public bool ValidateDate(string date)
        {
            bool result = true;
            DateTime DateChecker;
            if (DateTime.TryParse(date, out DateChecker) == false)
            {
                result = false;
            }
            else if (date.Length == 16 || date.Length == 19)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            if (result != false)
            {
                if (DateChecker.Ticks < DateTime.Now.Ticks)
                {
                    result = false;
                }
                IList<Person> person = PersonDataProvider.GetPeople();
                var match = person.Where(p => p.DateOfArrival == DateChecker);
                IList<Person> act = match.ToList();
                if(act.Count != 0)
                {
                    return false;
                }
            }
            return result;
        }

        public bool ValidateSocialSecurityNumber(string s)
        {
            bool result = true;
            string[] Parts = s.Split(' ');
            if (Parts.Length != 3)
            {
                result = false;
            }
            foreach (string str in Parts)
            {
                if (str.Length != 3)
                {
                    result = false;
                    break;
                }
                int n;
                if (int.TryParse(str, out n) == false)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete the selected patient?", "Question", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                PersonDataProvider.DeletePerson(_person.Id);
                DialogResult = true;
                Close();
            }
        }

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateDate(DateOfArrivalTextBox.Text.ToString()) && CheckDate())
            {
                DateTime date = DateTime.Parse(DateOfArrivalTextBox.Text);
                _person.DateOfArrival = date;
                PersonDataProvider.UpdatePerson(_person);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Incorrect Date Format Or Date Is Occupied!", "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool FirstNameValidated = ValidateName(FirstNameTextBox.Text.ToString());
            bool LastNameValidated = ValidateName(LastNameTextBox.Text.ToString());
            bool SSNValidated = ValidateSocialSecurityNumber(SocialSecurityNumberTextBox.Text.ToString());
            bool DateValidated = ValidateDate(DateOfArrivalTextBox.Text.ToString());
            if (ValidatePatient())
            {
                if (FirstNameValidated)
                {
                    if (LastNameValidated)
                    {
                        if (SSNValidated)
                        {
                            if (DateValidated)
                            {
                                _person = new Person();
                                _person.FirstName = FirstNameTextBox.Text;
                                _person.LastName = LastNameTextBox.Text;
                                _person.DateOfBirth = DateTextBox.SelectedDate.Value;
                                _person.Address = AddressTextBox.Text;
                                _person.SocialSecurityNumber = SocialSecurityNumberTextBox.Text;
                                _person.Complaint = ComplaintTextBox.Text;
                                DateTime date = DateTime.Parse(DateOfArrivalTextBox.Text);
                                _person.DateOfArrival = date;
                                PersonDataProvider.CreatePerson(_person);
                                DialogResult = true;
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Date Format Or Date Is Occupied!", "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect SSN Format!", "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Name Format!", "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect Name Format!", "Format Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void FirstNameTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (FirstNameTextBox.Text == "")
            {
                FirstNameTextBox.Text = "First Name...";
                FirstNameTextBox.Foreground = Brushes.Gray;
            }
        }

        private void FirstNameTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (FirstNameTextBox.Text == "First Name...")
            {
                FirstNameTextBox.Text = "";
                FirstNameTextBox.Foreground = Brushes.White;
            }
        }

        private void LastNameTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (LastNameTextBox.Text == "Last Name...")
            {
                LastNameTextBox.Text = "";
                LastNameTextBox.Foreground = Brushes.White;
            }
        }

        private void LastNameTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (LastNameTextBox.Text == "")
            {
                LastNameTextBox.Text = "Last Name...";
                LastNameTextBox.Foreground = Brushes.Gray;
            }
        }


        private void AddressTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (AddressTextBox.Text == "Postal Code, City, Street Address...")
            {
                AddressTextBox.Text = "";
                AddressTextBox.Foreground = Brushes.White;
            }
        }

        private void AddressTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AddressTextBox.Text == "")
            {
                AddressTextBox.Text = "Postal Code, City, Street Address...";
                AddressTextBox.Foreground = Brushes.Gray;
            }
        }

        private void SocialSecurityNumberTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (SocialSecurityNumberTextBox.Text == "XXX XXX XXX")
            {
                SocialSecurityNumberTextBox.Text = "";
                SocialSecurityNumberTextBox.Foreground = Brushes.White;
            }
        }

        private void SocialSecurityNumberTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (SocialSecurityNumberTextBox.Text == "")
            {
                SocialSecurityNumberTextBox.Text = "XXX XXX XXX";
                SocialSecurityNumberTextBox.Foreground = Brushes.Gray;
            }
        }

        private void ComplaintTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ComplaintTextBox.Text == "Symptoms...")
            {
                ComplaintTextBox.Text = "";
                ComplaintTextBox.Foreground = Brushes.White;
            }
        }

        private void ComplaintTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ComplaintTextBox.Text == "")
            {
                ComplaintTextBox.Text = "Symptoms...";
                ComplaintTextBox.Foreground = Brushes.Gray;
            }
        }

        private void DateOfArrivalTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (DateOfArrivalTextBox.Text == "yyyy-MM-dd HH:mm")
            {
                DateOfArrivalTextBox.Text = "";
                DateOfArrivalTextBox.Foreground = Brushes.White;
            }
        }

        private void DateOfArrivalTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (DateOfArrivalTextBox.Text == "")
            {
                DateOfArrivalTextBox.Text = "yyyy-MM-dd HH:mm";
                DateOfArrivalTextBox.Foreground = Brushes.Gray;
            }
        }
    }
}
