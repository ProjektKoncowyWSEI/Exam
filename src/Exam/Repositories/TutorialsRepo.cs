using Exam.IRepositories;
using ExamTutorialsAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Exam.Repositories
{
    public class TutorialsRepo : ITutorialsRepo

    {
        HttpClient client = new HttpClient();
        const string TutorialsUri = "Tutorials";
        public int TotalItems { get; set; }

        public TutorialsRepo()
        {
            //nie wiem jaki ma być adres?
            client.BaseAddress = new Uri("");
            
        }

        public async Task<Tutorial> Get(int id)
        {
            var result = new Tutorial();
            try
            {
                var response = await client.GetAsync($"{TutorialsUri}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Tutorial>(content);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;

        }


    }
}
