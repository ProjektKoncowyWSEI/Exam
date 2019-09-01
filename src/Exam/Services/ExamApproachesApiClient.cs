using ExamContract.MainDbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class ExamApproachesApiClient 
    {
        protected ILogger logger;
        protected string uri;
        protected HttpClient Client;
        public ExamApproachesApiClient(ILogger logger, IConfiguration configuration, string connectionName = "MainDbAPIConnection", string uri = "ExamApproaches", string apiKey = null)
        {
            string connStr = Environment.GetEnvironmentVariable(connectionName) ?? configuration.GetConnectionString(connectionName);
            Client = new HttpClient
            {
                BaseAddress = new Uri(connStr)
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (apiKey != null)
                Client.DefaultRequestHeaders.Add("api-key", configuration.GetValue<string>(apiKey));
            this.logger = logger;
            this.uri = uri;
        }
        public async virtual Task<ExamApproache> GetAsync(int examId, string login)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/Get/{examId}/{login}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ExamApproache>(content);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }
        public async virtual Task<List<ExamApproache>> GetListAsync(int examId)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}?examId={examId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ExamApproache>>(content);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetListAsync(examId)");
                return null;
            }
        }
        public async virtual Task<List<ExamApproache>> GetListAsync(string login)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}?login={login}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ExamApproache>>(content);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetListAsync(login)");
                return null;
            }
        }
        public async virtual Task<ExamApproache> AddAsync(ExamApproache item)
        {
            try
            {
                var response = await Client.PostAsJsonAsync($"{uri}/Post", item);
                if (response.IsSuccessStatusCode)
                {                    
                    return item;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }
        public async virtual Task<bool> UpdateAsync(ExamApproache item)
        {
            try
            {
                var response = await Client.PutAsJsonAsync($"{uri}/{item.ExamId}", item);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return false;
            }
        }
    }
}
