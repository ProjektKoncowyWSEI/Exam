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
        private readonly ILogger logger;
        private readonly string uri;
        protected HttpClient Client;
        public WebApiClient(ILogger logger, IConfiguration configuration, string connectionName, string uri, string apiKey = null)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(configuration.GetConnectionString(connectionName))
            };
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (apiKey != null)
                Client.DefaultRequestHeaders.Add("api-key", configuration.GetValue<string>(apiKey));
            this.logger = logger;
            this.uri = uri;
        }
        public async virtual Task<T> GetAsync(int id)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/{id}");
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
                logger.LogError(ex, "GetAsync(int id)");
                throw;
            }
        }
        public async virtual Task<List<T>> GetListAsync()
        {
            try
            {
                var response = await Client.GetAsync(uri);
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
        public async virtual Task<List<T>> GetListAsync(int page = 1, int? pageLocalSize = null)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/{page}/{pageLocalSize}");
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
                logger.LogError(ex, "GetListAsync(int page = 1, int? pageLocalSize = null)");
                throw;
            }
        }
        public async virtual Task<T> AddAsync(T item)
        {
            try
            {
                var response = await Client.PostAsJsonAsync(uri, item);
                if (response.IsSuccessStatusCode)
                {
                    return item;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, item.Login);
                throw;
            }
        }
        public async virtual Task<bool> UpdateAsync(T item)
        {
            try
            {
                var response = await Client.PutAsJsonAsync($"{uri}/{item.Id}", item);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, item.Login);
                throw;
            }
        }
        public async virtual Task<bool> DeleteAsync(int id)
        {
            try
            {
                var response = await Client.DeleteAsync($"{uri}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DeleteAsync(int id)");
                throw;
            }
        }
    }
}
