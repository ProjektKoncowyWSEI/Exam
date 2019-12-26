using ExamContract.Auth;
using ExamMainDataBaseAPI.Auth;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    [KeyAuthorize(Helpers.RoleEnum.admin)]
    public class ApiKeyRepo 
    {
        private readonly Context context;
        private readonly IHttpContextAccessor contextAccessor;       

        public ApiKeyRepo(Context context, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
        }
        public bool CheckApiKey(string key)
        {
            return context.Keys.Any(k => k.Name == key);
        }    
        private bool isAdmin(string name)
        {
            return context.Keys.Any(k => k.Name == name && k.Role == RoleEnum.admin.ToString());
        }
        private string getKey => contextAccessor.HttpContext.Request.Headers[GlobalHelpers.ApiKey];
       
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

        public async Task<List<string>> GetKeysAsync()
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            return await context.Keys
                .Where(a => a.ExpirationDate >= DateTime.Now)
                .Select(a => a.Name).ToListAsync();
        }
        public async Task<List<Key>> GetListAsync()
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            return await context.Keys.ToListAsync();
        }
        public async Task<Key> GetAsync(string key)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            return await context.Keys.SingleOrDefaultAsync(k => k.Name == key);
        }
        public async Task<string> AddAsync(Key item)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            try
            {
                await context.Keys.AddAsync(item);
                return item.Name;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<bool> UpdateAsync(Key item)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            try
            {
                var dbItem = await GetAsync(item.Name);
                dbItem.Role = item.Role;
                dbItem.ExpirationDate = item.ExpirationDate;
                await Task.Run(() => context.Keys.Update(dbItem));
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> DeleteAsync(string name)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            var item = await GetAsync(name);
            if (item != null)
            {
                try
                {
                    await Task.Run(() => context.Keys.Remove(item));
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public async Task SaveChangesAsync()
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            await context.SaveChangesAsync();
        }
    }
}
