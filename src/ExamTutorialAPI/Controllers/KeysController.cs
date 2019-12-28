﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.Auth;
using ExamContract.TutorialModels;
using ExamTutorialsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamTutorialsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [KeyAuthorize(RoleEnum.admin)]
    public class KeysController : KeyApiController
    {
        public KeysController(ApiKeyRepo repo) : base(repo)
        {
        }
    }
}
