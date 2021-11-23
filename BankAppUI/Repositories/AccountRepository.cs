using BankAppUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankAppUI.Repositories
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccounts(int customerId);

        void PostAccount(Account account, int customerId);
    }
    public class AccountRepository : IAccountRepository
    {
        public async Task<List<Account>> GetAccounts(int customerId)
        {
            string url = "https://localhost:44336/accounts/" + customerId;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(json);
                    return accounts;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void PostAccount(Account account, int customerId)
        {
            string url = "https://localhost:44336/accounts";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(account);

                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, data);

                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();

                    var newAccount = JsonConvert.DeserializeObject<Account>(responseString);

                    if (response.IsSuccessStatusCode)
                    {
                        Disposition disposition = new();

                        disposition.AccountId = newAccount.AccountId;
                        disposition.CustomerId = customerId;

                        string dispositionUrl = "https://localhost:44336/dispositions";

                        var dispositionJson = JsonConvert.SerializeObject(disposition);

                        var dispositionData = new StringContent(dispositionJson, Encoding.UTF8, "application/json");

                        HttpResponseMessage dispositionResponse = await client.PostAsync(dispositionUrl, dispositionData);

                        dispositionResponse.EnsureSuccessStatusCode();

                    }
                }

                
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
