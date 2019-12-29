using ExamContract.MainDbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class UsersMainDbApiClient : WebApiClient<User>
    {
        public UsersMainDbApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "MainDbAPIConnection", "Users", "Exam_MainDbApiKey")
        {
        }       
        public async override Task<List<User>> GetListAsync(int parentId, string shortUri = "")
        {
            try
            {
                var response = await Client.GetAsync($"{uri}/GetUsersForExam/{parentId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<User>>(content);
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
        public async override Task<List<User>> GetListAsync(string login = null, bool? onlyActive = null, string shortUri = "")
        {
            return await base.GetListAsync(login, onlyActive, "/Get");
        }
        public async override Task<User> GetAsync(int id, string shortUri = "")
        {
            return await base.GetAsync(id, "/Get");
        }
        public async override Task<User> AddAsync(User item, string shortUri = "")
        {
            return await base.AddAsync(item, "/Post");
        }
        public async override Task<bool> UpdateAsync(User item, string shortUri = "")
        {
            return await base.UpdateAsync(item, "/Put");
        }
        public async override Task<bool> DeleteAsync(int id, string shortUri = "")
        {
            return await base.DeleteAsync(id, "/Delete");
        }
    }
}
