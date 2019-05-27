using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamTutorialsAPI.Interfaces
{
    public interface IApiKeyRepo
    {
        bool CheckApiKey(string key);
        Task<List<string>> GetKeys();
        Dictionary<string, string> GetDictionary();
    }
}
