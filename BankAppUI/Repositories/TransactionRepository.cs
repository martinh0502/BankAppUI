using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BankAppUI.Models;

namespace BankAppUI.Repositories
{
    public interface ITransactionRepository
    {
        public void Transfer(int account, int targetAccount, int amount);

        public Task<List<Transaction>> GetTransactions(int accountId);
    }
    public class TransactionRepository:ITransactionRepository
    {
        public async Task<List<Transaction>> GetTransactions(int accountId)
        {
            string url = "https://localhost:44336/transactions/" + accountId;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    List<Transaction> transactions = JsonConvert.DeserializeObject<List<Transaction>>(json);
                    return transactions;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    

        public async void Transfer(int account, int targetAccount, int amount)
        {
            string url = "https://localhost:44336/transactions?account=" + account + "&targetAccount=" + targetAccount + "&amount=" + amount ;
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    HttpResponseMessage response = await client.SendAsync(request);

                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
