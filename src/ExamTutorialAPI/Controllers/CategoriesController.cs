﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamTutorialsAPI.DAL;
using ExamTutorialsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamTutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : MyBaseController<Category>
    {
        public CategoriesController(Repository<Category> repo) : base(repo)
        {
           
        }
    }
}