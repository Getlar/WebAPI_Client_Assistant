using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI_Client_Assistant.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfArrival { get; set; }
        public string Address { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string Complaint { get; set; }
        public string Diagnosis { get; set; }


        public override string ToString()
        {
            DateTime today = DateTime.Now;
            if (today.ToShortDateString() == DateOfArrival.ToShortDateString())
            {
                return $"Today:          {DateOfArrival.ToShortTimeString()}   {FirstName} {LastName}";
            }
            else
            {
                return $"{DateOfArrival.ToShortDateString()}  {DateOfArrival.ToShortTimeString()}   {FirstName} {LastName}";
            }
        }
    }
}
