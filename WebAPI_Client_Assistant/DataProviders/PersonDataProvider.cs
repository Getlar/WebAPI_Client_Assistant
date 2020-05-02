using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPI_Client_Assistant.Models;

namespace WebAPI_Client_Assistant.DataProviders
{
    public static class PersonDataProvider
    {
        private static string url = "http://localhost:5000/api/person";

        public static IList<Person> GetPeople()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    var rawData = response.Content.ReadAsStringAsync().Result;
                    var people = JsonConvert.DeserializeObject<IList<Person>>(rawData);
                    return people;
                }
                else
                {
                    throw new InvalidOperationException(response.StatusCode.ToString());
                }
            };
        }

        public static void CreatePerson(Person person)
        {
            using (var client = new HttpClient())
            {
                var rawData = JsonConvert.SerializeObject(person);
                var content = new StringContent(rawData, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(response.Content.ToString());
                }
            }
        }

        public static void UpdatePerson(Person person)
        {
            using (var client = new HttpClient())
            {
                var rawData = JsonConvert.SerializeObject(person);
                var content = new StringContent(rawData, Encoding.UTF8, "application/json");
                var response = client.PutAsync(url + "/" + person.Id, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(response.Content.ToString());
                }
            }
        }

        public static void DeletePerson(int id)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync(url + "/" + id).Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(response.Content.ToString());
                }
            }
        }
    }
}
