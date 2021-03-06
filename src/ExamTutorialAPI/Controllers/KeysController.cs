﻿using ExamContract.Auth;
using Microsoft.AspNetCore.Mvc;

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
