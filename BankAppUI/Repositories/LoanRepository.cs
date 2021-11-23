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
    public interface ILoanRepository
    {
        Task<List<Loan>> GetLoans(int accountId);

        void UpdateLoanStatus(int loanId);

        Task<Loan> PostLoan(Loan loan);
    }
    public class LoanRepository : ILoanRepository
    {
        public async Task<List<Loan>> GetLoans(int accountId)
        {
            string url = "https://localhost:44336/loans/" + accountId;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();

                    List<Loan> loans = JsonConvert.DeserializeObject<List<Loan>>(json);
                    return loans;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Loan> PostLoan(Loan loan)
        {
            string url = "https://localhost:44336/loans";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(loan);

                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, data);

                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<Loan>(responseString);


                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async void UpdateLoanStatus(int loanId)
        {
            string url = "https://localhost:44336/loans?loanId=" + loanId;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(loanId);

                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(url, data);

                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
