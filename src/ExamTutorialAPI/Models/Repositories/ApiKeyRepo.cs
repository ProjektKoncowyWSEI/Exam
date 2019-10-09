using ExamTutorialsAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamTutorialsAPI.Models.Repositories
{
    public class ApiKeyRepo : IApiKeyRepo

    {
        private readonly Context context;

        public ApiKeyRepo(Context context)
        {
            this.context = context;
        }
        public bool CheckApiKey(string key)
        {
            return context.Keys.Any(k => k.Name == key);
        }

        public Dictionary<string, string> GetDictionary()
        {
            var list = context.Keys.Where(a => a.ExpirationDate >= DateTime.Now).ToList();
            var names = list.Select(a => a.Name).Distinct().ToList();
            var dictionary = new Dictionary<string, string>();
            names.ForEach(n =>
            {
                dictionary.Add(n, list.First(k => k.Name == n).Role);
            });
            return dictionary;
        }

        public async Task<List<string>> GetKeys()
        {
            return await context.Keys
                .Where(a => a.ExpirationDate >= DateTime.Now)
                .Select(a => a.Name).ToListAsync();
        }
    }
}
