using ExamContract;
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
    public abstract class WebApiClient<T> where T : Entity
    {
        protected ILogger logger;
        protected string uri;
        protected HttpClient Client;
        public WebApiClient(ILogger logger, IConfiguration configuration, string connectionName, string uri, string apiKey = null)
        {
            string connStr = Environment.GetEnvironmentVariable(connectionName) ?? configuration.GetConnectionString(connectionName);
            Client = new HttpClient
            {
                BaseAddress = new Uri(connStr)
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (apiKey != null)
                Client.DefaultRequestHeaders.Add("api-key", Environment.GetEnvironmentVariable(apiKey) ?? configuration.GetValue<string>(apiKey));
            this.logger = logger;
            this.uri = uri;
        }
        public async virtual Task<T> GetAsync(int id, string shortUri = "")
        {
            try
            {
                var response = await Client.GetAsync($"{uri}{shortUri}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
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
        public async virtual Task<List<T>> GetListAsync(string login = null, bool? onlyActive = null, string shortUri = "")
        {
            try
            {                
                string fullUri = $"{uri}{shortUri}";
                if(login != null && onlyActive != true)
                    fullUri += $"?login={login}";
                else if (login != null && onlyActive == true)                
                    fullUri += $"?login={login}&onlyActive=true";
                else if (login == null && onlyActive == true)                
                    fullUri += $"?onlyActive=true";
                
                var  response = await Client.GetAsync(fullUri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                throw;
            }
        }
        public async virtual Task<List<T>> GetListAsync(int parentId, string shortUri = "")
        {
            try
            {
                var response = await Client.GetAsync($"{uri}{shortUri}/?parentId={parentId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GetListAsync()");
                throw;
            }
        }        
        public async virtual Task<T> AddAsync(T item, string shortUri = "")
        {
            try
            {
                var response = await Client.PostAsJsonAsync($"{uri}{shortUri}", item);             
                if (response.IsSuccessStatusCode)
                {
                    item.Id = Convert.ToInt32(response.Headers.Location.Segments.LastOrDefault());
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
        public async virtual Task<bool> UpdateAsync(T item, string shortUri = "")
        {
            try
            {
                var response = await Client.PutAsJsonAsync($"{uri}{shortUri}/{item.Id}", item);
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
        public async virtual Task<bool> DeleteAsync(int id, string shortUri = "")
        {
            try
            {
                var response = await Client.DeleteAsync($"{uri}{shortUri}/{id}");
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
        public async virtual Task<bool> WakeUp()
        {
            var response = await Client.GetAsync($"{uri}");
            return (response.IsSuccessStatusCode);            
        }
    }
}
