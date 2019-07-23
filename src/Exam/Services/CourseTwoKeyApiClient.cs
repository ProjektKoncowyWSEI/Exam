using ExamContract.CourseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace Exam.Services
{
    public class CourseTwoKeyApiClient<T> where T: class, ICourseTwoKey
    {
        protected ILogger logger;
        protected string uri;
        protected HttpClient Client;
        public CourseTwoKeyApiClient(ILogger logger, IConfiguration configuration, string connectionName, string uri, string apiKey = null)
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
        public async virtual Task<T> GetAsync(int course, int id)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/{course}/{id}");
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
                throw;
            }
        }
        public async virtual Task<List<T>> GetListAsync(int? courseId = null, int? id = null)
        {
            try
            {
                HttpResponseMessage response;
                string fullUri = uri;
                if (courseId != null && id == null)
                    fullUri += $"?courseId={courseId}";
                else if (courseId != null && id != null)
                    fullUri += $"?courseId={courseId}&id={id}";
                else if (courseId == null && id != null)
                    fullUri += $"?id={id}";

                response = await Client.GetAsync(fullUri);
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
        public async virtual Task<List<T>> GetListAsync(int parrentId)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/?parentId={parrentId}");
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
        public async virtual Task<T> AddAsync(T item)
        {
            try
            {
                var response = await Client.PostAsJsonAsync(uri, item);
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
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                throw;
            }
        }
        public async virtual Task<bool> DeleteAsync(int course, int id)
        {
            try
            {
                var response = await Client.DeleteAsync($"{uri}/{course}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                throw;
            }
        }
    }
}
