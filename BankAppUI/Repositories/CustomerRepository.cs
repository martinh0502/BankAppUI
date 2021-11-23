using BankAppUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankAppUI.Repositories
{
    public interface ICustomerRepo
    {
        Task<Customer> GetCustomer(int customerId);

        Task<Customer> CreateCustomer(Customer customer);
    }
    public class CustomerRepository : ICustomerRepo
    {
        public async Task<Customer> CreateCustomer(Customer customer)
        {
            string url = "https://localhost:44336/customers";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(customer);

                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, data);

                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Customer>(responseString);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            string url = "https://localhost:44336/customers/" + customerId;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    Customer customer = JsonConvert.DeserializeObject<Customer>(json);
                    return customer;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
