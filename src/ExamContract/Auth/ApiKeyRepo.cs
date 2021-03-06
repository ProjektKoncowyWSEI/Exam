﻿using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamContract.Auth
{    
    public sealed class ApiKeyRepo 
    {
        private readonly DbContext context;
        private readonly IHttpContextAccessor contextAccessor;
        private DbSet<Key> dbSet = null;

        public ApiKeyRepo(DbContext context, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.contextAccessor = contextAccessor;
            dbSet = context.Set<Key>();
        }
        public bool CheckApiKey(string key)
        {
            return dbSet.Any(k => k.Name == key);
        }    
        private bool isAdmin(string name)
        {
            return dbSet.Any(k => k.Name == name && k.Role == RoleEnum.admin.ToString());
        }
        private string getKey => contextAccessor.HttpContext.Request.Headers[GlobalHelpers.ApiKey];
               
        public string GetRole(string name)
        {
            return dbSet.Where(a => a.ExpirationDate >= DateTime.Now && a.Name == name).Select(a => a.Role).FirstOrDefault();
        }
        [KeyAuthorize(RoleEnum.admin)]
        public async Task<List<string>> GetKeysAsync()
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            return await dbSet
                .Where(a => a.ExpirationDate >= DateTime.Now)
                .Select(a => a.Name).ToListAsync();
        }
        [KeyAuthorize(RoleEnum.admin)]
        public async Task<List<Key>> GetListAsync()
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            return await dbSet.ToListAsync();
        }
        [KeyAuthorize(RoleEnum.admin)]
        public async Task<Key> GetAsync(string key)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            return await dbSet.SingleOrDefaultAsync(k => k.Name == key);
        }
        [KeyAuthorize(RoleEnum.admin)]
        public async Task<string> AddAsync(Key item)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            try
            {
                await dbSet.AddAsync(item);
                return item.Name;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [KeyAuthorize(RoleEnum.admin)]
        public async Task<bool> UpdateAsync(Key item)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            try
            {
                var dbItem = await GetAsync(item.Name);
                dbItem.Role = item.Role;
                dbItem.ExpirationDate = item.ExpirationDate;
                await Task.Run(() => dbSet.Update(dbItem));
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [KeyAuthorize(RoleEnum.admin)]
        public async Task<bool> DeleteAsync(string name)
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            var item = await GetAsync(name);
            if (item != null)
            {
                try
                {
                    await Task.Run(() => dbSet.Remove(item));
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        [KeyAuthorize(RoleEnum.admin)]
        public async Task SaveChangesAsync()
        {
            if (!isAdmin(getKey))
                throw new Exception(GlobalHelpers.AccesDenied);
            await context.SaveChangesAsync();
        }
    }
}
