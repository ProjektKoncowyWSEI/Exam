using ExamContract.MainDbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using exam = ExamContract.MainDbModels.Exam;

namespace Exam.Services
{
    public class ExamsQuestionsAnswersApiClient : WebApiClient<exam>
    {
        public ExamsQuestionsAnswersApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "ExamsQuestionsAnswers")
        {
        }
        public override Task<exam> GetAsync(int id)
        {
            return GetByIdAsync(id);
        }
        public async Task<exam> GetByIdAsync(int id)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/GetById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<exam>(content);
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
        public async Task<exam> GetByCodeAsync(string code)
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/GetByCode/{code}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<exam>(content);
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
        public override async Task<exam> AddAsync(exam item)
        {
            try
            {
                var response = await Client.PostAsJsonAsync($"{uri}/Post", item);
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
        public override async Task<bool> UpdateAsync(exam item)
        {
            try
            {
                var response = await Client.PutAsJsonAsync($"{uri}/Put/{item.Id}", item);
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
