﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamMainDataBaseAPI.Models;
using ExamMainDataBaseAPI.DAL;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using ExamContract.MainDbModels;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : MyBaseController<User>
    {
        public UsersController(Repository<User> repo) : base(repo)
        {           
        }        
    }
}