﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ExamContract.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [KeyAuthorize(RoleEnum.admin)]
    public abstract class KeyApiController : ControllerBase
    {
        private readonly ApiKeyRepo repo;

        public KeyApiController(ApiKeyRepo repo)
        {
            this.repo = repo;
        }
        [HttpGet("{name}")]
        public virtual async Task<ActionResult<Key>> Get(string name)
        {
            var item = await repo.GetAsync(name);
            if (item == null)
                return NotFound();
            return item;
        }
        [HttpGet()]
        public virtual async Task<ActionResult<List<Key>>> GetList()
        {
            return await repo.GetListAsync();
        }
        [HttpPut("{name}")]
        public virtual async Task<IActionResult> Put(string name, Key item)
        {
            if (name != item.Name)
            {
                return BadRequest();
            }
            await repo.UpdateAsync(item);
            try
            {
                await repo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!repo.CheckApiKey(name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public virtual async Task<ActionResult<Key>> Post(Key item)
        {
            try
            {
                await repo.AddAsync(item);
                await repo.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { name = item.Name }, item);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete("{name}")]
        public virtual async Task<ActionResult<Key>> Delete(string name)
        {
            var item = await repo.GetAsync(name);
            if (item == null)
            {
                return NotFound();
            }

            await repo.DeleteAsync(item.Name);
            await repo.SaveChangesAsync();
            return item;
        }
    }
}
