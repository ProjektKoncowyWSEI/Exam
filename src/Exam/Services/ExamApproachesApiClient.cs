using ExamContract.MainDbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class ExamApproachesApiClient
    {
        protected ILogger logger;
        protected string uri;
        protected HttpClient ExamApproachesClient;
        public ExamApproachesApiClient(ILogger logger, IConfiguration configuration, string connectionName = "MainDbAPIConnection", string uri = "ExamApproaches", string apiKey = "Exam_MainDbApiKey")
        {
            string connStr = Environment.GetEnvironmentVariable(connectionName) ?? configuration.GetConnectionString(connectionName);
            ExamApproachesClient = new HttpClient
            {
                BaseAddress = new Uri(connStr)
            };
            ExamApproachesClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (apiKey != null)
                ExamApproachesClient.DefaultRequestHeaders.Add("api-key", Environment.GetEnvironmentVariable(apiKey) ?? configuration.GetValue<string>(apiKey));
            this.logger = logger;
            this.uri = uri;
        }
        public async virtual Task<ExamApproache> GetAsync(int examId, string login)
        {
            try
            {
                var response = await ExamApproachesClient.GetAsync($"{uri}/Get/{examId}/{login}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ExamApproache>(content);
                }
                else
                {
                    return null;
                    //throw new Exception(response.StatusCode.ToString());
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
                var response = await ExamApproachesClient.GetAsync($"{uri}?examId={examId}");
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
                var response = await ExamApproachesClient.GetAsync($"{uri}?login={login}");
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
                var response = await ExamApproachesClient.PostAsJsonAsync($"{uri}/Post", item);
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
                var response = await ExamApproachesClient.PutAsJsonAsync($"{uri}/{item.ExamId}", item);
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
        #region Results
        public async virtual Task<ExamApproacheResult> GetResultAsync(int examId, string login)
        {
            try
            {
                var response = await ExamApproachesClient.GetAsync($"{uri}/GetResultGrouped/{login}/{examId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ExamApproacheResult>(content);
                }
                else
                {
                    return null;
                    //throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }
        public async virtual Task<List<ExamApproacheResult>> GetResultsAsync(int examId)
        {
            try
            {
                var response = await ExamApproachesClient.GetAsync($"{uri}/GetResultsGrouped/{examId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ExamApproacheResult>>(content);
                }
                else
                {
                    return null;
                    //throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }
        public async virtual Task<ExamApproacheResult> AddResultAsync(ExamApproacheResult item)
        {
            try
            {
                var response = await ExamApproachesClient.PostAsJsonAsync($"{uri}/PostResult", item);
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
        public async Task<int> AddResultsAsync(List<ExamApproacheResult> items)
        {
            try
            {
                var count = 0;
                foreach (var item in items)
                {
                    var response = await ExamApproachesClient.PostAsJsonAsync($"{uri}/PostResult", item);
                    if (response.IsSuccessStatusCode)
                        count++;
                }                
                return count;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return -1;
            }
        }
        #endregion
    }
}
