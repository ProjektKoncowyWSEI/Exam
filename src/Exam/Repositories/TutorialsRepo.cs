using Exam.IRepositories;
using ExamContract.TutorialModels;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;

        public int TotalItems { get; set; }

        public TutorialsRepo(ILogger logger)
        {
            //nie jestem pewna adresu?
            client.BaseAddress = new Uri("http://localhost:54321/api/tutorials");
            this.logger = logger;
        }

        public async Task<Tutorial> Add(Tutorial item)
        {
            try
            {
                var response = await client.PostAsJsonAsync(TutorialsUri, item);
                if (response.IsSuccessStatusCode)
                {
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

        public async Task<bool> Delete(int id)
        {
            try
            {
                var response = await client.DeleteAsync($"{TutorialsUri}/{id}");
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
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                throw;
            }
            return result;

        }

        public async Task<List<Tutorial>> GetList(int page = 1, int? pageLocalSize = null)
        {
            page = page < 1 ? 1 : page;
            int size = pageLocalSize ?? 0;
            var result = new List<Tutorial>();
            try
            {
                var response = await client.GetAsync(TutorialsUri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Tutorial>>(content);
                    TotalItems = result.Count();
                    if (pageLocalSize != null)
                        result = result.Skip((page - 1) * size).Take(size).ToList();
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
            return result;
        }

        public async Task<List<Tutorial>> GetList()
        {
            var result = new List<Tutorial>();
            try
            {
                var response = await client.GetAsync(TutorialsUri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Tutorial>>(content);
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
            return result;
        }

        public async Task<bool> Update(Tutorial item)
        {
            try
            {
                var response = await client.PutAsJsonAsync($"{TutorialsUri}/{item.Id}", item);
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
